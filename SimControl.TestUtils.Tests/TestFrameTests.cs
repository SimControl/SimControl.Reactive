// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    public static class TestClass
    {
        static TestClass()
        {
            ResetField();
            SetField(0);
        }

        public static int GetField() => field;
        private static void SetField(int p) => field = p;
        private static void ResetField() => field = 0;

        private static int field;
    }

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
            CatchOneTimeTearDownExceptions(() => throw new ApplicationException());
            Assert.That(TakePendingExceptionAssertTimeout(), Is.InstanceOf(typeof(ApplicationException)));
        }

        [SetUp]
        public new void SetUp() => autoResetEvent = RegisterTestAdapter(
            new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

        [TearDown]
        public new void TearDown()
        {
            CatchTearDownExceptions(() => throw new ApplicationException());
            Assert.That(TakePendingExceptionAssertTimeout(), Is.InstanceOf(typeof(ApplicationException)));
        }

        #endregion

        [Test]
        public static void ContextSwitch__Succeeds() => ContextSwitch();

        [Test]
        public static Task DebugTimeout__Succeeds() => Delay(DebugTimeout(1));

        [Test]
        public static Task Delay__Succeeds() => Delay(1);

        [Test]
        public static void ForceGarbageCollection__Succeeds() => ForceGarbageCollection();

        [Test]
        public static void InvokePrivateStaticMethod__()
        {
            InvokePrivateStaticMethod(typeof(TestClass), "ResetField");
            Assert.That(TestClass.GetField, Is.EqualTo(0));

            InvokePrivateStaticMethod(typeof(TestClass), "SetField", 1);
            Assert.That(TestClass.GetField, Is.EqualTo(1));

            InvokePrivateStaticMethod(typeof(TestClass), "ResetField");
            Assert.That(TestClass.GetField, Is.EqualTo(0));
        }

        [Test]
        public static Task Run__Succeeds() => Run(() => ContextSwitch());

        [Test]
        public static void SetPrivateStaticField__()
        {
            InvokePrivateStaticMethod(typeof(TestClass), "ResetField");
            Assert.That(TestClass.GetField, Is.EqualTo(0));

            SetPrivateStaticField(typeof(TestClass), "field", 1);
            Assert.That(TestClass.GetField, Is.EqualTo(1));

        }

        [Test, Isolated]
        public void AppDomainUnhandledExceptionHandler__Test()
        {
#if !NETCOREAPP //TODO test runner terminates
            if (Environment.GetEnvironmentVariable("NCrunch") != "1") //TODO UnhandledException not raised with NCrunch
            {
                var thread = new Thread(() => throw new ApplicationException());
                thread.Start();

                thread.JoinAssertTimeout();

                Assert.That(TakePendingExceptionAssertTimeout(), Is.InstanceOf(typeof(ApplicationException)));
            }
#endif
        }

        [Test]
        public void SetUp_TearDown__Test()
        {
            _ = oneTimeAutoResetEvent.Set();
            _ = autoResetEvent.Set();
        }

        [Test]
        public void TakePendingExceptionAssertTimeout__Succeds()
        {
            _ = Run(() => AddUnhandledException(new ApplicationException()));

            Assert.That(TakePendingExceptionAssertTimeout(), Is.InstanceOf(typeof(ApplicationException)));
        }

        [Test, Ignore("Code Contracts")]
        public void AssertIsContractException__Succeeds() //UNDONE Code Contracts
        {
            Exception contractException = null;

            try { ThrowContractException(true); }
            catch (Exception e) { contractException = e; }

            Assert.That(contractException, Is.Not.Null);
            Assert.That(IsContractException(contractException));
        }

        [Test, Isolated, Ignore("UnobservedTaskException not raised")] //TODO UnobservedTaskException not raised
        public void TaskSchedulerUnobservedTaskExceptionHandler__Test()
        {
            ThrowUnhandledExceptionInAsyncTask();

            ContextSwitch();
            ForceGarbageCollection();

            Exception e = TakePendingExceptionAssertTimeout();

            Assert.That(e, Is.Not.Null);
            Assert.That(e, Is.InstanceOf(typeof(ApplicationException)));
        }

        private void ThrowContractException(bool throwIfTrue) => Contract.Requires(!throwIfTrue);
        private void ThrowUnhandledExceptionInAsyncTask() => Run(() => throw new ApplicationException());

        private AutoResetEvent autoResetEvent;
        private AutoResetEvent oneTimeAutoResetEvent;
    }
}
