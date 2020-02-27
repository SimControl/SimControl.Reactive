// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;
using SimControl.Templates.CSharp.ConsoleApp;
using SimControl.TestUtils;

namespace SimControl.Templates.CSharp.Tests
{
    [Log, TestFixture]
    public class TemplateTests: TestFrame
    {
        [Test]
        public static void ClassLibrary_Class1__constructor__succeeds() =>
            Assert.That(new ClassLibrary.Class1().ToString(), Is.Not.Null);

        [Test, IntegrationTest]
        public static void ConsoleApp__Process__Returns_0()
        {
            ConsoleProcessTestAdapter.KillProcesses(typeof(Program).FullName);

            using (var process = new ConsoleProcessTestAdapter(typeof(Program).Namespace, null, out _, out _))
            {
                process.Process.StandardInput.Close();
                Assert.AreEqual(0, process.WaitForExitAssertTimeout());
            }
        }

        [Test]
        public static void ConsoleApp_Program__Main__succeeds() => Assert.That(Program.Main(new[] {
            typeof(Program).FullName + ".exe" }), Is.Zero);

        //TODO SimControl.Templates.CSharp.WcfServiceLibrary tests
    }
}
