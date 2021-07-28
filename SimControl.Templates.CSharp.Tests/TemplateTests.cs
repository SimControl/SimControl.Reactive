// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

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
        public static async Task ConsoleApp__start_process__returns_0__Async()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using (var process = new ProcessTestAdapter(ProcessName, null,
                out ChannelReader<string> standardOutput, out _))
            {
                await standardOutput.TakeUntilAssertTimeoutAsync(s => s.Contains("MainAssembly"))
                    .AssertTimeout().ConfigureAwait(false);

                process.Process.StandardInput.Close();

                await standardOutput.TakeUntilAssertTimeoutAsync(s => s.Contains("Exit"))
                    .AssertTimeout().ConfigureAwait(false);

                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        // TODO SimControl.Templates.CSharp.WcfServiceLibrary tests

        public const string ProcessName = "SimControl.Templates.CSharp.ConsoleApp";
    }
}
