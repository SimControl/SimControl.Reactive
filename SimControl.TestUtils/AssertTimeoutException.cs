// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Globalization;

namespace SimControl.TestUtils
{
    /// <summary>Thrown by <see cref="AssertTimeoutExtensions"></see> extension methods</summary>
    [Serializable]
    public class AssertTimeoutException: Exception
    {
        /// <summary>Initializes a new instance of the <see cref="AssertTimeoutException"/> class.</summary>
        /// <param name="timeout">The timeout.</param>
        public AssertTimeoutException(int timeout) :
                base("Test timeout " + timeout.ToString(CultureInfo.InvariantCulture) + " expired")
        { }

        /// <summary>Initializes a new instance of the <see cref="AssertTimeoutException"/> class.</summary>
        public AssertTimeoutException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AssertTimeoutException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public AssertTimeoutException(string message) : base(message) { }

        /// <summary>Initializes a new instance of the <see cref="AssertTimeoutException"/> class.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (
        /// <span class="keyword"><span class="languageSpecificText"><span class="cs">null</span><span class="vb">Nothing</span><span class="cpp">nullptr</span></span></span><span class="nu">a
        /// null reference ( <span class="keyword">Nothing</span> in Visual Basic)</span> in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public AssertTimeoutException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>Initializes a new instance of the <see cref="AssertTimeoutException"/> class.</summary>
        /// <param name="serializationInfo">The serialization information.</param>
        /// <param name="streamingContext">The streaming context.</param>
        protected AssertTimeoutException(System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        { }
    }
}
