﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NUnit.Framework;
using SimControl.Log;

[assembly: NonTestAssembly]

namespace SimControl.TestUtils
{
    /// <summary>Test frame for writing asynchronous unit tests.</summary>
    public abstract class TestFrame
    {
        #region Test SetUp/TearDown

        /// <summary>Onetime test setup.</summary>
        [Log, OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //UNDONE InternationalCultureInfo.SetCurrentThreadCulture();
            //UNDONE LogMethod.SetDefaultThreadCulture();

            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledExceptionHandler;
            //TODO not raised with NCrunch

            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskExceptionHandler; //TODO not raised

            oneTimeTestAdapters = new ConcurrentStack<TestAdapter>();
        }

        /// <summary>Onetime test tear down.</summary>
        [Log, OneTimeTearDown]
        public void OneTimeTearDown()
        {
            while (oneTimeTestAdapters.TryPop(out TestAdapter tfa))
                try { tfa.Dispose(); }
                catch (Exception e) { AddUnhandledException(e); }

            oneTimeTestAdapters = null;

            // force any unfinished and unreferenced tasks to terminate
            ContextSwitch();
            ForceGarbageCollection();

            AppDomain.CurrentDomain.UnhandledException -= AppDomainUnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException -= TaskSchedulerUnobservedTaskExceptionHandler;

            ThrowPendingExceptions();
        }

        /// <summary>Setup test execution.</summary>
        [Log, SetUp]
        public void SetUp()
        {
            testAdapters = new ConcurrentStack<TestAdapter>();

            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), TestContext.CurrentContext.Test.FullName,
                nameof(Environment), Environment.Version, Environment.Is64BitProcess ? "x64" : "x86",
                TestContext.CurrentContext.TestDirectory, TestContext.CurrentContext.WorkDirectory);
        }

        /// <summary>Tear down test execution.</summary>
        [Log, TearDown]
        public void TearDown()
        {
            while (testAdapters.TryPop(out TestAdapter tfa))
                try { tfa.Dispose(); }
                catch (Exception e) { AddUnhandledException(e); }

            testAdapters = null;

            // force any unfinished and unreferenced tasks to terminate
            ContextSwitch();
            ForceGarbageCollection();

            ThrowPendingExceptions();
        }

        #endregion

        public static bool IsContractException(Exception exception)
        {
            Contract.Requires(exception != null);

            return exception.GetType().FullName == contractExceptionName;
        }

        /// <summary>Context switch.</summary>
        /// <remarks>Forces the CLI to suspend thread execution.</remarks>
        public static void ContextSwitch() => Thread.Sleep(1);

        /// <summary>Disable timeouts if a debugger is attached.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static int DebugTimeout(int timeout) => Debugger.IsAttached ? int.MaxValue : timeout;

        /// <summary>Invoke either <see cref="Task.Delay"/> or <see cref="TaskEx.Delay".<s</summary>
        /// <param name="millisecondsDelay">The milliseconds delay.</param>
        /// <param name="token">(Optional) A token that allows processing to be cancelled.</param>
        /// <returns>An asynchronous result.</returns>
        public static Task Delay(int millisecondsDelay, CancellationToken token = default) =>
#if NET40 //TODO remove when NET40 is no more needed
            TaskEx.Delay(millisecondsDelay);
#else
            Task.Delay(millisecondsDelay);

#endif

        /// <summary>Force garbage collection.</summary>
        public static void ForceGarbageCollection()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>Invoke a private static method.</summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        public static void InvokePrivateStaticMethod(Type type, string methodName, params object[] args)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(methodName));

            _ = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);
        }

        /// <summary>Invoke either <see cref="Task.Run"/> or <see cref="TaskEx.Run".</summary>
        /// <param name="action">The action.</param>
        /// <returns>An asynchronous result.</returns>
        public static Task Run(Action action) => //TODO remove when NET40 is no more needed
#if NET40
            TaskEx.Run(action);
#else
            Task.Run(action);

#endif

        /// <summary>Sets private static field.</summary>
        /// <param name="type">The type.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        public static void SetPrivateStaticField(Type type, string field, object value)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(field));

            type.GetField(field, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
        }

        /// <summary>Adds an unhandled exception.</summary>
        /// <exception cref="ContractException">Thrown when a method Contract has been broken.</exception>
        /// <param name="exception">.</param>
        [Log]
        public void AddUnhandledException(Exception exception)
        {
            Contract.Requires(exception != null);

            if (exception.GetType() != typeof(ThreadAbortException)) //UNDONE why?
                pendingExceptions.Add(exception);
        }

        /// <summary>Catches any exception fired by a onetime tear down action.</summary>
        /// <param name="action">The action.</param>
        /// <remarks>The exception is re-thrown when all tear down actions are finished</remarks>
        public void CatchOneTimeTearDownExceptions(Action action)
        {
            Contract.Requires(action != null);

            try { action(); }
            catch (Exception e) { AddUnhandledException(e); }
        }

        /// <summary>Catches any exception fired by a tear down action.</summary>
        /// <param name="action">The action.</param>
        /// <remarks>The exception is re-thrown when all tear down actions are finished</remarks>
        public void CatchTearDownExceptions(Action action)
        {
            Contract.Requires(action != null);

            try { action(); }
            catch (Exception e) { AddUnhandledException(e); }
        }

        /// <summary>Register a test adapter for this class.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testAdapter">The test adapter.</param>
        /// <returns>The test adapter</returns>
        /// <remarks>Registered test adapters are automatically disposed during the class cleanup.</remarks>
        public T OneTimeRegisterTestAdapter<T>(T testAdapter) where T : TestAdapter
        {
            Contract.Requires(testAdapter != null);

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
            Contract.Requires(testAdapter != null);

            testAdapters.Push(testAdapter);
            return testAdapter;
        }

        /// <summary>Remove an unhandled exception.</summary>
        [Log]
        public Exception TakePendingExceptionAssertTimeout(int timeout = Timeout) => //UNDONE return Task
            pendingExceptions.TryTake(out Exception e, DebugTimeout(timeout)) ? e : null;

        [Log]
        private void AppDomainUnhandledExceptionHandler(object _, UnhandledExceptionEventArgs args) =>
            AddUnhandledException((Exception) args.ExceptionObject);

        [Log]
        private void TaskSchedulerUnobservedTaskExceptionHandler(object _, UnobservedTaskExceptionEventArgs args)
        {
            AddUnhandledException(args.Exception);
            args.SetObserved(); // as we have observed the exception, the process should not terminate
        }

        private void ThrowPendingExceptions()
        {
            var exceptions = new List<Exception>();

            while (pendingExceptions.TryTake(out Exception e)) exceptions.Add(e);

            if (exceptions.Count > 0) throw new AggregateException(exceptions);
        }

        /// <summary>Test timeout for interactive tests in milliseconds.</summary>
        /// <remarks>Returns int.MaxValue if a debugger is attached, otherwise 60 seconds.</remarks>
        public const int InteractiveTimeout = 30000;

        /// <summary>The test timeout in milliseconds.</summary>
        /// <remarks>Returns int.MaxValue if a debugger is attached, otherwise 10 seconds.</remarks>
        public const int Timeout = 10000;

        private const string contractExceptionName = "System.Diagnostics.Contracts.__ContractsRuntime+ContractException";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ConcurrentStack<TestAdapter> oneTimeTestAdapters;
        private BlockingCollection<Exception> pendingExceptions = new BlockingCollection<Exception>();
        private ConcurrentStack<TestAdapter> testAdapters;
    }
}
