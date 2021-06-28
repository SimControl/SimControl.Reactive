// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO CR

/*
using System.Collections.Concurrent;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.LogEx;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ConsoleApplication.Tests
{
    [Log]
    [TestFixture, ExclusivelyUses("Logging")]
    public class SampleConsoleApplicationTests : TestFrame
    {
        #region Additional test attributes

        [SetUp]
        public new void SetUp() =>
            filePath = TestContext.CurrentContext.TestDirectory + "\\SimControl.Samples.CSharp.ConsoleApplication.exe";

        #endregion

        [Test]
        public static void ConsoleApplicationMain_Normal() => Assert.AreEqual(0, Program.Main("Normal"));

        [Test, IntegrationTest]
        public void ConsoleApplication_Normal()
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "Normal",
                out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
                Assert.AreEqual(0, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ThrowException()
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ThrowException",
                out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
                Assert.AreEqual(7, processAdapter.WaitForExitAssertTimeout());
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ThrowExceptionOnThread()
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ThrowExceptionOnThread",
                out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
                Assert.AreEqual(6, processAdapter.WaitForExitAssertTimeout());
        }

        //[Test, IntegrationTest]
        //public void ConsoleApplication_ValidateSettings()
        //{
        //    using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "ValidateSettings",
        //        out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
        //        Assert.AreEqual(0, processAdapter.WaitForExitAssertTimeout());
        //}

        [Test, IntegrationTest]
        public void ConsoleApplication_Wait_StandardInputClosed() //TODO fails with NUnit TestAdapter
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "Wait",
                out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
            {
                //standardOutput.TakeUntilAssertTimeout(s => s.Contains("Waiting"));

                processAdapter.Process.StandardInput.Close();

                Assert.AreEqual(5, processAdapter.WaitForExitAssertTimeout());
            }
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_Wait_StandardInputRead1Line() //TODO fails with NUnit TestAdapter
        {
            using (var processAdapter = new ConsoleProcessTestAdapter(filePath, "Wait",
                out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
            {
                //standardOutput.TakeUntilAssertTimeout(s => s.Contains("Waiting"));

                processAdapter.Process.StandardInput.WriteLine("abc");

                Assert.AreEqual(0, processAdapter.WaitForExitAssertTimeout());
            }
        }

        private string filePath;
    }
}
*/
