// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Threading.Channels;
using NLog;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for starting a console process.</summary>
    /// <seealso cref="TestAdapter"/>
    public class ProcessTestAdapter: TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="ProcessTestAdapter"/> class.</summary>
        /// <param name="name">Process name.</param>
        /// <param name="arguments">Process arguments.</param>
        /// <param name="standardOutput">[out] The standard output.</param>
        /// <param name="standardError">[out] The standard error.</param>
        [Log]
        public ProcessTestAdapter(string name, string arguments,
            out ChannelReader<string> standardOutput, out ChannelReader<string> standardError)
        {
            Contract.Requires(string.IsNullOrEmpty(name));

            standardOutput = output.Reader;
            standardError = error.Reader;

            StartProcess(TestContext.CurrentContext.TestDirectory + "\\" + name + ".exe", arguments,
                output.Writer, error.Writer);
        }

        /// <summary>Initializes a new instance of the <see cref="ProcessTestAdapter"/> class.</summary>
        /// <param name="path">Absolute path to the process file.</param>
        /// <param name="name">Process name.</param>
        /// <param name="arguments">Process arguments.</param>
        /// <param name="standardOutput">[out] The standard output.</param>
        /// <param name="standardError">[out] The standard error.</param>
        public ProcessTestAdapter(string path, string name, string arguments,
            out ChannelReader<string> standardOutput, out ChannelReader<string> standardError)

        {
            Contract.Requires(string.IsNullOrEmpty(path));
            Contract.Requires(string.IsNullOrEmpty(name));

            standardOutput = output.Reader;
            standardError = error.Reader;

            StartProcess(path + "\\" + name + ".exe", arguments, output.Writer, error.Writer);
        }

        /// <summary>Kills all processes with the same <paramref name="processName"/>.</summary>
        /// <param name="processName">Name of the process.</param>
        public static void KillProcesses(string processName)
        {
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                process.Kill();

                Assert.That(process.WaitForExit(TestFrame.Timeout),
                    "Process " + processName + " could not be killed");
            }
        }

        /// <summary>Closes the main window while asserting the default test timeout.</summary>
        /// <returns><see cref="Process.ExitCode"/></returns>
        public int CloseMainWindowAssertTimeout() => CloseMainWindowAssertTimeout(TestFrame.Timeout);

        /// <summary>Closes the main window while asserting the specified timeout.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns><see cref="Process.ExitCode"/></returns>
        public int CloseMainWindowAssertTimeout(int timeout)
        {
            Assert.That(Process.CloseMainWindow());

            return WaitForExitAssertTimeout(timeout);
        }

        /// <summary>Kills the process instance.</summary>
        [Log]
        public void Kill()
        {
            Contract.Requires(Process != null);

            Process.Kill();
            Assert.That(Process.WaitForExit(TestFrame.Timeout), "Timeout expired");
            Process = null;
        }

        /// <inheritdoc/>
        public override string ToString() => LogFormat.FormatObject(typeof(ProcessTestAdapter),
            Process != null ? Process.Id : -1, Process == null || Process.HasExited);

        /// <summary>Waits for a process to exit while asserting the <see cref="TestFrame.Timeout"/>.</summary>
        /// <returns>the process exit code.</returns>
        public int WaitForExitAssertTimeout() => WaitForExitAssertTimeout(TestFrame.Timeout);

        /// <summary>Waits for a process to exit while asserting the timeout.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns><see cref="Process.ExitCode"/></returns>
        [Log]
        public int WaitForExitAssertTimeout(int timeout)
        {
            Contract.Requires(Process != null);

            if (!Process.WaitForExit(timeout))
            {
                try
                {
                    Process.Kill();
                    Process.WaitForExit(TestFrame.DebugTimeout(timeout));
                }
                catch (Exception e)
                {
                    logger.Warn(e, LogMethod.GetCurrentMethodName()); ;
                }
                Process.Dispose();
                Process = null;

                Assert.Fail("Timeout expired");
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

        private void StartProcess(string fileName, string arguments, ChannelWriter<string> standardOutput,
            ChannelWriter<string> standardError)
        {
            Process = Process.Start(new ProcessStartInfo {
                Arguments = arguments, CreateNoWindow = true, FileName = fileName, RedirectStandardInput = true,
                RedirectStandardError = true, RedirectStandardOutput = true, UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(fileName)
            });

            Process.OutputDataReceived += (sender, args) => standardOutput.TryWrite(args.Data);
            Process.ErrorDataReceived += (sender, args) => standardError.TryWrite(args.Data);

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        /// <summary>Gets the process.</summary>
        /// <value>The process.</value>
        public Process Process { get; private set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Channel<string> error = Channel.CreateUnbounded<string>(
            new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });

        private Channel<string> output = Channel.CreateUnbounded<string>(
            new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });
    }
}
