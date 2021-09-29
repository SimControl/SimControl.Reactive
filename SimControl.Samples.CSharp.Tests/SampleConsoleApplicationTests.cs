// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading.Channels;
using NCrunch.Framework;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Samples.CSharp.ClassLibrary;
using SimControl.Samples.CSharp.ConsoleApp;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ConsoleApplication.Tests
{
    [Log]
    [Log, TestFixture, ExclusivelyUses(ProcessName)]
    public class SampleConsoleApplicationTests: TestFrame
    {
        [Test]
        public static void ConsoleApplication__Main_Normal__Succeeds() =>
            Assert.That(Program.Main("Normal").Result, Is.EqualTo((int) ExitCode.Success));

#if !NET5_0_OR_GREATER // TODO ConsoleApp tests for net5.0

        [Test, IntegrationTest]
        public void ConsoleApplication_Normal()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "Normal", out _, out _);
            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ProcessRunning", processTestAdapter);

            Assert.That(processTestAdapter.WaitForExitAssertTimeout(), Is.EqualTo((int) ExitCode.Success));
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ThrowException()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "ThrowException", out _, out _);
            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ProcessRunning", processTestAdapter);

            Assert.That(processTestAdapter.WaitForExitAssertTimeout(), Is.EqualTo((int) ExitCode.UnhandledException));
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_ThrowExceptionOnThread()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "ThrowExceptionOnThread", out _, out _);
            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ProcessRunning", processTestAdapter);

            Assert.That(processTestAdapter.WaitForExitAssertTimeout(),
                Is.EqualTo((int) ExitCode.ThrowExceptionOnThread));
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_Wait_StandardInputClosed()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "Wait",
                out ChannelReader<string> standardOutput, out _);
            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("MainAssembly"), DebugTimeout(5000)).Wait();
            processTestAdapter.Process.StandardInput.Close();
            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("Exit"), DebugTimeout(5000)).Wait();
            Assert.That(processTestAdapter.WaitForExitAssertTimeout(), Is.EqualTo((int) ExitCode.ConsoleInputClosed));
        }

        [Test, IntegrationTest]
        public void ConsoleApplication_Wait_StandardInputRead1Line()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using var processTestAdapter = new ProcessTestAdapter(ProcessName, "Wait",
                out ChannelReader<string> standardOutput, out _);
            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("MainAssembly"), DebugTimeout(5000)).Wait();

            processTestAdapter.Process.StandardInput.WriteLine("abc");
            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("abc"), DebugTimeout(5000)).Wait();

            processTestAdapter.Process.StandardInput.Close();

            standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("Exit"), DebugTimeout(5000)).Wait();
            Assert.That(processTestAdapter.WaitForExitAssertTimeout(), Is.EqualTo((int) ExitCode.ConsoleInputClosed));
        }

#endif
        public const string ProcessName = "SimControl.Samples.CSharp.ConsoleApp";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
