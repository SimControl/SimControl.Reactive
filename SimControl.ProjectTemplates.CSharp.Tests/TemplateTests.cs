// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Collections.Concurrent;
using System.Reflection;
using NLog;
using NLog.Config;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Templates.CSharp.ClassLibrary;
using SimControl.Templates.CSharp.ConsoleApplication;
using SimControl.TestUtils;

namespace SimControl.Templates.CSharp.Tests
{
    [Log]
    [TestFixture]
    public class TemplateTests : TestFrame
    {
        [Test]
        public static void SampleClassTests_SimControlTemplatesCSharpClassLibraryClass1Constructor_Succeeds()
        {
            var class1 = new Class1();
            logger.Message(LogLevel.Trace, MethodBase.GetCurrentMethod(), class1.ToString());
        }

        [Test]
        public static void SampleClassTests_SimControlTemplatesCSharpPortableClassLibraryClass1Constructor_Succeeds()
        {
            var class1 = new PortableClassLibrary.Class1();
            logger.Message(LogLevel.Trace, MethodBase.GetCurrentMethod(), class1.ToString());
        }

        [Test, IntegrationTest]
        public static void SimControlTemplatesCSharpConsoleApplication_RunApplication_Returns0()
        {
            using (var process = new ConsoleProcessTestAdapter(
                TestContext.CurrentContext.TestDirectory + "\\SimControl.Templates.CSharp.ConsoleApplication.exe",
                null, null, out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
            {
                process.Process.StandardInput.Close();
                Assert.AreEqual(0, process.WaitForExitAssertTimeout());
            }
        }

        [Test]
        public static void SimControlTemplatesCSharpConsoleApplicationMain_Returns0() => Assert.AreEqual(0, Program.Main());

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
