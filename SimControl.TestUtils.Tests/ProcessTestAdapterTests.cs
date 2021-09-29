// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Diagnostics;
using System.Threading.Channels;
using NCrunch.Framework;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Samples.CSharp.ClassLibrary;

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture, ExclusivelyUses(ProcessName)]
    public class ProcessTestAdapterTests: TestFrame
    {
#if !NET5_0_OR_GREATER // TODO ConsoleApp tests for net5.0

        [Test, IntegrationTest]
        public static void Kill__start_ConsoleApp_process__process_is_killed()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);
            Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(0));

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "Wait", out _, out _);
            LongContextSwitch(50);
            Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(1));

            processTestAdapter.Kill();
            LongContextSwitch();
            Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(0));
        }

        [Test, IntegrationTest]
        public static void KillProcesses__start_ConsoleApp_process__process_is_killed()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);
            Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(0));

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "Wait", out _, out _);
            LongContextSwitch(50);
            Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(1));

            ProcessTestAdapter.KillProcesses(ProcessName);
            LongContextSwitch();
            Assert.That(Process.GetProcessesByName(ProcessName).Length, Is.EqualTo(0));

            processTestAdapter.WaitForExitAssertTimeout();
        }

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void Start_ConsoleApp__ToString_is_correct()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "Normal", out _, out _);
            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ProcessRunning", processTestAdapter);

            Assert.That(processTestAdapter.WaitForExitAssertTimeout(), Is.EqualTo((int) ExitCode.Success));

            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ProcessExited", processTestAdapter);
        }

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void Start_ConsoleApp_by_process_name__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "Wait",
                out ChannelReader<string> standardOutput, out _);
            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("MainAssembly"), DebugTimeout(5000)).Wait();
            processTestAdapter.Process.StandardInput.Close();
            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("Exit"), DebugTimeout(5000)).Wait();
            Assert.That(processTestAdapter.WaitForExitAssertTimeout(), Is.EqualTo((int) ExitCode.ConsoleInputClosed));
        }

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void Start_ConsoleApp_with_path__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var processTestAdapter = new ProcessTestAdapter(TestContext.CurrentContext.TestDirectory, ProcessName,
                "Wait", out _, out _);
            processTestAdapter.Process.StandardInput.Close();
            Assert.That(processTestAdapter.WaitForExitAssertTimeout(), Is.EqualTo((int) ExitCode.ConsoleInputClosed));
        }

#endif

        // TODO CloseMainWindowAssertTimeout tests

        public const string ProcessName = "SimControl.Samples.CSharp.ConsoleApp";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
