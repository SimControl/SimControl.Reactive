// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using NLog;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils
{
    /// <summary>Test frame for writing asynchronous unit tests.</summary>
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public abstract class TestFrame
    {
        private static class NativeMethods
        {
            [DllImport("ntdll.dll", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
            internal static extern int NtQueryTimerResolution(out int minimumResolution, out int maximumResolution,
                                                              out int currentResolution);
        }

        [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline")]
        static TestFrame()
        {
            Assert.That(NativeMethods.NtQueryTimerResolution(out int minimumResolution, out int _, out int _),
                Is.EqualTo(0));
            MinTimerResolution = (minimumResolution + 9999)/10000; // round to guaranteed timer sleep interval in ms
        }

        #region Test SetUp/TearDown

        /// <summary>Onetime test setup.</summary>
        [Log, OneTimeSetUp]
        public void OneTimeSetUp()
        {
            oneTimeTestAdapters.Clear();

            ThrowPendingExceptions();

            InternationalCultureInfo.SetCurrentThreadCulture();
            InternationalCultureInfo.SetDefaultThreadCulture();

            if (Environment.GetEnvironmentVariable("NCrunch") != "1")
                // AppDomain.UnhandledException is handled by NCrunch as an error
                AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledExceptionHandler;

            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskExceptionHandler;
        }

        /// <summary>Onetime test tear down.</summary>
        [Log, OneTimeTearDown]
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public void OneTimeTearDown()
        {
            while (oneTimeTestAdapters.TryPop(out TestAdapter testAdapter))
                try { testAdapter.Dispose(); }
                catch (Exception e) { AddPendingException(e); }

            // force any unfinished and unreferenced tasks to terminate
            ForceGarbageCollection();

            if (Environment.GetEnvironmentVariable("NCrunch") != "1")
                // AppDomain.UnhandledException is handled by NCrunch as an error
                AppDomain.CurrentDomain.UnhandledException -= AppDomainUnhandledExceptionHandler;

            TaskScheduler.UnobservedTaskException -= TaskSchedulerUnobservedTaskExceptionHandler;

            ThrowPendingExceptions();
        }

        /// <summary>Setup test execution.</summary>
        [Log, SetUp]
        public void SetUp()
        {
            testAdapters.Clear();

            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), TestContext.CurrentContext.Test.FullName,
                nameof(Environment), Environment.Version, Environment.Is64BitProcess ? "x64" : "x86",
                TestContext.CurrentContext.TestDirectory, TestContext.CurrentContext.WorkDirectory);
        }

        /// <summary>Tear down test execution.</summary>
        [Log, TearDown]
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public void TearDown()
        {
            while (testAdapters.TryPop(out TestAdapter testAdapter))
                try { testAdapter.Dispose(); }
                catch (Exception e) { AddPendingException(e); }

            // force any unfinished and unreferenced tasks to terminate
            ForceGarbageCollection();

            ThrowPendingExceptions();
        }

        #endregion

        /// <summary>Context switch.</summary>
        /// <remarks>Forces the CLI to suspend thread execution.</remarks>
        public static void PermitContextSwitch() => Thread.Sleep(1);

        public static Task PermitContextSwitchAsync() => Task.Delay(1);

        /// <summary>Context switch.</summary>
        /// <remarks>Forces the CLI to suspend thread execution.</remarks>
        public static void ForceContextSwitch() => Thread.Sleep(MinTimerResolution);

        public static Task ForceContextSwitchAsync() => Task.Delay(MinTimerResolution);

        /// <summary>Disable timeouts if a debugger is attached.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static int DebugTimeout(int timeout) => Debugger.IsAttached ? int.MaxValue : timeout;

        /// <summary>Force garbage collection.</summary>
        public static void ForceGarbageCollection()
        {
            ForceContextSwitch();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>Invoke a private static method.</summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="args">A variable-length parameters list containing arguments.</param>
        [Obsolete("Refactor static singletons")] // TODO delete
        public static void InvokePrivateStaticMethod(Type type, string methodName, params object[] args) =>
            type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);

        /// <summary>Sets private static field.</summary>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException"></exception>
        [Obsolete("Refactor static singletons")] // TODO delete
        public static void SetPrivateStaticField(Type type, string field, object value)
        {
            if (field.Length == 0) throw new ArgumentException("Field name must not be empty", nameof(field));

            type.GetField(field, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
        }

        /// <summary>Adds an unhandled exception.</summary>
        /// <param name="exception">.</param>
        [Log]
        public void AddPendingException(Exception exception) =>
            Assert.That(pendingExceptions.Writer.TryWrite(exception));

        /// <summary>Catches any exception fired by a onetime tear down action.</summary>
        /// <param name="action">The action.</param>
        /// <remarks>The exception is re-thrown when all tear down actions are finished</remarks>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public void CatchOneTimeTearDownExceptions(Action action)
        {
            try { action(); }
            catch (Exception e) { AddPendingException(e); }
        }

        /// <summary>Catches any exception fired by a tear down action.</summary>
        /// <param name="action">The action.</param>
        /// <remarks>The exception is re-thrown when all tear down actions are finished</remarks>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public void CatchTearDownExceptions(Action action)
        {
            try { action(); }
            catch (Exception e) { AddPendingException(e); }
        }

        /// <summary>Register a test adapter for this class.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testAdapter">The test adapter.</param>
        /// <returns>The test adapter</returns>
        /// <remarks>Registered test adapters are automatically disposed during the class cleanup.</remarks>
        public T OneTimeRegisterTestAdapter<T>(T testAdapter) where T : TestAdapter
        {
            oneTimeTestAdapters.Push(testAdapter);
            return testAdapter;
        }

        /// <summary>Register a test adapter for this test.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testAdapter">The test adapter.</param>
        /// <returns>The test adapter</returns>
        /// <remarks>Registered test adapters are automatically disposed during the test cleanup.</remarks>
        public T RegisterTestAdapter<T>(T testAdapter) where T : TestAdapter
        {
            testAdapters.Push(testAdapter);
            return testAdapter;
        }

        /// <summary>Get the first pending exception.</summary>
        /// <returns>An exception./&gt;.</returns>
        public Task<Exception> TakePendingExceptionAsync() => pendingExceptions.Reader.ReadAsync().AsTask();

        public Exception? TryTakePendingException()
        {
            Exception e = null;

            if (pendingExceptions.Reader.TryRead(out e))
                return e;
            else
                return null;
        }

        [Log]
        private void AppDomainUnhandledExceptionHandler(object _, UnhandledExceptionEventArgs args) =>
            AddPendingException((Exception) args.ExceptionObject);

        [Log]
        private void TaskSchedulerUnobservedTaskExceptionHandler(object _, UnobservedTaskExceptionEventArgs args)
        {
            AddPendingException(args.Exception);
            args.SetObserved(); // as we have observed the exception, the process should not terminate
        }

        private void ThrowPendingExceptions()
        {
            var exceptions = new List<Exception>();

            while (pendingExceptions.Reader.TryRead(out Exception? e))
                exceptions.Add(e);

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }

        /// <summary>Test timeout for interactive tests in milliseconds.</summary>
        public const int InteractiveTimeout = 30000;

        /// <summary>The test timeout in milliseconds.</summary>
        public const int Timeout = 10000;

        /// <summary>The minimum time in milliseconds for a Windows thread switch.</summary>
        public static readonly int MinTimerResolution;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentStack<TestAdapter> oneTimeTestAdapters = new ConcurrentStack<TestAdapter>();
        private readonly Channel<Exception> pendingExceptions = Channel.CreateUnbounded<Exception>();
        private readonly ConcurrentStack<TestAdapter> testAdapters = new ConcurrentStack<TestAdapter>();
    }
}
