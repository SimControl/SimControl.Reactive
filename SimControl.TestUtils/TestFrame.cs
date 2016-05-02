// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Reactive;

namespace SimControl.TestUtils
{
    /// <summary>Test frame for writing asynchronous unit tests.</summary>
    public class TestFrame
    {
        private static class NativeMethods
        {
            [DllImport("ntdll.dll", SetLastError = true)]
            internal static extern int NtQueryTimerResolution(out int minimumResolution, out int maximumResolution,
                                                              out int currentResolution);
        }

        static TestFrame()
        {
            int minimumResolution;
            int maximumResolution;
            int currentResolution;

            Assert.AreEqual(0, NativeMethods.NtQueryTimerResolution(out minimumResolution, out maximumResolution, out currentResolution));
            MinTimerResolution = (minimumResolution + 9999)/10000; // round to guaranteed timer sleep interval in ms
        }

        /// <summary>Initializes a new instance of the <see cref="TestFrame"/> class.</summary>
        protected TestFrame() { }

        #region Additional test attributes

        /// <summary>Onetime test setup.</summary>
        [Log]
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            InternationalCultureInfo.SetCurrentThreadCulture();
            LogMethod.SetDefaultThreadCulture();

            unhandledAsyncException = null;

            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskExceptionHandler;

            oneTimeAdapters = new ConcurrentStack<TestAdapter>();
        }

        /// <summary>Onetime test tear down.</summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log]
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestAdapter tfa;
            while (oneTimeAdapters.TryPop(out tfa))
                try { tfa.Dispose(); }
                catch (Exception e) { SetUnhandledException(e); }

            oneTimeAdapters = null;

            AppDomain.CurrentDomain.UnhandledException -= AppDomainUnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException -= TaskSchedulerUnobservedTaskExceptionHandler;

            ThrowUnhandledException();
        }

        /// <summary>Setup test execution.</summary>
        [Log]
        [SetUp]
        public void SetUp()
        {
            testAdapters = new ConcurrentStack<TestAdapter>();

            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), nameof(Environment), DateTime.Now,
                Environment.Version, Environment.Is64BitProcess ? "x64" : "x86",
                TestContext.CurrentContext.Test.FullName, TestContext.CurrentContext.TestDirectory,
                TestContext.CurrentContext.WorkDirectory);
        }

        /// <summary>Tear down test execution.</summary>
        [Log]
        [TearDown]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void TearDown()
        {
            TestAdapter tfa;
            while (testAdapters.TryPop(out tfa))
                try { tfa.Dispose(); }
                catch (Exception e) { SetUnhandledException(e); }

            testAdapters = null;

            ThrowUnhandledException();
        }

        #endregion

        /// <summary>Asserts that exception is a <see cref="ContractException"/></summary>
        /// <param name="exception">.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static void AssertIsContractException(Exception exception)
        {
            Contract.Requires(exception != null);

            Assert.AreEqual(contractExceptionName, exception.GetType().FullName);
        }

        /// <summary>Disable timeouts if a debugger is attached.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static int DisableDebugTimeout(int timeout) => Debugger.IsAttached ? int.MaxValue : timeout;

        /// <summary>Invoke a private static method.</summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        public static void InvokePrivateStaticMethod(Type type, string methodName) //TODO remove
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(methodName));

            type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <param name="action">The action.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static void RunAssertTimeout(Action action)
        {
            Contract.Requires(action != null);

            action.RunAssertTimeout();
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="action">The action.</param>
        /// <param name="timeout">The timeout.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static void RunAssertTimeout(Action action, int timeout)
        {
            Contract.Requires(action != null);

            action.RunAssertTimeout(timeout);
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns>A T.</returns>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static T RunAssertTimeout<T>(Func<T> function)
        {
            Contract.Requires(function != null);

            return function.RunAssertTimeout();
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A T.</returns>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static T RunAssertTimeout<T>(Func<T> function, int timeout)
        {
            Contract.Requires(function != null);

            return function.RunAssertTimeout(timeout);
        }

        /// <summary>Sets private static field.</summary>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        public static void SetPrivateStaticField(Type type, string field, object value) // TODO remove
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(field));

            type.GetField(field, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
        }

        /// <summary>Catches any exception fired by a onetime tear down action.</summary>
        /// <param name="action">The action.</param>
        /// <remarks>The exception is re-thrown when all tear down actions are finished</remarks>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void CatchOneTimeTearDownExceptions(Action action)
        {
            Contract.Requires(action != null);

            try { action(); }
            catch (Exception e) { SetUnhandledException(e); }
        }

        /// <summary>Catches any exception fired by a tear down action.</summary>
        /// <param name="action">The action.</param>
        /// <remarks>The exception is re-thrown when all tear down actions are finished</remarks>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void CatchTearDownExceptions(Action action)
        {
            Contract.Requires(action != null);

            try { action(); }
            catch (Exception e) { SetUnhandledException(e); }
        }

        /// <summary>Remove an unhandled exception.</summary>
        [Log]
        public void ClearUnhandledException()
        {
            lock (locker)
                unhandledAsyncException = null;
        }

        /// <summary>Register a test adapter for this class.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testAdapter">The test adapter.</param>
        /// <returns>The test adapter</returns>
        /// <remarks>Registered test adapters are automatically disposed during the class cleanup.</remarks>
        public T OneTimeRegisterTestAdapter<T>(T testAdapter) where T : TestAdapter
        {
            Contract.Requires(testAdapter != null);

            oneTimeAdapters.Push(testAdapter);
            return testAdapter;
        }

        /// <summary>Register a test adapter for this test.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testAdapter">The test adapter.</param>
        /// <returns>The test adapter</returns>
        /// <remarks>Registered test adapters are automatically disposed during the test cleanup.</remarks>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public T RegisterTestAdapter<T>(T testAdapter) where T : TestAdapter
        {
            Contract.Requires(testAdapter != null);

            testAdapters.Push(testAdapter);
            return testAdapter;
        }

        /// <summary>Unhandled exception.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="exception">.</param>
        [Log]
        public void SetUnhandledException(Exception exception)
        {
            Contract.Requires(exception != null);

            if (exception.GetType() != typeof(ThreadAbortException))
            {
                lock (locker)
                    if (unhandledAsyncException == null)
                        unhandledAsyncException = exception;
                UnhandledExceptionEvent?.Invoke(this, new EventArgs<Exception>(exception));
            }
        }

        [Log]
        internal void TestDispatcherContextUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            SetUnhandledException(args.Exception);
        }

        [Log]
        private void AppDomainUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            SetUnhandledException((Exception) args.ExceptionObject);
        }

        [Log]
        private void TaskSchedulerUnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs args)
        {
            SetUnhandledException(args.Exception);
            args.SetObserved(); // as we have observed the exception, the process should not terminate
        }

        private void ThrowUnhandledException()
        {
            lock (locker)
                if (unhandledAsyncException != null)
                {
                    Exception e = unhandledAsyncException;
                    unhandledAsyncException = null;
                    throw e;
                }
        }

        /// <summary>Unhandled exception event.</summary>
        protected event EventHandler<EventArgs<Exception>> UnhandledExceptionEvent;

        /// <summary>The default timeout for interactive tests.</summary>
        /// <remarks>Returns int.MaxValue if a debugger is attached, otherwise 60 seconds.</remarks>
        public static int DefaultInteractiveTestTimeout { get; } = DisableDebugTimeout(60000);

        /// <summary>The test timeout in milliseconds.</summary>
        /// <remarks>Returns int.MaxValue if a debugger is attached, otherwise 10 seconds.</remarks>
        public static int DefaultTestTimeout { get; } = DisableDebugTimeout(10000);

        /// <summary>The minimum thread switch.</summary>
        public static readonly int MinTimerResolution;

        private const string contractExceptionName = "System.Diagnostics.Contracts.__ContractsRuntime+ContractException";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly object locker = new object();
        private ConcurrentStack<TestAdapter> oneTimeAdapters;
        private ConcurrentStack<TestAdapter> testAdapters;
        private Exception unhandledAsyncException;
    }
}
