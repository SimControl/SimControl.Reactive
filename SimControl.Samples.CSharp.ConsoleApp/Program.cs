// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SimControl.Log;

// UNDONE base console application structure
// TODO settings operations

//using SimControl.Reactive;
//using SimControl.Samples.CSharp.ClassLibraryEx;
//using SimControl.Samples.CSharp.ConsoleApplication.Properties;

namespace SimControl.Samples.CSharp.ConsoleApplication
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

        public static async Task<int> Main(params string[] args)
        {
            ExitCode exitCode = ExitCode.Success;

            try
            {
                RegisterExceptionHandlers();

                if (Thread.CurrentThread.Name == null) Thread.CurrentThread.Name = nameof(Main);

                InternationalCultureInfo.SetCurrentThreadCulture();
                InternationalCultureInfo.SetDefaultThreadCulture();

                logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "MainAssembly",
                    typeof(Program).Assembly.GetName().Name,
                    FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion,
                    Environment.Version, Environment.Is64BitProcess ? "x64" : "x86", args);

                {
                    for (; ; )
                    {
                        string input;

                        try
                        {
                            input = Console.ReadLine();
                            if (input == null) break;
                        }
                        catch (ObjectDisposedException) { break; }

                        logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ConsoleInput", input);

                        // ...
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, ex);

                exitCode = ExitCode.UnhandledException;
            }

            UnregisterExceptionHandlers();

            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "Exit", exitCode);

            return (int) exitCode;
        }



        /// <summary>Console application entry point.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <returns>Exit-code for the process - 0 for success, else an error code.</returns>
        public static int OldMain(params string[] _/*args*/)
        {
            command = "";
            /*
                        // UNDONE Contract.Requires(args != null);
                        // UNDONE Contract.Requires(args.Length == 1);
                        // UNDONE Contract.Requires(Arguments.Contains(args[0]));

                        try
                        {
                            RegisterExceptionHandlers();

                            if (Thread.CurrentThread.Name == null)
                                Thread.CurrentThread.Name = nameof(Main);

                            InternationalCultureInfo.SetCurrentThreadCulture();
                            LogMethod.SetDefaultThreadCulture();

                            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "MainAssembly",
                                typeof(Program).Assembly.GetName().Name,
                                FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion,
                                DateTime.Now, Environment.Version, Environment.Is64BitProcess ? "x64" : "x86");

                            command = args[0];

                            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), command);

                            AppDomain.CurrentDomain.ProcessExit += ProcessExitEventHandler;
                            Console.CancelKeyPress += ProcessExitEventHandler;

                            var sampleClass = new SampleClass();

                            switch (command)
                            {
                                case "ChangeUserSettings":
                                    logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ChangeUserSettings");
                                    Settings.Default.CSharpConsoleApplication_UserSetting =
                                        "CSharpConsoleApplication_UserSetting_Changed";
                                    SampleClass.ChangeUserSettings("CSharpClassLibrary_UserSetting_Changed");
                                    Settings.Default.Save();
                                    break;
                                case "Normal":
                                    logger.Message(LogLevel.Debug, LogMethod.GetCurrentMethodName(),
                                        "CSharpConsoleApplication_AppSetting", Settings.Default.CSharpConsoleApplication_AppSetting);
                                    logger.Message(LogLevel.Debug, LogMethod.GetCurrentMethodName(),
                                        "CSharpConsoleApplication_UserSetting",
                                        Settings.Default.CSharpConsoleApplication_UserSetting);
                                    SampleClass.LogSettings();
                                    sampleClass.DoSomething();
                                    SampleClass.ValidateCodeContract(true);
                                    break;

                                case nameof(ThrowException):
                                    ThrowException();
                                    break;
                                case "ThrowExceptionOnThread":
                                    logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "ThrowExceptionOnThread");
                                    var thread = new Thread(ThrowException);
                                    thread.Start();
                                    Console.ReadLine();
                                    break;
                                case "ValidateSettings":
                                    if (Settings.Default.CSharpConsoleApplication_AppSetting !=
                                        "CSharpConsoleApplication_AppSetting_Test")
                                        throw new InvalidOperationException(
                                            "Invalid Default.CSharpConsoleApplication_AppSetting: " +
                                            Settings.Default.CSharpConsoleApplication_AppSetting);
                                    if (Settings.Default.CSharpConsoleApplication_UserSetting !=
                                        "CSharpConsoleApplication_UserSetting_Test")
                                        throw new InvalidOperationException(
                                            "Invalid Default.CSharpConsoleApplication_UserSetting: " +
                                            Settings.Default.CSharpConsoleApplication_UserSetting);
                                    SampleClass.ValidateSettings();
                                    break;
                                case nameof(VerifyJitOptimization):
                                    logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), nameof(VerifyJitOptimization));
                                    Console.WriteLine("Press 'Return' to continue");
                                    Console.ReadLine();
                                    VerifyJitOptimization.Run();
                                    ClassLibraryEx.VerifyJitOptimization.Run();
                                    break;
                                case "Wait":
                                    logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "Waiting");
                                    string s = Console.ReadLine();
                                    if (s == null)
                                        Environment.Exit(5);
                                    break;
                                case "AsyncContextThread"
                                case "WCF":
                                    //TODO WCF service hosting
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
                }

                return 0;
            }
            catch (Exception e)
            {
                logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, e);

                return command == nameof(ThrowException) ? 7 : 1;
            }
            finally { UnregisterExceptionHandlers(); }
*/
            return 0;
        }

        private static bool ConsoleCtrlHandler(uint sig)
        {
            Exit(4);

            return true;
        }

        private static void Exit(int exitCode)
        {
            UnregisterExceptionHandlers();

            Environment.Exit(exitCode);
        }

        private static void ProcessExitEventHandler(object sender, EventArgs e)
        {
            // not alway raised, logger already finalized
        }

        private static void RegisterExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;

            if (!NativeMethods.ExternSetConsoleCtrlHandler(null, false))
                logger.Error(LogMethod.GetCurrentMethodName(), "Unregister ExternSetConsoleCtrlHandler",
                    Marshal.GetLastWin32Error());
        }

        private static void ThrowException() => throw new InvalidOperationException();

        private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, (Exception) e.ExceptionObject);

            Exit(command == "ThrowExceptionOnThread" ? 6 : 2);
        }

        private static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs args)
        {
            logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, args.Exception);

            //args.SetObserved();  // as we have observed the exception, the process should not terminate abnormally

            Exit(3);
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
            catch (Exception e)
            {
                logger.Exception(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, e);
            }
        }

        /// <summary>Possible command line arguments.</summary>
        /// <value>The arguments.</value>
        public static IEnumerable<string> Arguments => new[] { "ChangeUserSettings", "Normal", nameof(ThrowException),
            "ThrowExceptionOnThread", "ValidateSettings", nameof(VerifyJitOptimization), "Wait", "WCF"
        }.AsEnumerable();

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static string command;
    }

    internal static class NativeMethods
    {
        // Delegate type to be used as the Handler Routine
        internal delegate bool ConsoleCtrlDelegate(uint ctrlType);

        //        [Log(AttributeExclude = true)]
        [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
        internal static extern bool ExternSetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine,
                                                                [MarshalAs(UnmanagedType.Bool)] bool add);
    }
}
