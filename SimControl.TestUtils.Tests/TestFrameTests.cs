// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;

// TODO CR

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

        private static void ResetField() => field = 0;

        private static void SetField(int p) => field = p;

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
            Assert.That(TakePendingException(), Is.InstanceOf(typeof(ApplicationException)));
        }

        [SetUp]
        public new void SetUp() => autoResetEvent = RegisterTestAdapter(
            new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

        [TearDown]
        public new void TearDown()
        {
            CatchTearDownExceptions(() => throw new ApplicationException());
            Assert.That(TakePendingException(), Is.InstanceOf(typeof(ApplicationException)));
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
        public static Task DebugTimeout__Succeeds() => Task.Delay(DebugTimeout(1));

        [Test]
        public static Task Delay__Succeeds() => Task.Delay(1);

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
        public static Task Run__Succeeds() => Task.Run(() => ContextSwitch());

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
            if (Environment.GetEnvironmentVariable("NCrunch") != "1") // UNDONE UnhandledException not raised with NCrunch
            {
                var thread = new Thread(() => throw new ApplicationException());
                thread.Start();

                thread.JoinAssertTimeout();

                Assert.That(TakePendingException(), Is.InstanceOf(typeof(ApplicationException)));
            }
        }

        [Test, Ignore("Code Contracts")]
        public void AssertIsContractException__Succeeds() // TODO Code Contracts
        {
            Exception contractException = null;

            try { ThrowContractException(true); }
            catch (Exception e) { contractException = e; }

            Assert.That(contractException, Is.Not.Null);
            Assert.That(IsContractException(contractException));
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
            _ = Task.Run(() => AddUnhandledException(new ApplicationException()));

            Assert.That(TakePendingException(), Is.InstanceOf(typeof(ApplicationException)));
        }

        [Test, Isolated, Ignore("UnobservedTaskException not raised")] // UNDONE UnobservedTaskException not raised
        public void TaskSchedulerUnobservedTaskExceptionHandler__Test()
        {
            ThrowUnhandledExceptionInAsyncTask();

            ContextSwitch();
            ForceGarbageCollection();

            Exception e = TakePendingException();

            Assert.That(e, Is.Not.Null);
            Assert.That(e, Is.InstanceOf(typeof(ApplicationException)));
        }

        private void ThrowContractException(bool throwIfTrue) => Contract.Requires(!throwIfTrue);

        private void ThrowUnhandledExceptionInAsyncTask() => Task.Run(() => throw new ApplicationException());

        private AutoResetEvent autoResetEvent;
        private AutoResetEvent oneTimeAutoResetEvent;
    }
}
