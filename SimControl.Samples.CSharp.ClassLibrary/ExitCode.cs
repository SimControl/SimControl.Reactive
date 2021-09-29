// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

namespace SimControl.Samples.CSharp.ClassLibrary
{
    /// <summary>SimControl.Samples.CSharp.ConsoleApp exit codes</summary>
    public enum ExitCode
    {
        /// <summary>Success</summary>
        Success = 0,

        /// <summary>Unhandled exception</summary>
        UnhandledException = 1,

        /// <summary>Unhandled exception event</summary>
        UnhandledExceptionEvent = 2,

        /// <summary>Unobserved task exception</summary>
        UnobservedTaskException = 3,

        /// <summary>Console control</summary>
        ConsoleCtrl = 4,

        /// <summary>Invalid commandline arguments</summary>
        InvalidCommandlineArguments = 5,

        /// <summary>Exception thrown on thread</summary>
        ThrowExceptionOnThread = 6,

        /// <summary>Console input closed</summary>
        ConsoleInputClosed = 7
    }
}
