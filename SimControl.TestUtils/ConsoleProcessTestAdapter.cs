// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;

//using System.Management;
using System.Reflection;
using NLog;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for starting a console process.</summary>
    public class ConsoleProcessTestAdapter: TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="ConsoleProcessTestAdapter"/> class.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="standardError">The standard error.</param>
        /// <remarks>
        /// Tries to kill all processes with the same filename and command line arguments before starting a new console
        /// application.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        [Log]
        public ConsoleProcessTestAdapter(string name, string arguments,
            out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError)
        {
            Contract.Requires(string.IsNullOrEmpty(name));

            standardOutput = new BlockingCollection<string>();
            standardError = new BlockingCollection<string>();

            Initialize(TestContext.CurrentContext.TestDirectory + "\\" + name + ".exe", arguments, standardOutput, standardError);
        }

        public ConsoleProcessTestAdapter(string path, string name, string arguments,
            out BlockingCollection<string> standardOutput, out BlockingCollection<string> standardError)

        {
            Contract.Requires(string.IsNullOrEmpty(path));
            Contract.Requires(string.IsNullOrEmpty(name));

            standardOutput = new BlockingCollection<string>();
            standardError = new BlockingCollection<string>();

            Initialize(path + "\\" + name, arguments, standardOutput, standardError);
        }

        public static void KillProcesses(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);

            foreach (Process process in processes)
            {
                try
                {
                    logger.Warn(MethodBase.GetCurrentMethod().Name, "Killing process", (uint) process.Id,
                        process.StartInfo.FileName, process.StartInfo.Arguments);

                    process.Kill();
                }
                catch (Exception e) { logger.Warn(e, MethodBase.GetCurrentMethod().ToString()); }
            }
        }

        /// <summary>Closes the main window while asserting the specified timeout.</summary>
        /// <param name="timeout">The timeout.</param>
        public void CloseMainWindowAssertTimeout(int timeout)
        {
            _ = Process.CloseMainWindow();
            _ = WaitForExitAssertTimeout(timeout);
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

        /// <summary>
        /// Waits for a process to exit while asserting the <see cref="TestFrame.DefaultTestTimeout"/>.
        /// </summary>
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
                    _ = Process.WaitForExit(TestFrame.DisableDebugTimeout(timeout));
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
                _ = WaitForExitAssertTimeout();
        }

        [LogExclude]
        private void Initialize(string fileName, string arguments, BlockingCollection<string> standardOutput,
            BlockingCollection<string> standardError)
        {
            Process = Process.Start(new ProcessStartInfo {
                Arguments = arguments, CreateNoWindow = true, FileName = fileName, RedirectStandardInput = true,
                RedirectStandardError = true, RedirectStandardOutput = true, UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(fileName)
            });

            Process.OutputDataReceived += (sender, args) => standardOutput.Add(args.Data);
            Process.ErrorDataReceived += (sender, args) => standardError.Add(args.Data);

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        /// <summary>Gets the process.</summary>
        /// <value>The process.</value>
        public Process Process { get; private set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
