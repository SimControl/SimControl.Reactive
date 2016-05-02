// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Management;
using System.Reflection;
using System.Threading;
using NLog;
using SimControl.Log;
using SimControl.Reactive;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for starting a console process.</summary>
    public class ConsoleProcessTestAdapter : TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="ConsoleProcessTestAdapter"/> class.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="standardInput">The standard input.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="standardError">The standard error.</param>
        /// <remarks>
        /// Tries to kill all processes with the same filename and command line arguments before starting a new console application.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "4#")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        [Log]
        public ConsoleProcessTestAdapter(string fileName, string arguments, string standardInput,
            out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError)
        {
            Contract.Requires(fileName != null);

            // kill orphaned processes from previous test runs
            using (var managementObjectSearcher = new ManagementObjectSearcher(
                "select CommandLine, ExecutablePath, ProcessId from Win32_Process where Name='" + Path.GetFileName(fileName) + "'"))
            using (var managementObjectCollection = managementObjectSearcher.Get())
                foreach (var retObject in managementObjectCollection)
                {
                    string commandLine = ((string) retObject["CommandLine"]);

                    logger.Warn(MethodBase.GetCurrentMethod().Name, "Running process found", ((string) retObject["ExecutablePath"]), commandLine);

                    if (arguments == null || commandLine.Substring(commandLine.Length-arguments.Length)== arguments)
                        try
                        {
                            logger.Warn(MethodBase.GetCurrentMethod().Name, "Killing process", ((uint) retObject["ProcessId"]), commandLine);
                            Process.GetProcessById((int) (uint) retObject["ProcessId"]).Kill();
                        }
                        catch (Exception e) { logger.Warn(e, MethodBase.GetCurrentMethod().ToString()); }
                }

            Process = Process.Start(new ProcessStartInfo {
                Arguments = arguments, CreateNoWindow = true, FileName = fileName, RedirectStandardInput = true,
                RedirectStandardError = true, RedirectStandardOutput = true, UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(fileName)
            });

            var stdOutput = standardOutput = new BlockingCollection<string>();
            var stdError = standardError = new BlockingCollection<string>();

            Process.OutputDataReceived += (sender, args) => stdOutput.Add(args.Data);
            Process.ErrorDataReceived += (sender, args) => stdError.Add(args.Data);

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();

            if (standardInput != null)
                Process.StandardInput.Write(standardInput);
        }

        /// <summary>Closes the main window while asserting the specified timeout.</summary>
        /// <param name="timeout">The timeout.</param>
        public void CloseMainWindowAssertTimeout(int timeout)
        {
            Process.CloseMainWindow();
            WaitForExitAssertTimeout(timeout);
        }

        /// <summary>Kills the process instance.</summary>
        [Log]
        public void Kill()
        {
            Contract.Requires(Process != null);

            Process.Kill();
            Process.WaitForExit();
            Process = null;
        }

        /// <inheritdoc/>
        public override string ToString() => LogFormat.FormatObject(typeof(ConsoleProcessTestAdapter), Process == null,
            Process == null || Process.HasExited);

        /// <summary>Waits for a process to exit while asserting the <see cref="TestFrame.DefaultTestTimeout"/>.</summary>
        /// <returns>the process exit code.</returns>
        public int WaitForExitAssertTimeout() => WaitForExitAssertTimeout(TestFrame.DefaultTestTimeout);

        /// <summary>Waits for a process to exit while asserting the timeout.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log]
        public int WaitForExitAssertTimeout(int timeout)
        {
            Contract.Requires(Process != null);

            if (!Process.WaitForExit(timeout))
            {
                try
                {
                    Process.Kill();
                    Process.WaitForExit(TestFrame.DisableDebugTimeout(timeout));
                }
                catch (Exception e)
                {
                    logger.Warn(e, MethodBase.GetCurrentMethod().ToString());
                }
                Process.Dispose();
                Process = null;

                throw new TimeoutException("Test timeout " + timeout.ToString(CultureInfo.InvariantCulture) + " expired");
            }

            int ret = Process.ExitCode;
            Process.Dispose();
            Process = null;
            return ret;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing && Process != null)
                WaitForExitAssertTimeout();
        }

        /// <summary>Gets the process.</summary>
        /// <value>The process.</value>
        public Process Process { get; private set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
