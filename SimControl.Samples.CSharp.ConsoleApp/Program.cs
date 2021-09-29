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
using SimControl.Samples.CSharp.ClassLibrary;

namespace SimControl.Samples.CSharp.ConsoleApp
{
    /// <summary>Console application main class.</summary>
    [Log]
    public static class Program
    {
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

                if (args.Length != 1) Exit(ExitCode.InvalidCommandlineArguments);

                command = args[0];
                logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), command);

                switch (command)
                {
                    case "AsyncContextThread":
                    {
                        //TODO AsyncContextThread
                        using var cts = new CancellationTokenSource();
                        ConfiguredTaskAwaitable task = Task.Run(() =>Task.Delay(-1, cts.Token)).ConfigureAwait(false);
                        cts.Cancel();
                        try { await task; }
                        catch (TaskCanceledException) { }
                        Exit(ExitCode.InvalidCommandlineArguments);
                    }
                    break;
                    case "Normal":
                        var sampleClass = new SampleClass();
                        sampleClass.DoSomething();
                        break;
                    case nameof(ThrowException):
                        ThrowException();
                        break;
                    case "ThrowExceptionOnThread":
                        var thread = new Thread(ThrowException);
                        thread.Start();
                        Console.ReadLine();
                        break;
                    case nameof(VerifyJitOptimization):
                        VerifyJitOptimization.Run();
                        ClassLibrary.VerifyJitOptimization.Run();
                        break;
                    case "Wait":
                        for (; ; )
                        {
                            string? input;

                            try
                            {
                                input = Console.ReadLine();
                                if (input is null)
                                    Exit(ExitCode.ConsoleInputClosed);
                                else
                                    logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ConsoleInput",
                                        input);
                            }
                            catch (ObjectDisposedException) { break; }
                        }
                        break;
                    case "WCF":
                        //TODO WCF service hosting
                        Exit(ExitCode.InvalidCommandlineArguments);
                        //private const string testBaseAddress =
                        //    "http://localhost:8733/Design_Time_Addresses/SimControl.Samples.CSharp.Wcf.Service/SampleServicePerSession/";
                        //logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "WCF");
                        //var host = new ServiceHost(typeof(SampleServicePerSessionInstance),//, new Uri(TestBaseAddress));
                        //host.Open();
                        //var client = new SampleServiceClient(new InstanceContext(new SampleServiceEapCallback()),
                        //    typeof(SampleServiceClient).FullName, new EndpointAddress(TestBaseAddress));
                        //((WSDualHttpBinding) client.Endpoint.Binding).ClientBaseAddress =  new Uri(host.BaseAddresses[0]  + "Callback"); //TestBaseAddress
                        //client.Connect();
                        //Console.ReadLine();
                        //client.Close();
                        //host.Close();
                        break;
                    default:
                        Exit(ExitCode.InvalidCommandlineArguments);
                        break;
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

        private static void ThrowException() => throw new InvalidOperationException();

        private static void UnhandledExceptionEventHandler(object _, UnhandledExceptionEventArgs e)
        {
            logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, (Exception) e.ExceptionObject);

            Exit(command == "ThrowExceptionOnThread" ?
                ExitCode.ThrowExceptionOnThread : ExitCode.UnhandledExceptionEvent);
        }

        private static void UnobservedTaskExceptionHandler(object _, UnobservedTaskExceptionEventArgs args)
        {
            logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, args.Exception);

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
        private static string command = "";
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
