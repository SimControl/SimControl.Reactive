// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Templates.CSharp.Tests
{
    [Log, TestFixture]
    public class TemplateTests: TestFrame
    {
#if NETFRAMEWORK
        [Test]
        public void ClassLibraryOld_Class1__constructor__succeeds() =>
            Assert.That(new ClassLibraryOld.Class1().ToString(), Is.Not.Null);

        [Test]
        public void ConsoleAppOld_Program__Main__succeeds() => Assert.That(ConsoleAppOld.Program.Main(new[] {
            "SimControl.Templates.CSharp.ConsoleAppOld.Program" + ".exe" }), Is.Zero);
#endif

#if (NET472 || NETCOREAPP)
        [Test]
        public void ClassLibrary_Class1__constructor__succeeds() =>
            Assert.That(new ClassLibrary.Class1().ToString(), Is.Not.Null);

        [Test]
        public void ConsoleApp_Program__Main__succeeds()
        {
            //Console.OpenStandardInput();
            //Console.In.Close();
            Assert.That(ConsoleApp.Program.Main(new[] { "SimControl.Templates.CSharp.ConsoleApp.Program" + ".exe" }),
                Is.Zero);
        }
#endif

        //[Test, IntegrationTest]
        //public static void SimControlTemplatesCSharpConsoleApplication_RunApplication_Returns0()
        //{
        //    using (var process = new ConsoleProcessTestAdapter(
        //        TestContext.CurrentContext.TestDirectory + "\\SimControl.Templates.CSharp.ConsoleApplication.exe",
        //        null, null, out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError))
        //    {
        //        process.Process.StandardInput.Close();
        //        Assert.AreEqual(0, process.WaitForExitAssertTimeout());
        //    }
        //}
    }
}
