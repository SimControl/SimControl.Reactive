// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SimControl.Log;

namespace SimControl.Templates.CSharp.ConsoleApp
{
    /// <summary>Console application main class.</summary>
    [Log]
    public static class Program
    {
        private enum ExitCode
        {
            Success = 0,
            UnhandledException = 1,
            UnhandledExceptionEvent = 2,
            UnobservedTaskException = 3,
            ConsoleCtrl = 4,
        }

        /// <summary>Console application entry point.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Return code.</returns>
        public static async Task<int> Main(params string[] args)
        {
            try
            {
                RegisterExceptionHandlers();

                if (Thread.CurrentThread.Name == null) Thread.CurrentThread.Name = nameof(Main);

                InternationalCultureInfo.SetCurrentThreadCulture();
                InternationalCultureInfo.SetDefaultThreadCulture();

                logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "MainAssembly",
                    typeof(Program).Assembly.GetName().Name,
                    FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion,
                    Environment.Version, Environment.Is64BitProcess ? "x64" : "x86", args);

                //UNDONE using (var act = new AsyncContextThread())
                {
                    using (var cts = new CancellationTokenSource())
                    {
                        //UNDONE Task task = act.Factory.Run(() => Task.Delay(-1, cts.Token));

                        for (; ; )
                        {
                            string input;

                            try
                            {
                                input = Console.ReadLine();
                                if (input == null) break;
                            }
                            catch (ObjectDisposedException) { break; }

                            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), input);

                            // ...
                        }

                        cts.Cancel();

                        //UNDONE try { await task; }
                        //UNDONE catch (TaskCanceledException) { }

                        //UNDONE act.JoinAsync().Wait();
                    }
                }

                return (int) ExitCode.Success;
            }
            catch (Exception ex)
            {
                logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, ex);

                return (int) ExitCode.UnhandledException;
            }
            finally { UnregisterExceptionHandlers(); }
        }

        private static bool ConsoleCtrlHandler(uint sig)
        {
            Exit(ExitCode.ConsoleCtrl);

            return true;
        }

        private static void Exit(ExitCode exitCode)
        {
            UnregisterExceptionHandlers();

            Environment.Exit((int) exitCode);
        }

        private static void RegisterExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;

            if (!NativeMethods.ExternSetConsoleCtrlHandler(ConsoleCtrlHandler, true))
                throw new Win32Exception();
        }

        private static void UnhandledExceptionEventHandler(object _, UnhandledExceptionEventArgs e)
        {
            logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, (Exception) e.ExceptionObject);

            Exit(ExitCode.UnhandledExceptionEvent); // otherwise the CLR would terminate with an application error
        }

        private static void UnobservedTaskExceptionHandler(object _, UnobservedTaskExceptionEventArgs args)
        {
            logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, args.Exception);

            //args.SetObserved(); // as we have observed the exception, the process should not terminate abnormally

            Exit(ExitCode.UnobservedTaskException);
        }

        private static void UnregisterExceptionHandlers()
        {
            try
            {
                if (!NativeMethods.ExternSetConsoleCtrlHandler(null, false)) throw new Win32Exception();

                TaskScheduler.UnobservedTaskException -= UnobservedTaskExceptionHandler;
                AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionEventHandler;
            }
            catch (Exception e) { logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, e); }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }

    internal static class NativeMethods //UNDONE netcoreapp3.1 compatibility
    {
        // Delegate type to be used as the Handler Routine
        internal delegate bool ConsoleCtrlDelegate(uint ctrlType);

        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ExternSetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine,
                                                                [MarshalAs(UnmanagedType.Bool)] bool add);
    }
}
