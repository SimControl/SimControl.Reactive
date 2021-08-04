// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.Reactive
{
    /// <summary>UML state machine "choice" pseudo state.</summary>
    public sealed class ChoiceState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="ChoiceState"/> class.</summary>
        /// <param name="name">The name.</param>
        public ChoiceState(string name) : base(name) => ContractRequiredName(name);
    }

    /// <summary>UML state machine "deep history" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class DeepHistoryState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="DeepHistoryState"/> class.</summary>
        /// <param name="name">The name.</param>
        public DeepHistoryState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Deep history state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine "entry point" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class EntryPointState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="EntryPointState"/> class.</summary>
        /// <param name="name">The name.</param>
        public EntryPointState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Entry pint state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine "exit point" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class ExitPointState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="ExitPointState"/> class.</summary>
        /// <param name="name">The name.</param>
        public ExitPointState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Exit point state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine "final" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class FinalState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="FinalState"/> class.</summary>
        /// <param name="name">The name.</param>
        public FinalState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Final state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine "fork" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class ForkState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="ForkState"/> class.</summary>
        /// <param name="name">The name.</param>
        public ForkState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Fork state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine "initial" pseudo state.</summary>
    public sealed class InitialState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="InitialState"/> class.</summary>
        /// <param name="name">The name.</param>
        public InitialState(string name) : base(name) { }
    }

    /// <summary>UML state machine "join" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class JoinState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="JoinState"/> class.</summary>
        /// <param name="name">The name.</param>
        public JoinState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Join state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine "junction" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class JunctionState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="JunctionState"/> class.</summary>
        /// <param name="name">The name.</param>
        public JunctionState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Junction state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine pseudo states base class.</summary>
    public class PseudoState: State
    {
        /// <summary>Initializes a new instance of the <see cref="PseudoState"/> class.</summary>
        /// <param name="name">State name.</param>
        protected PseudoState(string name) : base(name) => ContractRequiredName(name);
    }

    /// <summary>UML state machine "shallow history" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class ShallowHistoryState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="ShallowHistoryState"/> class.</summary>
        /// <param name="name">The name.</param>
        public ShallowHistoryState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Shallow history state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine "submachine" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class SubmachineState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="SubmachineState"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="sm">The sm.</param>
        public SubmachineState(string name, StateMachine sm) : base(name + sm.Name)
        {
            // Contract.Requires(!string.IsNullOrEmpty(name));
            // Contract.Requires(sm != null);

            throw new NotImplementedException("Submachine state not implemented yet"); // TODO implement
        }
    }

    /// <summary>UML state machine "terminate" pseudo state.</summary>
    [Obsolete("Not implemented yet")]
    public sealed class TerminateState: PseudoState
    {
        /// <summary>Initializes a new instance of the <see cref="TerminateState"/> class.</summary>
        /// <param name="name">The name.</param>
        public TerminateState(string name) : base(name)
        {
            ContractRequiredName(name);
            throw new NotImplementedException("Terminate state not implemented yet"); // TODO implement
        }
    }
}
