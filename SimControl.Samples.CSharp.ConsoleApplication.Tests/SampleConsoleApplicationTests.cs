// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using NCrunch.Framework;
using NLog;
using NLog.Config;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ConsoleApplication.Tests
{
    [Log]
    [TestFixture, ExclusivelyUses("Logging")]
    public class SampleConsoleApplicationTests : TestFrame
    {
        #region Additional test attributes

        [SetUp]
        new public void SetUp() => filePath = TestContext.CurrentContext.TestDirectory + "\\SimControl.Samples.CSharp.ConsoleApplication.exe";

        #endregion

        [Test, IntegrationTest]
        public void ConsoleApplication_Normal()
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "Normal", null, out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
                Assert.AreEqual(0, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ThrowException()
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ThrowException", null, out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
                Assert.AreEqual(7, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ThrowExceptionOnThread()
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ThrowExceptionOnThread", null, out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
                Assert.AreEqual(6, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ValidateSettings()
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ValidateSettings", null, out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
                Assert.AreEqual(0, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_Wait() //TODO fails with NUnit TestAdapter
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "Wait", null, out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
            {
                standardOutput.TakeUntilAssertTimeout(s => s.Contains("SimControl.Samples.CSharp.ConsoleApplication.Program"));

                System.Console.WriteLine("XXX");
                processAdapter.Process.StandardInput.Close();

                Assert.AreEqual(5, processAdapter.WaitForExitAssertTimeout());
            }
        }

        [Test]
        public void ConsoleApplicationMain_Normal() => Assert.AreEqual(0, Program.Main("Normal"));

        private string filePath;
    }
}
