// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Tests
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
        public new void OneTimeSetUp() =>
            autoResetEvent1 = OneTimeRegisterTestAdapter(new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

        [SetUp]
        public new void SetUp() =>
           autoResetEvent2 = RegisterTestAdapter(new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

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
        public static Task Run__Succeeds() => Run(() => ContextSwitch());

        [Test]
        public static void InvokePrivateStaticMethod__()
        {
        }

        [Test]
        public static void SetPrivateStaticField__()
        {
        }

        [Test]
        public void Test()
        {
            _ = autoResetEvent1.Set();
            _ = autoResetEvent2.Set();
        }

        private AutoResetEvent autoResetEvent1;
        private AutoResetEvent autoResetEvent2;
    }
}
