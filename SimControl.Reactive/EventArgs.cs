// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

// TODO: CR

namespace SimControl.Reactive
{
    /// <summary>Generic EventArgs.</summary>
    /// <typeparam name="T"></typeparam>
    public class EventArgs<T>: EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="EventArgs&lt;T&gt;"/> class.</summary>
        /// <param name="arg">The arg.</param>
        public EventArgs(T arg) => this.arg = arg;

        /// <summary>Performs an implicit conversion from <see cref="Reactive.EventArgs&lt;T&gt;"/> to T.</summary>
        /// <param name="eventArgs">The <see cref="Reactive.EventArgs&lt;T&gt;"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator T(EventArgs<T> eventArgs)
        {
            Contract.Requires(eventArgs != null);

            return eventArgs.arg;
        }

        private readonly T arg;
    }
}
