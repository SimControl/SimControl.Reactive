// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public static async Task<int> Main(params string[] args)
        {
            ExitCode exitCode = ExitCode.Success;

            try
            {
                RegisterEventHandlers();

                Thread.CurrentThread.Name ??=nameof(Main);

                InternationalCultureInfo.SetCurrentThreadCulture();
                InternationalCultureInfo.SetDefaultThreadCulture();

                logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "MainAssembly",
                    typeof(Program).Assembly.GetName().Name,
                    FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion,
                    Environment.Version, Environment.Is64BitProcess ? "x64" : "x86", args);

                //TODO SynchronizationContext using (var act = new AsyncContextThread())
                {
                    using var cts = new CancellationTokenSource();
                    ConfiguredTaskAwaitable task = /*act.Factory*/ Task.Run(() => Task.Delay(-1, cts.Token)).ConfigureAwait(false); // replace by async operation

                    for (; ; )
                    {
                        string input;

                        try
                        {
                            input = Console.ReadLine();
                            if (input is null) break;
                        }
                        catch (ObjectDisposedException) { break; }

                        logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ConsoleInput", input);

                        // ...
                    }

                    cts.Cancel();

                    try { await task; }
                    catch (TaskCanceledException) { }
                }
            }
            catch (Exception ex)
            {
                logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, ex);

                exitCode = ExitCode.UnhandledException;
            }

            UnregisterEventHandlers();

            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "Exit", exitCode);

            return (int) exitCode;
        }

        private static bool ConsoleCtrlHandler(uint sig)
        {
            Exit(ExitCode.ConsoleCtrl);

            return true;
        }

        private static void Exit(ExitCode exitCode)
        {
            UnregisterEventHandlers();

            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "Exit", exitCode);

            Environment.Exit((int) exitCode);
        }

        private static void RegisterEventHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;

            if (!NativeMethods.ExternSetConsoleCtrlHandler(ConsoleCtrlHandler, true))
                throw new Win32Exception();
        }

        private static void UnhandledExceptionEventHandler(object _, UnhandledExceptionEventArgs e)
        {
            logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, (Exception) e.ExceptionObject);

            Exit(ExitCode.UnhandledExceptionEvent); // otherwise the CLR would terminate with an application error
        }

        private static void UnobservedTaskExceptionHandler(object _, UnobservedTaskExceptionEventArgs args)
        {
            logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, args.Exception);

            //args.SetObserved(); // as we have observed the exception, the process should not terminate abnormally

            Exit(ExitCode.UnobservedTaskException);
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private static void UnregisterEventHandlers()
        {
            try
            {
                if (!NativeMethods.ExternSetConsoleCtrlHandler(null, false))
                    logger.Message(LogLevel.Error, LogMethod.GetCurrentMethodName(),
                        "Unregister ExternSetConsoleCtrlHandler", Marshal.GetLastWin32Error());

                TaskScheduler.UnobservedTaskException -= UnobservedTaskExceptionHandler;
                AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionEventHandler;
            }
            catch (Exception e) { logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, e); }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }

    internal static class NativeMethods
    {
        // Delegate type to be used as the Handler Routine
        internal delegate bool ConsoleCtrlDelegate(uint ctrlType);

        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler", SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ExternSetConsoleCtrlHandler(ConsoleCtrlDelegate? handlerRoutine,
            [MarshalAs(UnmanagedType.Bool)] bool add);
    }
}
