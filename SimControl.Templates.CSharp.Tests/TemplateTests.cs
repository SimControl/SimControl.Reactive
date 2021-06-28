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

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static async Task ConsoleApp__start_process__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            // UNDONE ProcessTestAdapter test case
            using (var process = new ProcessTestAdapter(ProcessName, null,
                out ChannelReader<string> standardOutput, out _))
            {
                var timeoutCancel = new CancellationTokenSource(Timeout);
                while (!(await standardOutput.ReadAsync(timeoutCancel.Token)).Contains("MainAssembly")) ;

                process.Process.StandardInput.Close();

                timeoutCancel = new CancellationTokenSource(Timeout);
                while (!(await standardOutput.ReadAsync(timeoutCancel.Token)).Contains("Exit")) ;

                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static async Task ConsoleApp2__start_process__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using (var process = new ProcessTestAdapter(ProcessName, null,
                out ChannelReader<string> standardOutput, out _))
            {
                _ = standardOutput.TakeUntilAssertTimeout(s => s.Contains("MainAssembly"));
                process.Process.StandardInput.Close();
                _ = standardOutput.TakeUntilAssertTimeout(s => s.Contains("Exit"));
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static async Task ConsoleApp3__start_process__returns_0()
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

        [Test, IntegrationTest, ExclusivelyUses(ProcessName)]
        public static async Task ConsoleApp4__start_process__returns_0()
        {
            ProcessTestAdapter.KillProcesses(ProcessName);

            using (var process = new ProcessTestAdapter(ProcessName, null,
                out ChannelReader<string> standardOutput, out _))
            {
                _ = TTakeUntilAssertTimeout(standardOutput, s => s.Contains("MainAssembly"));
                process.Process.StandardInput.Close();
                _ = TTakeUntilAssertTimeout(standardOutput, s => s.Contains("Exit"));
                Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
            }
        }

        [Log(LogLevel = LogAttributeLevel.Off)]
        public static IEnumerable<T> TTakeUntilAssertTimeout<T>(
            ChannelReader<T> asyncCollection, Func<T, bool> func, int timeout = TestFrame.Timeout)
        {
            //Contract.Requires(asyncCollection != null);
            //Contract.Requires(func != null);

            //System.Collections.Generic.List<T> result = new System.Collections.Generic.List<T>();
            //var result = new System.Collections.Generic.List<T>();
            //var result = new List<T>();

            var timeoutCancel = new CancellationTokenSource(TestFrame.DebugTimeout(timeout));
            for (; ; )
                try
                {
                    T item = asyncCollection.ReadAsync(timeoutCancel.Token).AsTask().Result;

                    //result.Add(item);

                    if (func(item))
                        return null;
                    //return result;
                }
                catch (OperationCanceledException) { throw new TimeoutException(); }
        }

        // TODO SimControl.Templates.CSharp.WcfServiceLibrary tests

        public const string ProcessName = "SimControl.Templates.CSharp.ConsoleApp";
    }
}
