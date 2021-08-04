// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.Reactive
{
    /// <summary>Interface for generic trigger with only type parameter.</summary>
    /// <typeparam name="T"></typeparam>
    /// <tparam name="T">Generic type parameter.</tparam>
    public interface IGenericTrigger<T> { }

    /// <summary>Call trigger with no arguments.</summary>
    public sealed class CallTrigger: CallTriggerBase
    {
        /// <summary>Initializes a new instance of the <see cref="CallTrigger"/> class.</summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with 0 arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    public sealed class CallTrigger<T1>: CallTriggerBase, IGenericTrigger<T1>
    {
        /// <summary>Initializes a new instance of the <see cref="CallTrigger&lt;T1&gt;"/> class.</summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1> method) : base(method) { } // Contract.Requires(method != null);

        /// <summary>
        /// Initializes a new instance of the <see cref="CallTrigger&lt;T1&gt;"/> class for specifying proprty setters.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="propertyName">The roperty name.</param>
        public CallTrigger(object target, string propertyName)
            : base(
                Delegate.CreateDelegate(typeof(Effect<T1>),
                    target,
                    target.GetType().GetProperty(propertyName).GetSetMethod()))
        {
            // Contract.Requires(target != null);
            // Contract.Requires(target.GetType().GetProperty(propertyName) != null);
            // Contract.Requires(target.GetType().GetProperty(propertyName).GetSetMethod() != null);
        }
    }

    /// <summary>Call trigger with two arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public sealed class CallTrigger<T1, T2>: CallTriggerBase
    {
        /// <summary>Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2&gt;"/> class.</summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with three arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public sealed class CallTrigger<T1, T2, T3>: CallTriggerBase
    {
        /// <summary>Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2, T3&gt;"/> class.</summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2, T3> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with 4 arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public sealed class CallTrigger<T1, T2, T3, T4>: CallTriggerBase
    {
        /// <summary>Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2, T3, T4&gt;"/> class.</summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2, T3, T4> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with 5 arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    public sealed class CallTrigger<T1, T2, T3, T4, T5>: CallTriggerBase
    {
        /// <summary>Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2, T3, T4, T5&gt;"/> class.</summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2, T3, T4, T5> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with 6 arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    public sealed class CallTrigger<T1, T2, T3, T4, T5, T6>: CallTriggerBase
    {
        /// <summary>Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2, T3, T4, T5, T6&gt;"/> class.</summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2, T3, T4, T5, T6> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with 7 arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    public sealed class CallTrigger<T1, T2, T3, T4, T5, T6, T7>: CallTriggerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2, T3, T4, T5, T6, T7> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with 8 arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    public sealed class CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8>: CallTriggerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2, T3, T4, T5, T6, T7, T8> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with 9 arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    public sealed class CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8, T9>: CallTriggerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2, T3, T4, T5, T6, T7, T8, T9> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call trigger with 10 arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="T10"></typeparam>
    public sealed class CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>: CallTriggerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallTrigger&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public CallTrigger(Effect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method) : base(method) { } // Contract.Requires(method != null);
    }

    /// <summary>Call triggers base class</summary>
    public class CallTriggerBase: Trigger
    {
        /// <summary>Initializes a new instance of the <see cref="CallTriggerBase"/> class.</summary>
        /// <param name="method">Call trigger delegate.</param>
        protected CallTriggerBase(Delegate method)
        {
            // Contract.Requires(method != null);

            Method = method;
        }

        internal override bool Matches(Trigger trigger) => trigger is CallTriggerBase other && Method == other.Method;

        internal Delegate Method { get; }
    }
}
