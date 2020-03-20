// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.TestUtils
{
    public class ExceptionEventArgs: EventArgs
    {
        public ExceptionEventArgs(Exception exception) => Exception = exception;

        public Exception Exception { get; private set; }
    }
}
