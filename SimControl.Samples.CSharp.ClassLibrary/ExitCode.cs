// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NLog;
using SimControl.Log;

namespace SimControl.Samples.CSharp.ClassLibrary
{
    public enum ExitCode
    {
        Success = 0,
        UnhandledException = 1,
        UnhandledExceptionEvent = 2,
        UnobservedTaskException = 3,
        ConsoleCtrl = 4,
        InvalidCommandlineArguments = 5,
        ThrowExceptionOnThread = 6,
        ConsoleInputClosed = 7
    }
}
