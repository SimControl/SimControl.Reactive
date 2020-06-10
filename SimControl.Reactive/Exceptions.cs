// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

// TODO: CR

namespace SimControl.Reactive
{
    /// <summary>Exception thrown during state machine initialization if a transition source or target is not found.</summary>
    public class StateMachineException: Exception
    {
        /// <summary>Initializes a new instance of the <see cref="StateMachineException"/> class.</summary>
        public StateMachineException() { }

        /// <summary>Initializes a new instance of the <see cref="StateMachineException"/> class.</summary>
        /// <param name="message">The message.</param>
        public StateMachineException(string message) : base(message) { }

        /// <summary>Initializes a new instance of the <see cref="StateMachineException"/> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public StateMachineException(string message, Exception innerException) : base(message, innerException) { }
    }
}
