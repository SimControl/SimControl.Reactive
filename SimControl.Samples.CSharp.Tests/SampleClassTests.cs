// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;
using SimControl.Samples.CSharp.ConsoleApplication;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class SampleClassTests: TestFrame
    {
        #region Test SetUpTearDown

        [SetUp]
        public static new void SetUp() => SetPrivateStaticField(typeof(SampleClass), "staticCounter", 0);

        #endregion

        [Test]
        public static void SampleClassTests_SampleClass_DoSomething_WriteLogMessagesToLogTargets() =>
            new SampleClass().DoSomething();

        [Test]
        public static void SampleClassTests_SampleClass_StaticCounter_AssertIs0AfterTestInitialize1()
        {
            Assert.AreEqual(0, SampleClass.StaticCounter);
            SampleClass.IncrementStaticCounter();
        }

        [Test]
        public static void SampleClassTests_SampleClass_StaticCounter_AssertIs0AfterTestInitialize2()
        {
            Assert.AreEqual(0, SampleClass.StaticCounter);
            SampleClass.IncrementStaticCounter();
        }

        //[Test]
        //public static void SampleClassTests_SampleClass_ValidateSettings_NoException() => SampleClass.ValidateSettings();

        [Test, Sequential]
        public static void SampleClassTests_SequentialValues([Values(0, 1, 2)] int arg, [Values(0, 1, 4)] int res) =>
            Assert.That(arg*arg, Is.EqualTo(res));

        [Test]
        public static void SampleClassTests_VerifyJitOptimization_Run() => VerifyJitOptimization.Run();
    }
}
