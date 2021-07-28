// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimControl.Reactive
{
    /// <summary>StateMachine execution states.</summary>
    public enum ExecutionStateValue
    {
        /// <summary>StateMachine has not yet been initialized.</summary>
        Uninitialized,

        /// <summary>StateMachine is valid.</summary>
        Valid,

        /// <summary>StateMachine is currently executing a trigger.</summary>
        Running,

        /// <summary>StateMachine execution has failed.</summary>
        Failed
    }

    internal delegate void TimerCallback(object state);

    /// <summary>Used to report executed transitions and state changes.</summary>
    public class Executed
    {
        /// <summary>States entered during transition execution.</summary>
        public IEnumerable<State> Entered { get; set; }

        /// <summary>States exited during transition execution.</summary>
        public IEnumerable<State> Exited { get; set; }

        /// <summary>Executed transition.</summary>
        public TransitionBase Transition { get; set; }
    }

    /// <summary>UML state machine.</summary>
    /// <remarks>
    /// For a detailed description of UML state machines see "OMG Unified Modeling Language TM (OMG UML), Superstructure
    /// 2.3.pdf".
    /// </remarks>
    public class StateMachine: CompositeState, IDisposable /*ActiveObjectCollection,*/
    {
        /// <summary>Initializes a new instance of the <see cref="StateMachine"/> class.</summary>
        /// <param name="entry">The entry.</param>
        /// <param name="doActivity">The do activity.</param>
        /// <param name="exit">The exit.</param>
        /// <param name="deferrable">The deferrable.</param>
        public StateMachine(Effect entry = null, Func<Task> doActivity = null, Effect exit = null,
                            Trigger[] deferrable = null) : base(".", entry, doActivity, exit, deferrable)
        {
            FullName = ".";
            rootPath = new State[] { this };
            ExecutionState = ExecutionStateValue.Uninitialized;

            timer = new Timer(o => synchronizationContext.Post(ob => Run(), null), null, Timeout.Infinite,
                Timeout.Infinite);
        }

        /// <summary>Finalizes an instance of the <see cref="StateMachine"/> class.</summary>
        ~StateMachine() => Dispose(false);

        /// <summary>Adds the specified outgoing transitions.</summary>
        /// <param name="transitions">Transitions originating from this state.</param>
        /// <returns>This state instance</returns>
        public new StateMachine Add(params TransitionBase[] transitions)
        {
            Contract.Requires(ExecutionState == ExecutionStateValue.Uninitialized);

            base.Add(transitions);
            return this;
        }

        /// <summary>Adds the specified sub states.</summary>
        /// <param name="states">Sub states of this state instance.</param>
        /// <returns>This state instance</returns>
        public new StateMachine Add(params State[] states)
        {
            Contract.Requires(ExecutionState == ExecutionStateValue.Uninitialized);

            base.Add(states);
            return this;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Initializes this instance. Must be called before invoking <see cref="TriggerCallEvent"/></summary>
        public void Initialize()
        {
            Contract.Requires(ExecutionState == ExecutionStateValue.Uninitialized);

            try
            {
                synchronizationContext = SynchronizationContext.Current;

                AddState(this, null, rootPath);

                foreach (TransitionBase t in allTransitions)
                {
                    if (!allStates.ContainsKey(t.Target))
                        throw new StateMachineException("Target not found " +
                                                        LogFormat.FormatArgs("Transition:", t.Name, "Target:", t.Target));

                    t.TargetState = allStates[t.Target];
                }

                if (initialState == null)
                    throw new StateMachineException("No " + nameof(InitialState) + " found " +
                                                    LogFormat.FormatArgs("CompositeState:", FullName));

                Active = initialState;
                ExecutionState = ExecutionStateValue.Valid;

                TriggerCompletionEvents();
            }
            catch (StateMachineException e)
            {
                RaiseFailed(e);
                throw;
            }
        }

        /// <summary>Determines whether the specified state is Active.</summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if the specified state is Active; otherwise, <c>false</c>.</returns>
        public bool IsActive(string state)
        {
            //TODO check statMachine execution state

            State s = allStates[state];

            return s == this || s.ParentState is OrthogonalState || ((CompositeState) s.ParentState).Active == s;
        }

        /// <inheritdoc/>
        public override string ToString() => LogFormat.FormatObject(typeof(StateMachine), ExecutionState, ActiveSimpleStates.Select(i => i.FullName).ToArray());

        /// <summary>Triggers the specified trigger event.</summary>
        /// <param name="call">The trigger trigger.</param>
        /// <param name="args">Arguments for the trigger trigger.</param>
        public void TriggerCallEvent(CallTriggerBase call, params object[] args)
        {
            Contract.Requires(call != null);
            Contract.Requires(args.Length == call.Method.Method.GetParameters().Length);
            Contract.Requires(ExecutionState != ExecutionStateValue.Uninitialized &&
                              ExecutionState != ExecutionStateValue.Failed);

            queuedEvents.Add(new StateMachineEvent(call, args));

            Run();
        }

        /// <summary>Triggers any pending completion events.</summary>
        public void TriggerCompletionEvents()
        {
            Contract.Requires(ExecutionState != ExecutionStateValue.Uninitialized &&
                              ExecutionState != ExecutionStateValue.Failed);

            Run();
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged
        /// resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        private void AddState(State s, State parent, State[] root)
        {
            if (s.ParentState != null)
                throw new StateMachineException("State has already been added " + s.FullName);

            if (parent != null)
            {
                s.FullName = (parent == this ? "." : parent.FullName + ".") + s.Name;
                s.rootPath = root.Concat(new[] { s }).ToArray();
            }

            s.ParentState = parent;

            allStates[s.FullName] = s;

            if (s is CompositeState cs)
            {
                cs.initialState = cs.Children.FirstOrDefault(n => n is InitialState);

                foreach (State c in cs.Children)
                    AddState(c, cs, cs.rootPath);
            }

            if (s is OrthogonalState os)
            {
                //TODO ? os.initialState = os.Children.FirstOrDefault(n => n is InitialState);

                foreach (State c in os.Children)
                    AddState(c, os, os.rootPath);
            }

            foreach (TransitionBase t in s.Transitions)
                AddTransition(t, s, parent);
        }

        private void AddTransition(TransitionBase t, State s, State parent)
        {
            t.SourceState = s;

            if (t.Kind == TransitionBase.TransitionKind.Internal)
                t.Target = s.FullName;
            else if (t.Target[0] != '.')
                t.Target = (parent == this ? "." : parent.FullName + ".") + t.Target;

            allTransitions.Add(t);
        }

        private void AppendActiveSimpleStates(State s, List<State> activeStates)
        {
            if (s != null)
            {
                if (s is SimpleState)
                    activeStates.Add(s);

                if (s is CompositeState state)
                    AppendActiveSimpleStates(state.Active, activeStates);
                else if (s is OrthogonalState orthogonalState)
                    foreach (CompositeState child in orthogonalState.Children)
                        AppendActiveSimpleStates(child, activeStates);
            }
        }

        private void AppendActiveStates(State s, List<State> activeStates)
        {
            if (s != null)
            {
                activeStates.Add(s);

                if (s is CompositeState state)
                    AppendActiveStates(state.Active, activeStates);
                else if (s is OrthogonalState state1)
                    foreach (CompositeState child in state1.Children)
                        AppendActiveStates(child, activeStates);
            }
        }

        private void ExecuteTransition(TransitionBase t, object[] args)
        {
            exited = new List<State>();
            entered = new List<State>();

            int lcai = Math.Min(t.SourceState.rootPath.Length - 2, t.TargetState.rootPath.Length - 2);

            if (t.Kind == TransitionBase.TransitionKind.External)
                while (lcai > 0 && t.SourceState.rootPath[lcai] != t.TargetState.rootPath[lcai])
                    lcai--;
            else
                lcai = t.SourceState.rootPath.Length - 1;

            State lca = t.SourceState.rootPath[lcai];

            if (t.Kind == TransitionBase.TransitionKind.External)
                InvokeStateExit(lca);
            InvokeTransitionEffect(t, args);
            if (t.Kind == TransitionBase.TransitionKind.External)
            {
                InvokeStateEntry(lca, t.TargetState);
                RaiseStateChanged();
            }

            TransitionExecuted?.Invoke(this, new EventArgs<Executed>(new Executed { Exited = exited, Transition = t, Entered = entered }));
        }

        private TransitionBase FindTriggeredTransition(State s, Trigger trigger, object[] args)
        {
            TransitionBase result;

            if (s is CompositeState compositeState &&
                (result = FindTriggeredTransition(compositeState.Active, trigger, args)) != null)
                return result;

            if (s is OrthogonalState orthogonalState)
                foreach (CompositeState child in orthogonalState.Children)
                    if ((result = FindTriggeredTransition(child, trigger, args)) != null)
                        return result;

            foreach (TransitionBase t in s.Transitions)
                if (t.Trigger?.Matches(trigger) == true)
                {
                    if (t.Guard == null)
                        return t;
                    try
                    {
                        if (t.Guard.DynamicInvoke(args).Equals(true))
                            return t;
                    }
                    catch (Exception e)
                    {
                        queuedEvents.Add(
                            new StateMachineEvent(
                                new ExceptionTrigger(
                                    new StateMachineException(
                                        "Exception while executing guard " +
                                        LogFormat.FormatArgs("State:", s.FullName, "Transition:", t.Name),
                                        e.InnerException)), new object[] { e.InnerException }));
                    }
                }

            return null;
        }

        private TransitionBase FindValidCompletionTransition(State s, DateTime now)
        {
            TransitionBase result;

            if (s is CompositeState state && (result = FindValidCompletionTransition(state.Active, now)) != null)
                return result;

            if (s is OrthogonalState state1)
                foreach (CompositeState child in state1.Children)
                    if ((result = FindValidCompletionTransition(child, now)) != null)
                        return result;

            if (!s.doActivityStarted)
                foreach (TransitionBase t in s.Transitions)
                    if (t.Trigger == null || t.Trigger is TimeTrigger trigger && trigger.Due <= now)
                    {
                        if (t.Guard == null)
                            return t;

                        try
                        {
                            if (t.Guard.DynamicInvoke().Equals(true))
                                return t;
                        }
                        catch (Exception e)
                        {
                            queuedEvents.Add(
                                new StateMachineEvent(
                                    new ExceptionTrigger(
                                        new StateMachineException(
                                            "Exception while executing guard " +
                                            LogFormat.FormatArgs("State:", s.FullName, "Transition:", t.Name),
                                            e.InnerException)), new object[] { e.InnerException }));
                        }
                    }

            return null;
        }

        private void InvokeStateEntry(State s, State target)
        {
            if (s == target || target != null && target.rootPath[s.rootPath.Length - 1] != s)
                target = null;

            if (s is CompositeState c)
            {
                c.Active = target == null ? c.initialState : target.rootPath[s.rootPath.Length];
                if (c.Active == null)
                    throw new StateMachineException("No " + nameof(InitialState) + " found " + LogFormat.FormatArgs(c));

                var ca = c.Active as ConcreteState;

                if (ca != null)
                    entered.Add(ca);

                if (ca?.Entry != null)
                    try
                    {
                        ca.Entry.Invoke();
                    }
                    catch (Exception e)
                    {
                        queuedEvents.Add(
                            new StateMachineEvent(
                                new ExceptionTrigger(
                                    new StateMachineException(
                                        "Exception while executing entry action " + LogFormat.FormatArgs(ca), e)),
                                new object[] { e }));
                    }

                if (ca?.DoActivity != null)
                {
                    ca.doActivityStarted = true;

                    Task doActivity = ca.DoActivity();

                    doActivity.ContinueWith(task => {
                        ca.doActivityStarted = false;

                        if (task.IsFaulted)
                        {
                            queuedEvents.Add(
                                new StateMachineEvent(
                                    new ExceptionTrigger(
                                        new StateMachineException(
                                            "Exception while doActivity " + LogFormat.FormatArgs(ca),
                                            task.Exception.InnerException)),
                                    new object[] { task.Exception.InnerException }));
                            Run();
                        }
                        else if (task.IsCanceled)
                        {
                            var ex = new TaskCanceledException();
                            queuedEvents.Add(
                                new StateMachineEvent(
                                    new ExceptionTrigger(
                                        new StateMachineException(
                                            "Exception while executing doActivity " + LogFormat.FormatArgs(ca),
                                            ex)),
                                    new object[] { ex }));
                            Run();
                        }
                        else
                            RunCompletionEvents(DateTime.Now);
                    });
                }

                foreach (TransitionBase t in c.Active.Transitions)
                {
                    if (t.Trigger is TimeTrigger trigger)
                        trigger.Next();
                }

                InvokeStateEntry(c.Active, target);
            }
            if (s is OrthogonalState o)
                foreach (CompositeState child in o.Children)
                {
                    entered.Add(child);

                    if (child.Entry != null)
                        try
                        {
                            child.Entry.Invoke();
                        }
                        catch (Exception e)
                        {
                            queuedEvents.Add(
                                new StateMachineEvent(
                                    new ExceptionTrigger(
                                        new StateMachineException(
                                            "Exception while executing entry action " + LogFormat.FormatArgs(child), e)),
                                    new object[] { e }));
                        }

                    foreach (TransitionBase t in child.Transitions)
                    {
                        if (t.Trigger is TimeTrigger trigger)
                            trigger.Next();
                    }

                    InvokeStateEntry(child, target);
                }
        }

        private void InvokeStateExit(State s)
        {
            if (s is CompositeState c)
            {
                InvokeStateExit(c.Active);

                var ca = c.Active as ConcreteState;

                if (ca != null)
                    exited.Add(ca);

                if (ca?.Exit != null)
                    try
                    {
                        ca.Exit.Invoke();
                    }
                    catch (Exception e)
                    {
                        queuedEvents.Add(
                            new StateMachineEvent(
                                new ExceptionTrigger(
                                    new StateMachineException(
                                        "Exception while executing exit action " + LogFormat.FormatArgs(ca), e)),
                                new object[] { e }));
                    }

                c.Active = null;
            }
            if (s is OrthogonalState o)
                foreach (CompositeState child in o.Children)
                {
                    InvokeStateExit(child);

                    exited.Add(child);

                    if (child.Exit != null)
                        try
                        {
                            child.Exit.Invoke();
                        }
                        catch (Exception e)
                        {
                            throw new StateMachineException(
                                "Exception while executing exit action " + LogFormat.FormatArgs(child), e);
                        }
                }
        }

        private void InvokeTransitionEffect(TransitionBase t, object[] args)
        {
            if (t.Effect != null)
                try
                {
                    t.Effect.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    queuedEvents.Add(
                        new StateMachineEvent(
                            new ExceptionTrigger(
                                new StateMachineException(
                                    "Exception while executing effect action " + LogFormat.FormatArgs(t),
                                    e.InnerException)), new object[] { e.InnerException }));
                }
        }

        private DateTime NextTimeTrigger(State s, DateTime now)
        {
            DateTime result = DateTime.MaxValue;

            if (s is CompositeState state)
                result = NextTimeTrigger(state.Active, now);
            else if (s is OrthogonalState orthogonalState)
                foreach (CompositeState child in orthogonalState.Children)
                {
                    DateTime subresult = NextTimeTrigger(child, now);
                    if (subresult < result)
                        result = subresult;
                }

            foreach (TransitionBase t in s.Transitions)
            {
                if (t.Trigger is TimeTrigger trigger&& trigger.Due > now && trigger.Due < result)
                    result = trigger.Due;
            }

            return result;
        }

        private void RaiseFailed(StateMachineException e)
        {
            ExecutionState = ExecutionStateValue.Failed;

            Failed?.Invoke(this, new EventArgs<StateMachineException>(e));
        }

        private void RaiseStateChanged() => StateChanged?.Invoke(this, EventArgs.Empty);

        private void Run()
        {
            try
            {
                if (ExecutionState == ExecutionStateValue.Running)
                    return;

                DateTime now = DateTime.Now;

                ExecutionState = ExecutionStateValue.Running;

                if (queuedEvents.Count == 0)
                    RunCompletionEvents(now);

                bool completeionTransitionFound;

                TransitionBase t = null;
                do
                {
                    if (queuedEvents.Count > 0)
                    {
                        StateMachineEvent ev = queuedEvents[0];
                        queuedEvents.RemoveAt(0);

                        if ((t = FindTriggeredTransition(this, ev.trigger, ev.args)) != null)
                            ExecuteTransition(t, ev.args);
                        else if (ev.trigger is ExceptionTrigger exceptionTrigger)
                            throw exceptionTrigger.exception;
                    }

                    completeionTransitionFound = false;
                    // TODO: handle queued exception transitions before any other transition
                    while ((t = FindValidCompletionTransition(this, now)) != null)
                    {
                        completeionTransitionFound = true;
                        ExecuteTransition(t, null);
                    }
                } while (queuedEvents.Count > 0 || completeionTransitionFound);

                DateTime next = NextTimeTrigger(this, now);

                if (next != now && next != DateTime.MaxValue)
                    timer.Change((next - now).Milliseconds, Timeout.Infinite);

                ExecutionState = ExecutionStateValue.Valid;
            }
            catch (StateMachineException e)
            {
                RaiseFailed(e);
                throw;
            }
        }

        private void RunCompletionEvents(DateTime now)
        {
            for (TransitionBase t = FindValidCompletionTransition(this, now); t != null;
                 t = FindValidCompletionTransition(this, now))
                ExecuteTransition(t, null);
        }

        /// <summary>Event queue for all listeners interested in Failed events.</summary>
        public event EventHandler<EventArgs<StateMachineException>> Failed;

        /// <summary>Occurs when [state changed].</summary>
        public event EventHandler StateChanged;

        /// <summary>Occurs when a transition has been executed.</summary>
        public event EventHandler<EventArgs<Executed>> TransitionExecuted;

        /// <summary>Gets the Active states.</summary>
        public ICollection<State> ActiveSimpleStates
        {
            get
            {
                //TODO check statMachine execution state

                Contract.Ensures(Contract.Result<ICollection<State>>() != null);

                // if (ExecutionState == ExecutionStateValue.Uninitialized || ExecutionState ==
                // ExecutionStateValue.Running) return new State[] { }; else
                var activeStates = new List<State>();
                AppendActiveSimpleStates(this, activeStates);
                return activeStates;
            }
        }

        /// <summary>Gets the Active states.</summary>
        public ICollection<State> ActiveStates
        {
            get
            {
                //TODO check statMachine executuon state

                Contract.Ensures(Contract.Result<ICollection<State>>() != null);

                // if (ExecutionState == ExecutionStateValue.Uninitialized || ExecutionState ==
                // ExecutionStateValue.Running) return new State[] { }; else
                var activeStates = new List<State>();
                AppendActiveStates(this, activeStates);
                return activeStates;
            }
        }

        /// <summary>Disable timeouts if a debugger is attached.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static int DebugTimeout(int timeout) => Debugger.IsAttached ? int.MaxValue : timeout;

        /// <summary>Gets the state machines execution state.</summary>
        /// <value>The execution state.</value>
        public ExecutionStateValue ExecutionState { get; private set; }

        /// <summary>Gets all states.</summary>
        /// <value>The states.</value>
        public ICollection<State> States
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<State>>() != null);

                return allStates.Values;
            }
        }

        /// <summary>The cancellation token.</summary>
        private readonly Dictionary<string, State> allStates = new Dictionary<string, State>();

        private readonly List<TransitionBase> allTransitions = new List<TransitionBase>();
        private readonly List<StateMachineEvent> queuedEvents = new List<StateMachineEvent>();
        private List<State> entered;
        private List<State> exited;
        private SynchronizationContext synchronizationContext;
        private Timer timer;
    }
}
