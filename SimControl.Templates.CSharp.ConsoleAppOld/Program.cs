// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SimControl.Log;

//TODO SynchronizationContext

namespace SimControl.Templates.CSharp.ConsoleAppOld
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
        public static int Main(params string[] args)
        {
            Contract.Requires(args != null);

            try
            {
                RegisterExceptionHandlers();

                if (Thread.CurrentThread.Name == null)
                    Thread.CurrentThread.Name = nameof(Main);

                //InternationalCultureInfo.SetCurrentThreadCulture();
                //LogMethod.SetDefaultThreadCulture();

                logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "MainAssembly",
                    typeof(Program).Assembly.GetName().Name,
                    FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion,
                    DateTime.Now, Environment.Version, Environment.Is64BitProcess ? "x64" : "x86");

                string input;
                while ((input = Console.ReadLine()) != null)
                {
                    logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), input);

                    // ...
                }

                return (int) ExitCode.Success;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031
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

        private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, (Exception) e.ExceptionObject);

            Exit(ExitCode.UnhandledExceptionEvent); // otherwise the CLR would terminate with an application error
        }

        private static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs args)
        {
            logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, args.Exception);

            //args.SetObserved();  // as we have observed the exception, the process should not terminate abnormally

            Exit(ExitCode.UnobservedTaskException);
        }

        private static void UnregisterExceptionHandlers()
        {
            try
            {
                if (!NativeMethods.ExternSetConsoleCtrlHandler(null, false))
                    throw new Win32Exception();

                TaskScheduler.UnobservedTaskException -= UnobservedTaskExceptionHandler;
                AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionEventHandler;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031
            {
                logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, e);
            }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }

    internal static class NativeMethods
    {
        // Delegate type to be used as the Handler Routine
        internal delegate bool ConsoleCtrlDelegate(uint ctrlType);

        //[Log(AttributeExclude = true)]
        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ExternSetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine,
                                                                [MarshalAs(UnmanagedType.Bool)] bool add);
    }
}
