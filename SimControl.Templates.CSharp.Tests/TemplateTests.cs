// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NCrunch.Framework;
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

#if !NETCOREAPP3_1 //TODO copy MSBuild.deps.json and MSBuild.runtimeconfig.json
        [Test, IntegrationTest, ExclusivelyUses("SimControl.Templates.CSharp.ConsoleApp.exe")]
        public static void ConsoleApp__StartProcess__Returns_0()
        {
            ConsoleProcessTestAdapter.KillProcesses(typeof(Program).FullName);

            using (var process = new ConsoleProcessTestAdapter(typeof(Program).Namespace, null, out _, out _))
            {
                process.Process.StandardInput.Close();
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }
#endif

        [Test]
        public static void ConsoleApp_Program__Invoke_Main__Returns_0()
        {
            Console.In.Close();
            Assert.That(Program.Main(new[] { typeof(Program).FullName + ".exe" }), Is.Zero);
        }

        //TODO SimControl.Templates.CSharp.WcfServiceLibrary tests
    }
}
