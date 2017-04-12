// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

// TODO: CR

namespace SimControl.Reactive
{
    /*
    public interface IState
    {
    }
    */

    /// <summary>UML state machine "composite" state.</summary>
    public class CompositeState: ConcreteState //TODO sealed
    {
        /// <summary>Initializes a new instance of the <see cref="CompositeState"/> class.</summary>
        /// <param name="name">State name.</param>
        /// <param name="entry">Entry action.</param>
        /// <param name="doActivity">Do activity.</param>
        /// <param name="exit">Exit action.</param>
        /// <param name="deferrable">Deferrable triggers.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "doActivity")]
        public CompositeState(string name, Effect entry = null, Func<Task> doActivity = null, Effect exit = null,
                              Trigger[] deferrable = null) : base(name, entry, doActivity, exit, deferrable) => ContractRequiredName(name);

        /// <summary>Gets or sets the active.</summary>
        /// <value>The active.</value>
        public State Active { get; internal set; }

        /// <summary>Adds the specified outgoing transitions.</summary>
        /// <param name="transitions">Transitions originating from this state.</param>
        /// <returns>This state instance</returns>
        new public CompositeState Add(params TransitionBase[] transitions)
        {
            Contract.Requires(transitions != null);

            base.Add(transitions);
            return this;
        }

        /// <summary>Adds the specified sub states.</summary>
        /// <param name="states">Sub states of this state instance.</param>
        /// <returns>This state instance</returns>
        public CompositeState Add(params State[] states)
        {
            Contract.Requires(states != null);

            foreach (State s in states) children[s.Name] = s;
            return this;
        }

        internal IEnumerable<State> Children => children.Values;

        internal State initialState;

        private readonly Dictionary<string, State> children = new Dictionary<string, State>();
    }

    /// <summary>Base class for concrete states.</summary>
    public class ConcreteState: State
    {
        /// <summary>Initializes a new instance of the <see cref="ConcreteState"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="doActivity">The do activity.</param>
        /// <param name="exit">The exit.</param>
        /// <param name="deferrableTriggers">The deferrable triggers.</param>
        internal ConcreteState(string name, Effect entry, Func<Task> doActivity, Effect exit,
                               Trigger[] deferrableTriggers): base(name)
        {
            ContractRequiredName(name);

            Entry = entry;
            DoActivity = doActivity;
            Exit = exit;

            //TODO DeferrableTriggers = deferrableTriggers;

            if (deferrableTriggers != null) throw new NotImplementedException("Deferrable triggers not implemented yet"); // TODO implement
        }

        internal Func<Task> DoActivity { get; }
        internal Effect Entry { get; }
        internal Effect Exit { get; }

        //TODO internal IEnumerable<Trigger> DeferrableTriggers { get; private set; }
    }

    /// <summary>UML state machine "region" state.</summary>
    public sealed class OrthogonalState : ConcreteState

    {
        /// <summary>Initializes a new instance of the <see cref="OrthogonalState"/> class.</summary>
        /// <param name="name">State name.</param>
        /// <param name="entry">Entry action.</param>
        /// <param name="doActivity">Do activity.</param>
        /// <param name="exit">Exit action.</param>
        /// <param name="deferrable">Deferrable triggers.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "doActivity")]
        public OrthogonalState(string name, Effect entry = null, Func<Task> doActivity = null, Effect exit = null,
                               Trigger[] deferrable = null) : base(name, entry, doActivity, exit, deferrable) => ContractRequiredName(name);

        /// <summary>Adds the specified outgoing transitions.</summary>
        /// <param name="transitions">Transitions originating from this state.</param>
        /// <returns>This state instance</returns>
        new public OrthogonalState Add(params TransitionBase[] transitions)
        {
            Contract.Requires(transitions != null);

            base.Add(transitions);
            return this;
        }

        /// <summary>Adds the specified sub states.</summary>
        /// <param name="states">Sub states of this state instance.</param>
        /// <returns>This state instance</returns>
        public OrthogonalState Add(params CompositeState[] states)
        {
            Contract.Requires(states != null);

            foreach (CompositeState s in states)
                children[s.Name] = s;
            return this;
        }

        internal IEnumerable<CompositeState> Children => children.Values;

        private readonly Dictionary<string, CompositeState> children = new Dictionary<string, CompositeState>();
    }

    /// <summary>UML state machine "simple" state.</summary>
    public sealed class SimpleState: ConcreteState
    {
        /// <summary>Initializes a new instance of the <see cref="SimpleState"/> class.</summary>
        /// <param name="name">State name.</param>
        /// <param name="entry">Entry action.</param>
        /// <param name="doActivity">Do activity.</param>
        /// <param name="exit">Exit action.</param>
        /// <param name="deferrable">Deferrable triggers.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "doActivity")]
        public SimpleState(string name, Effect entry = null, Func<Task> doActivity = null, Effect exit = null,
                           Trigger[] deferrable = null) : base(name, entry, doActivity, exit, deferrable) => ContractRequiredName(name);
    }

    /// <summary>Base class for UML state machine states.</summary>
    public class State //TODO : IState
    {
        /// <summary>Initializes a new instance of the <see cref="State"/> class.</summary>
        /// <param name="name">State name.</param>
        internal State(string name)
        {
            ContractRequiredName(name);

            Name = name;
        }

        /// <summary>Adds the specified outgoing transitions.</summary>
        /// <param name="transitions">Transitions originating from this state.</param>
        /// <returns>This state instance.</returns>
        public State Add(params TransitionBase[] transitions)
        {
            Contract.Requires(transitions != null);

            foreach (TransitionBase t in transitions) this.transitions[t.Name] = t;
            return this;
        }

        /// <inheritdoc/>
        public override string ToString() => LogFormat.FormatObject(GetType(), FullName);

        /// <summary>Code contract for validating state names.</summary>
        /// <param name="name">The name.</param>
        protected static void ContractRequiredName(string name) => Contract.Requires(!string.IsNullOrEmpty(name) && !name.Contains(" ") && (name == "." || !name.Contains(".")));

        /// <summary>Gets the full name of a state.</summary>
        public string FullName { get; internal set; }

        internal string Name { get; }

        internal State ParentState { get; set; }

        internal ICollection<TransitionBase> Transitions => transitions.Values;

        internal State[] rootPath;

        private readonly Dictionary<string, TransitionBase> transitions = new Dictionary<string, TransitionBase>();

        internal bool doActivityStarted;

        //TODO internal TransitionBase[]            completionTransitions;
        //TODO internal TransitionBase[]            timeTriggerTransitions;
        //TODO internal Dictionary<CallTriggerBase, TransitionBase[]> callTriggerTransitions = new Dictionary<CallTriggerBase, TransitionBase[]>();
    }
}
