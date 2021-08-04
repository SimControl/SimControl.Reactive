// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Globalization;

namespace SimControl.TestUtils
{
    /// <summary>Thrown by <see cref="AssertTimeoutExtensions"></see> extension methods</summary>
    public class AssertTimeoutException: Exception
    {
        /// <summary>Initializes a new instance of the <see cref="AssertTimeoutException"/> class.</summary>
        /// <param name="timeout">The timeout.</param>
        public AssertTimeoutException(int timeout) :
                base("Test timeout " + timeout.ToString(CultureInfo.InvariantCulture) + " expired")
        { }
    }
}
