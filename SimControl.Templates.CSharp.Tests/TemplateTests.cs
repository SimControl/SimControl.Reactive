// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

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

        [Test, IntegrationTest, ExclusivelyUses(ProcessName), Isolated]
        public static async Task ConsoleApp__start_process__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using (var process = new ProcessTestAdapter(ProcessName, null,
                out ChannelReader<string> standardOutput, out _))
            {
                _ = await standardOutput.TakeUntilAssertTimeoutAsync(s => s.Contains("MainAssembly")).AssertTimeout();
                process.Process.StandardInput.Close();
                _ = await standardOutput.TakeUntilAssertTimeoutAsync(s => s.Contains("Exit")).AssertTimeout();
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        // TODO SimControl.Templates.CSharp.WcfServiceLibrary tests

        public const string ProcessName = "SimControl.Templates.CSharp.ConsoleApp";
    }
}
