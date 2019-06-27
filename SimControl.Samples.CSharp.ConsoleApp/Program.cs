// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SimControl.Log;
//using SimControl.Reactive;
//using SimControl.Samples.CSharp.ClassLibraryEx;
//using SimControl.Samples.CSharp.ConsoleApplication.Properties;

namespace SimControl.Samples.CSharp.ConsoleApplication
{
    /// <summary>Console application main class.</summary>
    [Log]
    public static class Program
    {
        /// <summary>Console application entry point.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="args">.</param>
        /// <returns>Exit-code for the process - 0 for success, else an error code.</returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CSharpConsoleApplicationAppSetting")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static int Main(params string[] args)
        {
            /*
                        Contract.Requires(args != null);
                        Contract.Requires(args.Length == 1);
                        Contract.Requires(Arguments.Contains(args[0]));

                        try
                        {
                            RegisterExceptionHandlers();

                            if (Thread.CurrentThread.Name == null)
                                Thread.CurrentThread.Name = nameof(Main);

                            InternationalCultureInfo.SetCurrentThreadCulture();
                            LogMethod.SetDefaultThreadCulture();

                            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "MainAssembly",
                                typeof(Program).Assembly.GetName().Name,
                                FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion,
                                DateTime.Now, Environment.Version, Environment.Is64BitProcess ? "x64" : "x86");

                            command = args[0];

                            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), command);

                            AppDomain.CurrentDomain.ProcessExit += ProcessExitEventHandler;
                            Console.CancelKeyPress += ProcessExitEventHandler;

                            var sampleClass = new SampleClass();

                            switch (command)
                            {
                                case "ChangeUserSettings":
                                    logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "ChangeUserSettings");
                                    Settings.Default.CSharpConsoleApplication_UserSetting =
                                        "CSharpConsoleApplication_UserSetting_Changed";
                                    SampleClass.ChangeUserSettings("CSharpClassLibrary_UserSetting_Changed");
                                    Settings.Default.Save();
                                    break;
                                case "Normal":
                                    logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(),
                                        "CSharpConsoleApplication_AppSetting", Settings.Default.CSharpConsoleApplication_AppSetting);
                                    logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(),
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
                                    logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "ThrowExceptionOnThread");
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
                                    logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), nameof(VerifyJitOptimization));
                                    Console.WriteLine("Press 'Return' to continue");
                                    Console.ReadLine();
                                    VerifyJitOptimization.Run();
                                    ClassLibraryEx.VerifyJitOptimization.Run();
                                    break;
                                case "Wait":
                                    logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "Waiting");
                                    string s = Console.ReadLine();
                                    if (s == null)
                                        Environment.Exit(5);
                                    break;
                                case "WCF":
                                    //TODO WCF service hosting
                                    //private const string testBaseAddress =
                                    //    "http://localhost:8733/Design_Time_Addresses/SimControl.Samples.CSharp.Wcf.Service/SampleServicePerSession/";

                                    //logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "WCF");

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
                logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, e);

                return command == nameof(ThrowException) ? 7 : 1;
            }
            finally { UnregisterExceptionHandlers(); }
*/
            return 0;
        }

        [SuppressMessage("Usage", "CC0057:Unused parameters", Justification = "<Pending>")]
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

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static void ProcessExitEventHandler(object sender, EventArgs e)
        {
            // not alway raised, logger already finalized
        }

        private static void RegisterExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;

            if (!NativeMethods.ExternSetConsoleCtrlHandler(ConsoleCtrlHandler, true))
                throw new Win32Exception();
        }

        private static void ThrowException() => throw new InvalidOperationException();

        private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, (Exception) e.ExceptionObject);

            Exit(command == "ThrowExceptionOnThread" ? 6 : 2);
        }

        private static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs args)
        {
            logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, args.Exception);

            //args.SetObserved();  // as we have observed the exception, the process should not terminate abnormally

            Exit(3);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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
                logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null, e);
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
        internal static extern bool ExternSetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine,
                                                                [MarshalAs(UnmanagedType.Bool)] bool add);
    }
}
