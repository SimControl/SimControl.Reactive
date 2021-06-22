// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading.Channels;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Templates.CSharp.ConsoleApp;
using SimControl.TestUtils;

// TODO CR

namespace SimControl.Templates.CSharp.Tests
{
    [Log, TestFixture]
    public class TemplateTests: TestFrame
    {
        [Test]
        public static void ClassLibrary_Class1__InvokeConstructor__succeeds() =>
            Assert.That(new ClassLibrary.Class1().ToString(), Is.Not.Null);

        [Test]
        public static void ClassLibraryOld_Class1__InvokeConstructor__succeeds() =>
            Assert.That(new ClassLibraryOld.Class1().ToString(), Is.Not.Null);

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void ConsoleApp__start_process__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);


            using (var process = new ProcessTestAdapter(ProcessName, null,
                out ChannelReader<string> standardOutput, out _))
            {
                _ = standardOutput.TakeUntilAssertTimeout(s => s.Contains("MainAssembly"));
                process.Process.StandardInput.Close();
                _ = standardOutput.TakeUntilAssertTimeout(s => s.Contains("Exit"), DebugTimeout(50000));
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        // TODO SimControl.Templates.CSharp.WcfServiceLibrary tests

        public const string ProcessName = "SimControl.Templates.CSharp.ConsoleApp";
    }
}
