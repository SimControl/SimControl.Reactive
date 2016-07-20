// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using NCrunch.Framework;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ConsoleApplication.Tests
{
    [Log]
    [TestFixture, ExclusivelyUses("Logging")]
    public class SampleConsoleApplicationTests: TestFrame
    {
        #region Additional test attributes

        [SetUp]
        new public void SetUp()
        {
            filePath = TestContext.CurrentContext.TestDirectory + "\\SimControl.Samples.CSharp.ConsoleApplication.exe";
        }

        #endregion

        private string filePath;

        [Test, IntegrationTest]
        public void ConsoleApplication_Normal()
        {
            BlockingCollection<string> standardOutput;
            BlockingCollection<string> standardError;

            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "Normal", null, out standardOutput, out standardError))
                Assert.AreEqual(0, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ThrowException()
        {
            BlockingCollection<string> standardOutput;
            BlockingCollection<string> standardError;

            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ThrowException", null, out standardOutput, out standardError))
                Assert.AreEqual(7, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ThrowExceptionOnThread()
        {
            BlockingCollection<string> standardOutput;
            BlockingCollection<string> standardError;

            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ThrowExceptionOnThread", null, out standardOutput, out standardError))
                Assert.AreEqual(6, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ValidateSettings()
        {
            BlockingCollection<string> standardOutput;
            BlockingCollection<string> standardError;

            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ValidateSettings", null, out standardOutput, out standardError))
                Assert.AreEqual(0, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_Wait()
        {
            BlockingCollection<string> standardOutput;
            BlockingCollection<string> standardError;

            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "Wait", null, out standardOutput, out standardError))
            {
                standardOutput.TakeUntilAssertTimeout(s => s.Contains("SimControl.Samples.CSharp.ConsoleApplication.Program"));

                System.Console.WriteLine("XXX");
                processAdapter.Process.StandardInput.Close();

                Assert.AreEqual(5, processAdapter.WaitForExitAssertTimeout());
            }
        }

        [Test]
        public void ConsoleApplicationMain_Normal()
        {
            Assert.AreEqual(0, Program.Main("Normal"));
        }
    }
}
