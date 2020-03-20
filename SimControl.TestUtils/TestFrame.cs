// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
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
            //LogMethod.SetDefaultThreadCulture();

            unhandledAsyncException = null;

            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskExceptionHandler;

            oneTimeAdapters = new ConcurrentStack<TestAdapter>();
        }

        /// <summary>Onetime test tear down.</summary>
        [Log, OneTimeTearDown]
        public void OneTimeTearDown()
        {
            while (oneTimeAdapters.TryPop(out TestAdapter tfa))
                try { tfa.Dispose(); }
                catch (Exception e) { SetUnhandledException(e); }

            oneTimeAdapters = null;

            AppDomain.CurrentDomain.UnhandledException -= AppDomainUnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException -= TaskSchedulerUnobservedTaskExceptionHandler;

            ThrowUnhandledException();
        }

        /// <summary>Setup test execution.</summary>
        [Log, SetUp]
        public void SetUp()
        {
            testAdapters = new ConcurrentStack<TestAdapter>();

            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), nameof(Environment), DateTime.Now,
                Environment.Version, Environment.Is64BitProcess ? "x64" : "x86",
                TestContext.CurrentContext.Test.FullName, TestContext.CurrentContext.TestDirectory,
                TestContext.CurrentContext.WorkDirectory);
        }

        /// <summary>Tear down test execution.</summary>
        [Log, TearDown]
        public void TearDown()
        {
            while (testAdapters.TryPop(out TestAdapter tfa))
                try
                { tfa.Dispose(); }
                catch (Exception e) { SetUnhandledException(e); }

            testAdapters = null;

            ThrowUnhandledException();
        }

        #endregion

        /// <summary>Asserts that exception is a <see cref="ContractException"/></summary>
        /// <param name="exception">.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static void AssertIsContractException(Exception exception) //TODO
        {
            Contract.Requires(exception != null);

            Assert.That(exception.GetType().FullName, Is.EqualTo(contractExceptionName));
        }

        public static void ContextSwitch() => Thread.Sleep(1);

        public static Task ContextSwitchAsync() //TODO remove?
        {
#if NET40
            return TaskEx.Delay(1);
#else
            return Task.Delay(1);
#endif
        }

        /// <summary>Disable timeouts if a debugger is attached.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static int DebugTimeout(int timeout) => Debugger.IsAttached ? int.MaxValue : timeout;

        public static Task Delay(int millisecondsDelay) //TODO remove when NET40 is no more needed
        {
#if NET40
            return TaskEx.Delay(millisecondsDelay);
#else
            return Task.Delay(millisecondsDelay);
#endif
        }

        /// <summary>Invoke a private static method.</summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        public static void InvokePrivateStaticMethod(Type type, string methodName) //TODO remove
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(methodName));

            _ = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        }

        public static Task Run(Action action) //TODO remove when NET40 is no more needed
        {
#if NET40
            return TaskEx.Run(action);
#else
            return Task.Run(action);
#endif
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
        public void CatchOneTimeTearDownExceptions(Action action)
        {
            Contract.Requires(action != null);

            try { action(); }
            catch (Exception e) { SetUnhandledException(e); }
        }

        /// <summary>Catches any exception fired by a tear down action.</summary>
        /// <param name="action">The action.</param>
        /// <remarks>The exception is re-thrown when all tear down actions are finished</remarks>
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
                    unhandledAsyncException = unhandledAsyncException ?? exception;
                UnhandledExceptionEvent?.Invoke(this, new ExceptionEventArgs(exception));
            }
        }

        [Log]
        private void AppDomainUnhandledExceptionHandler(object _, UnhandledExceptionEventArgs args) => SetUnhandledException((Exception) args.ExceptionObject);

        [Log]
        private void TaskSchedulerUnobservedTaskExceptionHandler(object _, UnobservedTaskExceptionEventArgs args)
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

        ///// <summary>Unhandled exception event.</summary>
        protected event EventHandler<ExceptionEventArgs> UnhandledExceptionEvent;

        /// <summary>The default timeout for interactive tests.</summary>
        /// <remarks>Returns int.MaxValue if a debugger is attached, otherwise 60 seconds.</remarks>
        public static int DefaultInteractiveTestTimeout { get; } = 60000;

        /// <summary>The test timeout in milliseconds.</summary>
        /// <remarks>Returns int.MaxValue if a debugger is attached, otherwise 10 seconds.</remarks>
        public const int Timeout = 10000;

        private const string contractExceptionName = "System.Diagnostics.Contracts.__ContractsRuntime+ContractException";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly object locker = new object();
        private ConcurrentStack<TestAdapter> oneTimeAdapters;
        private ConcurrentStack<TestAdapter> testAdapters;
        private Exception unhandledAsyncException;
    }
}
