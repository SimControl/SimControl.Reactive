// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NCrunch.Framework;
using NLog;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture]
    public class TestFrameTests: TestFrame
    {
        #region Test SetUpTearDown

        [OneTimeSetUp]
        public void OneTimeSetUp_OneTimeRegisterTestAdapter__succeeds() =>
            oneTimeSemaphoreSlim = OneTimeRegisterTestAdapter(
                new DisposableTestAdapter<SemaphoreSlim>(new SemaphoreSlim(0))).Disposable;

        [OneTimeTearDown]
        public void OneTimeTearDown_CatchOneTimeTearDownExceptions__thrown_exception__is_added_to_PendingExceptions()
        {
            CatchOneTimeTearDownExceptions(() => throw new InvalidOperationException());
            Assert.That(TakePendingExceptionAsync().AssertTimeoutAsync().Result,
                Is.InstanceOf(typeof(InvalidOperationException)));
        }

        [SetUp]
        public void SetUp_RegisterTestAdapter__succeeds() => semaphoreSlim = RegisterTestAdapter(
            new DisposableTestAdapter<SemaphoreSlim>(new SemaphoreSlim(0))).Disposable;

        [TearDown]
        public void TearDown_CatchTearDownExceptions__thrown_Exception__is_added_to_PendingExceptions()
        {
            CatchTearDownExceptions(() => throw new InvalidOperationException());
            Assert.That(TakePendingExceptionAsync().AssertTimeoutAsync().Result,
                Is.InstanceOf(typeof(InvalidOperationException)));
        }

        #endregion

        [Test]
        public static void ContextSwitch__Succeeds() => ForceContextSwitch();

        [Test]
        public static void CurrentThreadCulture_IsSetTo_InternationalCultureInfo__Test()
        {
            try { throw new InvalidOperationException(); }
            catch (InvalidOperationException e)
            { Assert.That(e.Message, Is.EqualTo("Operation is not valid due to the current state of the object.")); }
        }

        [Test]
        public static void DebugTimeout__Succeeds() => Task.Delay(DebugTimeout(1)).Wait();

        [Test]
        public static void ForceGarbageCollection__Succeeds() => ForceGarbageCollection();

        [Test, Isolated]
        public void AddUnhandledException__exception_is_received_by_TakePendingException()
        {
            Task.Run(() => {
                try
                {
                    throw new InvalidOperationException(
                  nameof(AddUnhandledException__exception_is_received_by_TakePendingException));
                }
                catch (InvalidOperationException e) { AddPendingException(e); }
            }).AssertTimeoutAsync().Wait();

            Exception e = TakePendingExceptionAsync().AssertTimeoutAsync().Result;

            Assert.That(e, Is.Not.Null);
            Assert.That(e, Is.InstanceOf(typeof(InvalidOperationException)));
            Assert.That(e.Message, Is.EqualTo(
                nameof(AddUnhandledException__exception_is_received_by_TakePendingException)));
        }

        [Test, Isolated]
        public void AppDomain_UnhandledException__thrown_Exception__is_added_to_PendingExceptions()
        {
            if (Environment.GetEnvironmentVariable("NCrunch") != "1")
            // AppDomain.UnhandledException is handled by NCrunch as an error
            {
                var thread = new Thread(() => throw new InvalidOperationException());
                thread.Start();

                thread.JoinAssertTimeout();

                Exception result = TakePendingExceptionAsync().AssertTimeoutAsync().Result;
                Assert.That(result,
                    Is.InstanceOf(typeof(InvalidOperationException)));
            }
        }

        [Test]
        public void SetUp_TearDown__succeed()
        {
            oneTimeSemaphoreSlim.Release();
            semaphoreSlim.Release();
        }

        [Test]
        public void TakePendingExceptionAsync__thrown_Exception__is_added_to_PendingExceptions()
        {
            Task.Run(() => AddPendingException(new InvalidOperationException())).AssertTimeoutAsync().Wait();

            Assert.That(TakePendingExceptionAsync().AssertTimeoutAsync().Result,
                Is.InstanceOf(typeof(InvalidOperationException)));
        }

        [Test, Isolated]
        public void UnobservedTaskException__ContinueWith_OnlyOnFaulted__does_not_raise_UnobservedTaskException()
        {
            ThrowUnhandledExceptionInAsyncTaskContinueWithOnlyOnFaultedLogException();

            Thread.Sleep(1000);
            ForceGarbageCollection();

            Assert.That<Exception>(TryTakePendingException(), Is.Null);
        }

        [Test, Isolated]
        public void UnobservedTaskException__thrown_Exception__is_added_to_PendingExceptions()
        {
            ThrowUnhandledExceptionInAsyncTask();

            Thread.Sleep(1000);
            ForceGarbageCollection();

            Exception e = TakePendingExceptionAsync().AssertTimeoutAsync().Result;

            Assert.That(e, Is.Not.Null);
            Assert.That(e, Is.InstanceOf(typeof(AggregateException)));
            Assert.That(e.InnerException, Is.InstanceOf(typeof(InvalidOperationException)));
            Assert.That(e.InnerException.Message, Is.EqualTo(nameof(ThrowUnhandledExceptionInAsyncTask)));
        }

        [Test, Isolated]
        public void UnobservedTaskException__thrown_Exception__is_added_to_TryTakePendingException()
        {
            ThrowUnhandledExceptionInAsyncTask();

            Thread.Sleep(1000);
            ForceGarbageCollection();

            Exception? e = TryTakePendingException();

            Assert.That(e, Is.Not.Null);
            Assert.That(e, Is.InstanceOf(typeof(AggregateException)));
            Assert.That(e.InnerException, Is.InstanceOf(typeof(InvalidOperationException)));
            Assert.That(e.InnerException.Message, Is.EqualTo(nameof(ThrowUnhandledExceptionInAsyncTask)));
        }

        private static void ThrowUnhandledExceptionInAsyncTask() =>
            Task.Run(() => throw new InvalidOperationException(nameof(ThrowUnhandledExceptionInAsyncTask)));

        private static void ThrowUnhandledExceptionInAsyncTaskContinueWithOnlyOnFaultedLogException() =>
            Task.Run(() => throw new InvalidOperationException(
                nameof(ThrowUnhandledExceptionInAsyncTaskContinueWithOnlyOnFaultedLogException)))
                .ContinueWith(t => logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(),
                    null, t.Exception.InnerException), TaskContinuationOptions.OnlyOnFaulted);

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private SemaphoreSlim oneTimeSemaphoreSlim;
        private SemaphoreSlim semaphoreSlim;
    }
}

// add AssertTimeout aborted with timeout does not signal exception
