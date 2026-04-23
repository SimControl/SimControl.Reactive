// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

namespace SimControl.TestUtils;

/// <summary>Additional information for exception events.</summary>
public class ExceptionEventArgs : EventArgs
{
    /// <summary>Constructor.</summary>
    /// <param name="exception">The exception.</param>
    public ExceptionEventArgs(Exception exception) => Exception = exception;

    /// <summary>Gets or sets the exception.</summary>
    /// <value>The exception.</value>
    public Exception Exception { get; }
}
