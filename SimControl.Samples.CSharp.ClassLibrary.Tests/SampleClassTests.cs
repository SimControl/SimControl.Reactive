// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Globalization;
using System.Reflection;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Samples.CSharp.ClassLibrary;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class SampleClassTests: TestFrame
    {
        #region Additional test attributes

        [SetUp]
        new public void SetUp() => SetPrivateStaticField(typeof(SampleClass), "counter", 0);

        #endregion

        [Test]
        public void SampleClassTests_SampleClass_DoSomething_WriteLogMessagesToLogTargets() => sampleClass.DoSomething();

        [Test]
        public void SampleClassTests_SampleClass_LogSettingsTest_WriteAppAndUserSettingsToLogTargets() => sampleClass.LogSettings();

        [Test]
        public void SampleClassTests_SampleClass_StaticCounter_AssertIs0AfterTestInitialize1()
        {
            Assert.AreEqual(0, SampleClass.StaticCounter);
            SampleClass.IncrementStaticCounter();
        }

        [Test]
        public void SampleClassTests_SampleClass_StaticCounter_AssertIs0AfterTestInitialize2()
        {
            Assert.AreEqual(0, SampleClass.StaticCounter);
            SampleClass.IncrementStaticCounter();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Test]
        public void SampleClassTests_SampleClass_ValidateCodeContractFalse_ThrowsContractException()
        {
            try
            {
                sampleClass.ValidateCodeContract(false);
            }
            catch (Exception e)
            {
                AssertIsContractException(e);
            }
        }

        [Test]
        public void SampleClassTests_SampleClass_ValidateCodeContractTrue_NoException() => sampleClass.ValidateCodeContract(true);

        [Test]
        public void SampleClassTests_SampleClass_ValidateSettings_NoException() => sampleClass.ValidateSettings();

        [Test, Sequential]
        public void SampleClassTests_SequentialValues([Values(0, 1, 2, 3)] int arg, [Values(0, 1, 4, 9)] int res) => Assert.That(arg*arg, Is.EqualTo(res));

        [Test]
        public void SampleClassTests_VerifyJitOptimization_Run() => VerifyJitOptimization.Run();

        private readonly SampleClass sampleClass = new SampleClass();
    }
}
