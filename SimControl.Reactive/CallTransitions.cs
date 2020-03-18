// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

// TODO: CR

namespace SimControl.Reactive
{
    /// <summary>External state machine transition with one call trigger argument.</summary>
    /// <typeparam name="T1"></typeparam>
    public sealed class Transition<T1>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, IGenericTrigger<T1> trigger = null, Constraint<T1> guard = null,
                          Effect<T1> effect = null, string name = null)
            : base(TransitionKind.External, target, (Trigger) trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with two call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public sealed class Transition<T1, T2>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2> trigger = null, Constraint<T1, T2> guard = null,
                          Effect<T1, T2> effect = null, string name = null)
            : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with three call trigger argument.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public sealed class Transition<T1, T2, T3>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2, T3&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2, T3> trigger = null, Constraint<T1, T2, T3> guard = null,
                          Effect<T1, T2, T3> effect = null, string name = null)
            : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with four call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public sealed class Transition<T1, T2, T3, T4>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2, T3, T4&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2, T3, T4> trigger = null,
                          Constraint<T1, T2, T3, T4> guard = null, Effect<T1, T2, T3, T4> effect = null,
                          string name = null) : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with five call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    public sealed class Transition<T1, T2, T3, T4, T5>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2, T3, T4, T5&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2, T3, T4, T5> trigger = null,
                          Constraint<T1, T2, T3, T4, T5> guard = null, Effect<T1, T2, T3, T4, T5> effect = null,
                          string name = null) : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with six call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    public sealed class Transition<T1, T2, T3, T4, T5, T6>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2, T3, T4, T5, T6&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2, T3, T4, T5, T6> trigger = null,
                          Constraint<T1, T2, T3, T4, T5, T6> guard = null, Effect<T1, T2, T3, T4, T5, T6> effect = null,
                          string name = null) : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with 7 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    public sealed class Transition<T1, T2, T3, T4, T5, T6, T7>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2, T3, T4, T5, T6, T7> trigger = null,
                          Constraint<T1, T2, T3, T4, T5, T6, T7> guard = null,
                          Effect<T1, T2, T3, T4, T5, T6, T7> effect = null, string name = null)
            : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with 10 call trigger argument.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    public sealed class Transition<T1, T2, T3, T4, T5, T6, T7, T8>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8> trigger = null,
                          Constraint<T1, T2, T3, T4, T5, T6, T7, T8> guard = null,
                          Effect<T1, T2, T3, T4, T5, T6, T7, T8> effect = null, string name = null)
            : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with 9 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    public sealed class Transition<T1, T2, T3, T4, T5, T6, T7, T8, T9>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8, T9> trigger = null,
                          Constraint<T1, T2, T3, T4, T5, T6, T7, T8, T9> guard = null,
                          Effect<T1, T2, T3, T4, T5, T6, T7, T8, T9> effect = null, string name = null)
            : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External state machine transition with 10 call trigger argument.</summary>
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
    public sealed class Transition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/>
        ///     class.</summary>
        /// <param name="target">The target.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public Transition(string target, CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> trigger = null,
                          Constraint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> guard = null,
                          Effect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> effect = null, string name = null)
            : base(TransitionKind.External, target, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with one call trigger argument.</summary>
    /// <typeparam name="T1"></typeparam>
    public sealed class InternalTransition<T1>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1> trigger = null, Constraint<T1> guard = null, Effect<T1> effect = null,
                                  string name = null) : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with two call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public sealed class InternalTransition<T1, T2>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1, T2&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2> trigger = null, Constraint<T1, T2> guard = null,
                                  Effect<T1, T2> effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with three call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public sealed class InternalTransition<T1, T2, T3>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1, T2, T3&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2, T3> trigger = null, Constraint<T1, T2, T3> guard = null,
                                  Effect<T1, T2, T3> effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with four call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public sealed class InternalTransition<T1, T2, T3, T4>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1, T2, T3, T4&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2, T3, T4> trigger = null, Constraint<T1, T2, T3, T4> guard = null,
                                  Effect<T1, T2, T3, T4> effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with five call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    public sealed class InternalTransition<T1, T2, T3, T4, T5>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1, T2, T3, T4, T5&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2, T3, T4, T5> trigger = null,
                                  Constraint<T1, T2, T3, T4, T5> guard = null, Effect<T1, T2, T3, T4, T5> effect = null,
                                  string name = null) : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with 6 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    public sealed class InternalTransition<T1, T2, T3, T4, T5, T6>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1, T2, T3, T4, T5, T6&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2, T3, T4, T5, T6> trigger = null,
                                  Constraint<T1, T2, T3, T4, T5, T6> guard = null,
                                  Effect<T1, T2, T3, T4, T5, T6> effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with 7 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    public sealed class InternalTransition<T1, T2, T3, T4, T5, T6, T7>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2, T3, T4, T5, T6, T7> trigger = null,
                                  Constraint<T1, T2, T3, T4, T5, T6, T7> guard = null,
                                  Effect<T1, T2, T3, T4, T5, T6, T7> effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with 8 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    public sealed class InternalTransition<T1, T2, T3, T4, T5, T6, T7, T8>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/>
        ///     class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8> trigger = null,
                                  Constraint<T1, T2, T3, T4, T5, T6, T7, T8> guard = null,
                                  Effect<T1, T2, T3, T4, T5, T6, T7, T8> effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with 9 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    public sealed class InternalTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/>
        ///     class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8, T9> trigger = null,
                                  Constraint<T1, T2, T3, T4, T5, T6, T7, T8, T9> guard = null,
                                  Effect<T1, T2, T3, T4, T5, T6, T7, T8, T9> effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Internal state machine transition with 10 call trigger argument.</summary>
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
    public sealed class InternalTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>: TransitionBase
    {
        /// <summary>Initializes a new instance of the
        ///     <see cref="InternalTransition&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public InternalTransition(CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> trigger = null,
                                  Constraint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> guard = null,
                                  Effect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with one call trigger argument.</summary>
    /// <typeparam name="T1"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1> trigger = null, Constraint<T1> guard = null, Effect<T1> effect = null,
                               string name = null) : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with two call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2> trigger = null, Constraint<T1, T2> guard = null,
                               Effect<T1, T2> effect = null, string name = null)
            : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with three call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2, T3>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2, T3&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2, T3> trigger = null, Constraint<T1, T2, T3> guard = null,
                               Effect<T1, T2, T3> effect = null, string name = null)
            : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with 4 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2, T3, T4>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2, T3, T4&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2, T3, T4> trigger = null, Constraint<T1, T2, T3, T4> guard = null,
                               Effect<T1, T2, T3, T4> effect = null, string name = null)
            : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with 5 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2, T3, T4, T5>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2, T3, T4, T5&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2, T3, T4, T5> trigger = null,
                               Constraint<T1, T2, T3, T4, T5> guard = null, Effect<T1, T2, T3, T4, T5> effect = null,
                               string name = null) : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with 6 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2, T3, T4, T5, T6>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2, T3, T4, T5, T6&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2, T3, T4, T5, T6> trigger = null,
                               Constraint<T1, T2, T3, T4, T5, T6> guard = null,
                               Effect<T1, T2, T3, T4, T5, T6> effect = null, string name = null)
            : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with 7 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2, T3, T4, T5, T6, T7>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2, T3, T4, T5, T6, T7> trigger = null,
                               Constraint<T1, T2, T3, T4, T5, T6, T7> guard = null,
                               Effect<T1, T2, T3, T4, T5, T6, T7> effect = null, string name = null)
            : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with 8 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2, T3, T4, T5, T6, T7, T8>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8> trigger = null,
                               Constraint<T1, T2, T3, T4, T5, T6, T7, T8> guard = null,
                               Effect<T1, T2, T3, T4, T5, T6, T7, T8> effect = null, string name = null)
            : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with 9 call trigger arguments.</summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/>
        ///     class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8, T9> trigger = null,
                               Constraint<T1, T2, T3, T4, T5, T6, T7, T8, T9> guard = null,
                               Effect<T1, T2, T3, T4, T5, T6, T7, T8, T9> effect = null, string name = null)
            : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local state machine transition with 10 call trigger arguments.</summary>
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
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/>
        ///     class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(CallTrigger<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> trigger = null,
                               Constraint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> guard = null,
                               Effect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> effect = null, string name = null)
            : base(TransitionKind.Local, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }
}
