﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Diagnostics;
using System.Reflection;
using NCrunch.Framework;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Testutils.Tests
{
#if !NETCOREAPP3_1 //UNDONE copy MSBuild.deps.json and MSBuild.runtimeconfig.json

    [Log, TestFixture, ExclusivelyUses(ProcessName)]
    public class ProcessTestAdapterTests: TestFrame
    {
        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void ConsoleApp__start_process_by_process_name__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using (var process = new ProcessTestAdapter(ProcessName, null, out _, out _))
            {
                process.Process.StandardInput.Close();
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static void ConsoleApp__start_process_by_process_name__succeeds()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using (var process = new ProcessTestAdapter(ProcessName, null, out _, out _))
                process.Process.StandardInput.Close();
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
                logger.Info(MethodBase.GetCurrentMethod().ToString(), process.ToString());

                process.Process.StandardInput.Close();
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));

                logger.Info(MethodBase.GetCurrentMethod().ToString(), process.ToString());
            }
        }

        [Test, IntegrationTest]
        public static void Start_ConsoleApp_process__Kill__ConsoleApp_process_killed()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);
            Assert.That(Process.GetProcessesByName(ProcessName).Length == 0);

            using (var process = new ProcessTestAdapter(ProcessName, null, out _, out _))
            {
                Assert.That(Process.GetProcessesByName(ProcessName).Length > 0);

                process.Kill();
                Assert.That(Process.GetProcessesByName(ProcessName).Length == 0);
            }
        }

        [Test, IntegrationTest]
        public static void Start_ConsoleApp_process__KillProcess__ConsoleApp_process_killed()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);
            Assert.That(Process.GetProcessesByName(ProcessName).Length == 0);

            using (var process = new ProcessTestAdapter(ProcessName, null, out _, out _))
            {
                Assert.That(Process.GetProcessesByName(ProcessName).Length > 0);

                ProcessTestAdapter.KillProcesses(ProcessName);
                Assert.That(Process.GetProcessesByName(ProcessName).Length == 0);

                process.WaitForExitAssertTimeout();
            }
        }

        //TODO CloseMainWindowAssertTimeout tests

        public const string ProcessName = "SimControl.Templates.CSharp.ConsoleApp";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }

#endif
}