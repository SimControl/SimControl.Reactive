// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Diagnostics;
using System.Threading.Channels;
using NCrunch.Framework;
using NLog;
using NUnit.Framework;
using SimControl.Log;

// UNDONE ProcessTestAdapterTests

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture, ExclusivelyUses(ProcessName)]
    public class ProcessTestAdapterTests: TestFrame
    {
        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void ConsoleApp__start_process_by_process_name__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            ChannelReader<string> standardOutput;

            using (var process = new ProcessTestAdapter(ProcessName, null, out standardOutput, out _))
            {
                _ = standardOutput.TakeUntilAssertTimeout(s => s.Contains("MainAssembly"), DebugTimeout(50000));
                process.Process.StandardInput.Close();
                _ = standardOutput.TakeUntilAssertTimeout(s => s.Contains("Exit"), DebugTimeout(50000));
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void ConsoleApp__start_process_with_path__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using (var process = new ProcessTestAdapter(TestContext.CurrentContext.TestDirectory, ProcessName,
                null, out _, out _))
            {
                process.Process.StandardInput.Close();
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void ConsoleApp_start_process__ToString__is_correct()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using (var process = new ProcessTestAdapter(ProcessName, null, out _, out _))
            {
                logger.Info("ProcessRunning", LogMethod.GetCurrentMethodName(), process.ToString());

                process.Process.StandardInput.Close();
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));

                logger.Info("ProcessExited", LogMethod.GetCurrentMethodName(), process.ToString());
            }
        }

        [Test, IntegrationTest]
        public static void Start_ConsoleApp_process__Kill__ConsoleApp_process_killed()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);
            Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(0));

            using (var process = new ProcessTestAdapter(ProcessName, null, out _, out _))
            {
                Assert.That(Process.GetProcessesByName(ProcessName).Length > 0);

                process.Kill();
                Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(0));
            }
        }

        [Test, IntegrationTest]
        public static void Start_ConsoleApp_process__KillProcess__ConsoleApp_process_killed()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);
            Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(0));

            using (var process = new ProcessTestAdapter(ProcessName, null, out _, out _))
            {
                Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(1));

                ProcessTestAdapter.KillProcesses(ProcessName);
                Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(0));

                _ = process.WaitForExitAssertTimeout();
            }
        }

        // UNDONE CloseMainWindowAssertTimeout tests

        public const string ProcessName = "SimControl.Samples.CSharp.ConsoleApp";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
