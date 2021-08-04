// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;

// UNDONE TestFrameTests CR

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture]
    public class TestFrameTests: TestFrame
    {
        #region Test SetUpTearDown

        [OneTimeSetUp]
        public new void OneTimeSetUp() => oneTimeAutoResetEvent = OneTimeRegisterTestAdapter(
            new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

        [OneTimeTearDown]
        public new void OneTimeTearDown()
        {
            CatchOneTimeTearDownExceptions(() => throw new InvalidOperationException());
            Assert.That(TakePendingException(), Is.InstanceOf(typeof(InvalidOperationException)));
        }

        [SetUp]
        public new void SetUp() => autoResetEvent = RegisterTestAdapter(
            new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

        [TearDown]
        public new void TearDown()
        {
            CatchTearDownExceptions(() => throw new InvalidOperationException());
            Assert.That(TakePendingException(), Is.InstanceOf(typeof(InvalidOperationException)));
        }

        #endregion

        [Test]
        public static void ContextSwitch__Succeeds() => ContextSwitch();

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
        public static void Delay__Succeeds() => Task.Delay(1).Wait();

        [Test]
        public static void ForceGarbageCollection__Succeeds() => ForceGarbageCollection();

        [Test]
        public static void Run__Succeeds() => Task.Run(ContextSwitch).AssertTimeoutAsync().Wait();

        [Test, Isolated]
        public void AppDomainUnhandledExceptionHandler__Test()
        {
            if (Environment.GetEnvironmentVariable("NCrunch") != "1") // UNDONE UnhandledException not raised with NCrunch
            {
                var thread = new Thread(() => throw new InvalidOperationException());
                thread.Start();

                thread.JoinAssertTimeout();

                Assert.That(TakePendingException(), Is.InstanceOf(typeof(InvalidOperationException)));
            }
        }

        [Test]
        public void SetUp_TearDown__Test()
        {
            oneTimeAutoResetEvent.Set();
            autoResetEvent.Set();
        }

        [Test]
        public void TakePendingExceptionAssertTimeout__Succeds()
        {
            Task.Run(() => AddUnhandledException(new InvalidOperationException())).AssertTimeoutAsync().Wait();

            Assert.That(TakePendingException(), Is.InstanceOf(typeof(InvalidOperationException)));
        }

        [Test, Isolated, Ignore("UnobservedTaskException not raised")] // UNDONE UnobservedTaskException not raised
        public void TaskSchedulerUnobservedTaskExceptionHandler__Test()
        {
            ThrowUnhandledExceptionInAsyncTask();

            ContextSwitch();
            ForceGarbageCollection();

            Exception e = TakePendingException();

            Assert.That(e, Is.Not.Null);
            Assert.That(e, Is.InstanceOf(typeof(InvalidOperationException)));
        }

        private static void ThrowUnhandledExceptionInAsyncTask() =>
            Task.Run(() => throw new InvalidOperationException());

        private AutoResetEvent autoResetEvent;
        private AutoResetEvent oneTimeAutoResetEvent;
    }
}
