// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture]
    public class TestFrameTests: TestFrame
    {
        #region Test SetUpTearDown

        [OneTimeTearDown] // should be invoked regardless of exception during SetUp()
        public static new void OneTimeTearDown() { }

        [TearDown]
        public static new void TearDown() { }

        [OneTimeSetUp]
        public new void OneTimeSetUp() => autoResetEvent1 = OneTimeRegisterTestAdapter(
            new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

        [SetUp]
        public new void SetUp() => autoResetEvent2 = RegisterTestAdapter(
            new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

        #endregion

        [Test]
        public static void ContextSwitch__Succeeds() => ContextSwitch();

        [Test]
        public static Task ContextSwitchAsync__Succeeds() => ContextSwitchAsync();

        [Test]
        public static Task DebugTimeout__Succeeds() => Delay(DebugTimeout(1));

        [Test]
        public static Task Delay__Succeeds() => Delay(1);

        [Test]
        public static void InvokePrivateStaticMethod__() //UNDONE
        {
        }

        [Test]
        public static Task Run__Succeeds() => Run(() => ContextSwitch());

        [Test]
        public static void SetPrivateStaticField__() //UNDONE
        {
        }

        [Test]
        public void AppDomainUnhandledExceptionHandler__Test()
        {
#if !NETCOREAPP //TODO
            if (Environment.GetEnvironmentVariable("NCrunch") != "1") //TODO
            {
                var thread = new Thread(() => throw new ApplicationException());
                thread.Start();

                thread.JoinAssertTimeout();
                Exception[] exceptions = ClearPendingExceptions();
                Assert.That(exceptions.Length, Is.EqualTo(1));
                Assert.That(exceptions[0], Is.InstanceOf(typeof(ApplicationException)));
            }
#endif
        }

        [Test]
        public void SetUp_TearDown__Test()
        {
            _ = autoResetEvent1.Set();
            _ = autoResetEvent2.Set();
        }

        [Test]
        public void TaskSchedulerUnobservedTaskExceptionHandler__Test()
        {
            _ = Run(() => throw new ApplicationException());

            Exception e = TakePendingExceptionAssertTimeout();

            Assert.That(e, Is.Not.Null);
            Assert.That(e, Is.InstanceOf(typeof(ApplicationException)));
        }

        private AutoResetEvent autoResetEvent1;
        private AutoResetEvent autoResetEvent2;
    }
}
