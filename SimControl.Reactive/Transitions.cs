// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.Contracts;

// TODO: CR

namespace SimControl.Reactive
{
    /// <summary>Internal transition.</summary>
    public sealed class InternalTransition: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="InternalTransition"/> class.</summary>
        /// <param name="trigger">Transition trigger.</param>
        /// <param name="guard">Transition guard.</param>
        /// <param name="effect">Transition effect action.</param>
        /// <param name="name">Transition name.</param>
        public InternalTransition(Trigger trigger = null, Constraint guard = null, Effect effect = null,
                                  string name = null) : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>Local trransition.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class LocalTransition: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="LocalTransition"/> class.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guard">The guard.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="name">The name.</param>
        public LocalTransition(Trigger trigger = null, Constraint guard = null, Effect effect = null, string name = null)
            : base(TransitionKind.Internal, null, trigger, guard, effect, name) => ContractRequiredName(name);
    }

    /// <summary>External transition.</summary>
    public sealed class Transition: TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="Transition"/> class.</summary>
        /// <param name="target">Target state name.</param>
        /// <param name="trigger">Transition trigger.</param>
        /// <param name="guard">Transition guard.</param>
        /// <param name="effect">Transition effect action.</param>
        /// <param name="name">Transition name.</param>
        public Transition(string target, Trigger trigger = null, Constraint guard = null, Effect effect = null,
                          string name = null): base(TransitionKind.External, target, trigger, guard, effect, name)
        {
            ContractRequiredName(name);
            Contract.Requires(!string.IsNullOrWhiteSpace(target));
        }
    }

    /// <summary>Base class for transitions</summary>
    public class TransitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="TransitionBase"/> class.</summary>
        /// <param name="kind">transition kind.</param>
        /// <param name="target">Target state name.</param>
        /// <param name="trigger">Transition trigger.</param>
        /// <param name="guard">Transition guard.</param>
        /// <param name="effect">Transition effect action.</param>
        /// <param name="name">Transition name.</param>
        internal TransitionBase(TransitionKind kind, string target, Trigger trigger, Delegate guard, Delegate effect,
                                string name)
        {
            ContractRequiredName(name);

            Kind = kind;
            Target = target;
            Trigger = trigger;
            Guard = guard;
            Effect = effect;
            Name = name ?? nameof(Transition) + autoNameTransitions++.ToString(InternationalCultureInfo.Instance);
        }

        /// <inheritdoc/>
        public override string ToString() => LogFormat.FormatObject(GetType(), Name, SourceState.FullName, TargetState.FullName);

        /// <summary>Code contract for validating transition names.</summary>
        /// <param name="name">The name.</param>
        //[ContractAbbreviator]
        protected static void ContractRequiredName(string name) => Contract.Requires(name == null || name.Length > 0);

        internal Delegate Effect { get; }

        internal Delegate Guard { get; }

        internal TransitionKind Kind { get; }

        /// <summary>Readable name for reporting transition executions.</summary>
        public string Name { get; set; }

        internal State SourceState { get; set; }

        internal string Target { get; set; }

        internal State TargetState { get; set; }

        internal Trigger Trigger { get; }
        private static int autoNameTransitions;

        internal enum TransitionKind
        {
            External,
            Internal,
            Local
        }
    }

    /// <summary>Base class for triggers.</summary>
    public class Trigger
    {
        internal virtual bool Matches(Trigger trigger)
        {
            Contract.Requires(trigger != null);

            Contract.Assert(false);

            return false;
        }
    }
}
