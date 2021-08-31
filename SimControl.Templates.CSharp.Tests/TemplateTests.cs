// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading.Channels;
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
        public static void ClassLibrary_Class1__invoke_constructor__succeeds() =>
            Assert.That(new ClassLibrary.Class1().ToString(), Is.Not.Null);

        [Test]
        public static void ClassLibraryOld_Class1__invoke_constructor__succeeds() =>
            Assert.That(new ClassLibraryOld.Class1().ToString(), Is.Not.Null);

#if !NET5_0 // TODO ConsoleApp tests for net5.0

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void ConsoleApp__start_process__exits_with_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var process = new ProcessTestAdapter(ProcessName, "", out ChannelReader<string> standardOutput,
                out _);

            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("MainAssembly"))
                .AssertTimeoutAsync().Wait();

            if (process.Process != null) process.Process.StandardInput.Close();

            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("Exit"))
                .AssertTimeoutAsync().Wait();

            Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
        }

#endif
        // TODO SimControl.Templates.CSharp.WcfServiceLibrary tests

        public const string ProcessName = "SimControl.Templates.CSharp.ConsoleApp";
    }
}
