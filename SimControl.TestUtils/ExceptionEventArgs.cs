// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.TestUtils
{
    /// <summary>Additional information for exception events.</summary>
    /// <seealso cref="T:System.EventArgs"/>
    public class ExceptionEventArgs: EventArgs
    {
        /// <summary>Constructor.</summary>
        /// <param name="exception">The exception.</param>
        public ExceptionEventArgs(Exception exception) => Exception = exception;

        /// <summary>Gets or sets the exception.</summary>
        /// <value>The exception.</value>
        public Exception Exception { get; private set; }
    }
}
