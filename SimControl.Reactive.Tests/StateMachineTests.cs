// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

// TODO: change method names
#pragma warning disable CC0021 // Use nameof

namespace SimControl.Reactive.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    [Log]
    [TestFixture]
    public class StateMachineTests : TestFrame
    {
        #region Test

        [SetUp]
        public new void SetUp() => count = 0;

        [TearDown]
        public new void TearDown() => sm = null;

        #endregion

        /// <inheritdoc/>
        [Test]
        public void DoActivity()
        {
            using (var stateChanged = new BlockingCollection<int>())
            {
                int stateChangedCount = 0;

                using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
                {
                    sm.Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState1", name: "Transition1",
                            effect: () => Action(0))),
                        new SimpleState("SimpleState1", entry: () => Action(1), doActivity: async () => {
                            await TaskEx.Delay(100).ConfigureAwait(false);
                            Action(2);
                        }, exit: () => Action(3)).Add(new Transition("SimpleState2", name: "Transition2",
                            effect: () => Action(4))),
                        new SimpleState("SimpleState2", entry: () => Action(5), exit: () => Action(-1)));

                    sm.StateChanged += (sender, args) => stateChanged.Add(stateChangedCount++);

                    using (var context = new DispatcherContextTestAdapter(this, "TestDispatcherContext", ApartmentState.STA))
                    {
                        Assert.AreEqual(0, stateChanged.Count);
                        context.PostAssertTimeout(sm.Initialize);

                        Assert.AreEqual(0, stateChanged.TakeAssertTimeout());
                        Assert.AreEqual(1, stateChanged.TakeAssertTimeout());

                        Assert.IsTrue(sm.IsActive("."));
                        Assert.IsTrue(sm.IsActive(".SimpleState2"));

                        Assert.AreEqual(6, count);
                    }
                }
            }
        }

        [Test]
        public void DoActivity_Cancel()
        {
            using (var stateChanged = new BlockingCollection<int>())
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                int stateChangedCount = 0;

                using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
                {
                    sm.Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState1", name: "Transition1",
                            effect: () => Action(0))),
                        new SimpleState("SimpleState1", entry: () => Action(1), doActivity: async () => {
                            Action(2);
                            await TaskEx.Delay(100000, cancellationTokenSource.Token).ConfigureAwait(false);
                        }, exit: () => Action(3)).Add(new Transition<OperationCanceledException>("SimpleState2",
                            new ExceptionTrigger<OperationCanceledException>(), effect: e => Action(4))),
                        new SimpleState("SimpleState2", entry: () => Action(5), exit: () => Action(-1)));

                    sm.StateChanged += (sender, args) => stateChanged.Add(stateChangedCount++);

                    using (var context = new DispatcherContextTestAdapter(this, "TestDispatcherContext", ApartmentState.STA))
                    {
                        context.PostAssertTimeout(sm.Initialize);

                        Assert.AreEqual(0, stateChanged.TakeAssertTimeout());
                        TaskEx.Delay(50).WaitAssertTimeout(); //TODO why?
                        cancellationTokenSource.Cancel();
                        Assert.AreEqual(1, stateChanged.TakeAssertTimeout());
                        Assert.IsTrue(sm.IsActive("."));
                        Assert.IsTrue(sm.IsActive(".SimpleState2"));

                        Assert.AreEqual(6, count);
                    }
                }
            }
        }

        [Test]
        public void DoActivity_Exception()
        {
            using (var stateChanged = new BlockingCollection<int>())
            {
                int stateChangedCount = 0;

                using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
                {
                    sm.Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState1", name: "Transition1",
                            effect: () => Action(0))),
                        new SimpleState("SimpleState1", entry: () => Action(1), doActivity: async () => {
                            await TaskEx.Delay(100).ConfigureAwait(false);
                            Action(2);
                            throw new InvalidOperationException();
                        }, exit: () => Action(3)).Add(new Transition<InvalidOperationException>("SimpleState2",
                            new ExceptionTrigger<InvalidOperationException>(), effect: e => Action(4))),
                        new SimpleState("SimpleState2", entry: () => Action(5), exit: () => Action(-1)));

                    sm.StateChanged += (sender, args) => stateChanged.Add(stateChangedCount++);

                    using (var context = new DispatcherContextTestAdapter(this, "TestDispatcherContext", ApartmentState.STA))
                    {
                        context.PostAssertTimeout(sm.Initialize);

                        Assert.AreEqual(0, stateChanged.TakeAssertTimeout());
                        Assert.AreEqual(1, stateChanged.TakeAssertTimeout());
                        Assert.IsTrue(sm.IsActive("."));
                        Assert.IsTrue(sm.IsActive(".SimpleState2"));

                        Assert.AreEqual(6, count);
                    }
                }
            }
        }

        [Test]
        public void DoActivity_Exception2()
        {
            using (var stateChanged = new BlockingCollection<int>())
            {
                int stateChangedCount = 0;

                using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
                {
                    sm.Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState1", name: "Transition1",
                            effect: () => Action(0))),
                        new SimpleState("SimpleState1", entry: () => Action(1), doActivity: async () => {
                            await TaskEx.Delay(100).ConfigureAwait(false);
                            Action(2);
                            throw new InvalidOperationException();
                        }, exit: () => Action(3)),
                        new SimpleState("SimpleState2", entry: () => Action(5), exit: () => Action(-1)));

                    sm.StateChanged += (sender, args) => stateChanged.Add(stateChangedCount++);
                    sm.Failed += (o, args) => logger.Error(((StateMachineException) args).ToString());

                    using (var context = new DispatcherContextTestAdapter(this, "TestDispatcherContext", ApartmentState.STA))
                    {
                        context.PostAssertTimeout(sm.Initialize);

                        Assert.AreEqual(0, stateChanged.TakeAssertTimeout());
                        Assert.AreEqual(1, stateChanged.TakeAssertTimeout());
                        Assert.IsTrue(sm.IsActive("."));
                        Assert.IsTrue(sm.IsActive(".SimpleState2"));

                        Assert.AreEqual(6, count);
                    }
                }
            }
        }

        [Test]
        public void ExceptionTrigger__should__be_queued__when__more_than_exceptions_are_thrown_within_one_transition()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState.SimpleState1",
                        effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(7)).Add(
                        new SimpleState("SimpleState1", entry: () => Action(2), exit: () => Action(3)).Add(
                            new Transition<int>("SimpleState2", new CallTrigger<int>(Call), effect: i => {
                                Action(i);
                                throw new InvalidOperationException();
                            })), new SimpleState("SimpleState2", entry: () => Action(5), exit: () => {
                                Action(6);
                                throw new InvalidOperationException();
                            })).Add(new Transition<InvalidOperationException>("SimpleState3",
                                new ExceptionTrigger<InvalidOperationException>(), effect: e => Action(8))),
                    new SimpleState("SimpleState3", entry: () => Action(9), exit: () => Action(10)).Add(
                        new Transition<InvalidOperationException>("SimpleState4",
                            new ExceptionTrigger<InvalidOperationException>(), effect: e => {
                                Action(11);
                                Assert.AreEqual(typeof(InvalidOperationException), e.GetType());
                            })), new SimpleState("SimpleState4", entry: () => Action(12), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Call(4);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState4"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState1"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState2"));
                Assert.IsFalse(sm.IsActive(".SimpleState3"));

                Assert.AreEqual(13, count);
            }
        }

        [Test]
        public void ExceptionTrigger__should__be_raised__when__derived_exception_is_thrown_in_guard_expression()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState.SimpleState1",
                        effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(4)).Add(
                        new SimpleState("SimpleState1", entry: () => Action(2), exit: () => Action(3)).Add(
                            new Transition<int>("SimpleState1", new CallTrigger<int>(Call),
                                guard: i => throw new InvalidOperationException(), effect: Action)),
                        new SimpleState("SimpleState2", entry: () => Action(-1), exit: () => Action(-1))).Add(
                            new Transition<Exception>("SimpleState3", new ExceptionTrigger<Exception>(), effect: e =>
                            {
                                Action(5);
                                Assert.AreEqual(typeof(InvalidOperationException), e.GetType());
                            })), new SimpleState("SimpleState3", entry: () => Action(6), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Call(-1);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState3"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState1"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState2"));

                Assert.AreEqual(7, count);
            }
        }

        [Test]
        public void ExceptionTrigger__should__be_raised__when__exception_is_thrown_in_effect_action()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState.SimpleState1",
                        effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(7)).Add(
                        new SimpleState("SimpleState1", entry: () => Action(2), exit: () => Action(3)).Add(
                            new Transition<int>("SimpleState2", new CallTrigger<int>(Call), effect: i => {
                                Action(i);
                                throw new InvalidOperationException();
                            })), new SimpleState("SimpleState2", entry: () => Action(5), exit: () => Action(6))).Add(
                                new Transition<InvalidOperationException>("SimpleState3",
                                    new ExceptionTrigger<InvalidOperationException>(), effect: e => {
                                        Action(8);
                                        Assert.AreEqual(typeof(InvalidOperationException), e.GetType());
                                    })), new SimpleState("SimpleState3", entry: () => Action(9), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Call(4);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState3"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState1"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState2"));

                Assert.AreEqual(10, count);
            }
        }

        [Test]
        public void ExceptionTrigger__should__be_raised__when__exception_is_thrown_in_entry_action()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState.SimpleState1",
                        effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(7)).Add(
                        new SimpleState("SimpleState1", entry: () => Action(2), exit: () => Action(3)).Add(
                            new Transition<int>("SimpleState2", new CallTrigger<int>(Call), effect: Action)),
                        new SimpleState("SimpleState2", entry: () => {
                            Action(5);
                            throw new InvalidOperationException();
                        }, exit: () => Action(6))).Add(new Transition<InvalidOperationException>("SimpleState3",
                            new ExceptionTrigger<InvalidOperationException>(), effect: e =>
                            {
                                Action(8);
                                Assert.AreEqual(typeof(InvalidOperationException), e.GetType());
                            })), new SimpleState("SimpleState3", entry: () => Action(9), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Call(4);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState3"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState1"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState2"));

                Assert.AreEqual(10, count);
            }
        }

        [Test]
        public void ExceptionTrigger__should__be_raised__when__exception_is_thrown_in_exit_action()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState.SimpleState1",
                        effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(7)).Add(
                        new SimpleState("SimpleState1", entry: () => Action(2), exit: () => {
                            Action(3);
                            throw new InvalidOperationException();
                        }).Add(new Transition<int>("SimpleState2", new CallTrigger<int>(Call), effect: Action)),
                        new SimpleState("SimpleState2", entry: () => Action(5), exit: () => Action(6))).Add(
                            new Transition<InvalidOperationException>("SimpleState3",
                                new ExceptionTrigger<InvalidOperationException>(), effect: e => {
                                    Action(8);
                                    Assert.AreEqual(typeof(InvalidOperationException), e.GetType());
                                })), new SimpleState("SimpleState3", entry: () => Action(9), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Call(4);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState3"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState1"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState2"));

                Assert.AreEqual(10, count);
            }
        }

        [Test]
        public void ExceptionTrigger__should__be_raised__when__exception_is_thrown_in_guard_expression()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState.SimpleState1",
                        effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(4)).Add(
                        new SimpleState("SimpleState1", entry: () => Action(2), exit: () => Action(3)).Add(
                            new Transition<int>("SimpleState1", new CallTrigger<int>(Call),
                                guard: i => throw new InvalidOperationException(), effect: Action)),
                        new SimpleState("SimpleState2", entry: () => Action(-1), exit: () => Action(-1))).Add(
                            new Transition<InvalidOperationException>("SimpleState3",
                                new ExceptionTrigger<InvalidOperationException>(), effect: e => {
                                    Action(5);
                                    Assert.AreEqual(typeof(InvalidOperationException), e.GetType());
                                })), new SimpleState("SimpleState3", entry: () => Action(6), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Call(-1);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState3"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState1"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState2"));

                Assert.AreEqual(7, count);
            }
        }

        [Test]
        public void ExceptionTrigger__should__not_be_raised__when__exception_of_not_matching_type_is_thrown()
        {
            using (sm =
            new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState.SimpleState1",
                        effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(-1)).Add(
                        new SimpleState("SimpleState1", entry: () => Action(2), exit: () => Action(-1)).Add(
                            new Transition<int>("SimpleState1", new CallTrigger<int>(Call),
                                guard: i => throw new InvalidOperationException(), effect: Action)),
                        new SimpleState("SimpleState2", entry: () => Action(-1), exit: () => Action(-1))).Add(
                            new Transition<ArgumentException>("SimpleState3", new ExceptionTrigger<ArgumentException>(),
                                effect: e => Action(-1))),
                    new SimpleState("SimpleState3", entry: () => Action(-1), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Assert.Throws<StateMachineException>(() => Call(-1));
            }
        }

        [Test]
        public void StateMachineException_should_be_thrown_if_CompositeState_contains_no_initial_state()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState", effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(-1)));

                Assert.Throws<StateMachineException>(() => RunAssertTimeout(sm.Initialize));
            }
        }

        [Test]
        public void StateMachineException_should_be_thrown_if_guard_execution_fails()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState",
                        guard: () => throw new InvalidOperationException(), effect: () => Action(0))),
                    new SimpleState("SimpleState", entry: () => Action(1), exit: () => Action(-1)));

                Assert.Throws<StateMachineException>(() => RunAssertTimeout(sm.Initialize));
            }
        }

        [Test]
        public void StateMachineException_should_be_thrown_if_state_entry_action_execution_fails()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(0))),
                    new SimpleState("SimpleState", entry: () => throw new InvalidOperationException(),
                        exit: () => Action(-1)));

                Assert.Throws<StateMachineException>(() => RunAssertTimeout(sm.Initialize));
            }
        }

        [Test]
        public void StateMachineException_should_be_thrown_if_state_exit_action_execution_fails()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(0))),
                    new SimpleState("SimpleState1", entry: () => Action(1),
                        exit: () => throw new InvalidOperationException()).Add(new Transition("SimpleState2",
                            effect: () => Action(-1))),
                    new SimpleState("SimpleState2", entry: () => Action(-1), exit: () => Action(-1)));

                Assert.Throws<StateMachineException>(() => RunAssertTimeout(sm.Initialize));
            }
        }

        [Test]
        public void StateMachineException_should_be_thrown_if_StateMachine_contains_no_initial_state()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
                Assert.Throws<StateMachineException>(() => RunAssertTimeout(sm.Initialize));
        }

        [Test]
        public void StateMachineException_should_be_thrown_if_Transition_effect_execution_fails()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState",
                        effect: () => throw new InvalidOperationException())),
                    new SimpleState("SimpleState", entry: () => Action(-1), exit: () => Action(-1)));

                Assert.Throws<StateMachineException>(() => RunAssertTimeout(sm.Initialize));
            }
        }

        [Test]
        public void TestMethod01()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(0))),
                    new SimpleState("SimpleState", entry: () => Action(1), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));

                Assert.AreEqual(2, count);
            }
        }

        [Test]
        public void TestMethod02()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState", effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(-1)).Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(2))),
                        new SimpleState("SimpleState", entry: () => Action(3), exit: () => Action(-1))));

                RunAssertTimeout(sm.Initialize);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".CompositeState"));
                Assert.IsTrue(sm.IsActive(".CompositeState.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.InitialState"));

                Assert.AreEqual(4, count);
            }
        }

        [Test]
        public void TestMethod03()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState.SimpleState",
                        effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(-1)).Add(
                        new SimpleState("SimpleState", entry: () => Action(2), exit: () => Action(-1))));

                RunAssertTimeout(sm.Initialize);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".CompositeState"));
                Assert.IsTrue(sm.IsActive(".CompositeState.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));

                Assert.AreEqual(3, count);
            }
        }

        [Test]
        public void TestMethod04()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                State initialState =
                    new InitialState("InitialState").Add(new Transition("OrthogonalState", effect: () => Action(0)));
                var orthogonalState = new OrthogonalState("OrthogonalState", entry: () => Action(1), exit: () => Action(-1));

                var compositeState1 = new CompositeState("CompositeState1", entry: () => Action(2), exit: () => Action(-1));
                State initialState1 =
                    new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(4)));
                var simpleState1 = new SimpleState("SimpleState", entry: () => Action(5), exit: () => Action(-1));

                var compositeState2 = new CompositeState("CompositeState2", entry: () => Action(3), exit: () => Action(-1));
                State initialState2 =
                    new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(6)));
                var simpleState2 = new SimpleState("SimpleState", entry: () => Action(7), exit: () => Action(-1));

                sm.Add(initialState,
                    orthogonalState.Add(compositeState1.Add(initialState1, simpleState1),
                        compositeState2.Add(initialState2, simpleState2)));

                RunAssertTimeout(sm.Initialize);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".OrthogonalState"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState1"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState1.SimpleState"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState2"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState2.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".OrthogonalState.CompositeState1.InitialState"));
                Assert.IsFalse(sm.IsActive(".OrthogonalState.CompositeState2.InitialState"));

                ICollection<State> activeStates = sm.ActiveStates;

                var expectedActiveStates = new State[]
                                           {
                                           sm, orthogonalState, compositeState1, simpleState1, compositeState2,
                                           simpleState2
                                           };
                int i = 0;
                foreach (State s in activeStates)
                    Assert.AreSame(expectedActiveStates[i++], s);

                Assert.AreEqual(8, count);
            }
        }

        [Test]
        public void TestMethod05()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("OrthogonalState.CompositeState1.SimpleState",
                        effect: () => Action(0))),
                    new OrthogonalState("OrthogonalState", entry: () => Action(1), exit: () => Action(-1)).Add(
                        new CompositeState("CompositeState1", entry: () => Action(2), exit: () => Action(-1)).Add(
                            new SimpleState("SimpleState", entry: () => Action(3), exit: () => Action(-1))),
                        new CompositeState("CompositeState2", entry: () => Action(4), exit: () => Action(-1)).Add(
                            new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(5))),
                            new SimpleState("SimpleState", entry: () => Action(6), exit: () => Action(-1)))));

                RunAssertTimeout(sm.Initialize);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".OrthogonalState"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState1"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState1.SimpleState"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState2"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState2.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".OrthogonalState.CompositeState2.InitialState"));

                Assert.AreEqual(7, count);
            }
        }

        [Test]
        public void TestMethod06()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("OrthogonalState.CompositeState2.SimpleState",
                        effect: () => Action(0))),
                    new OrthogonalState("OrthogonalState", entry: () => Action(1), exit: () => Action(-1)).Add(
                        new CompositeState("CompositeState1", entry: () => Action(2), exit: () => Action(-1)).Add(
                            new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(5))),
                            new SimpleState("SimpleState", entry: () => Action(6), exit: () => Action(-1))),
                        new CompositeState("CompositeState2", entry: () => Action(3), exit: () => Action(-1)).Add(
                            new SimpleState("SimpleState", entry: () => Action(4), exit: () => Action(-1)))));

                RunAssertTimeout(sm.Initialize);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".OrthogonalState"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState1"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState1.SimpleState"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState2"));
                Assert.IsTrue(sm.IsActive(".OrthogonalState.CompositeState2.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".OrthogonalState.CompositeState1.InitialState"));

                Assert.AreEqual(7, count);
            }
        }

        [Test]
        public void TestMethod07()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState1", effect: () => Action(0))),
                    new SimpleState("SimpleState1", entry: () => Action(1), exit: () => Action(2)).Add(
                        new Transition<int>("SimpleState2", new CallTrigger<int>(Call), effect: Action)),
                    new SimpleState("SimpleState2", entry: () => Action(4), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Call(3);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState2"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".SimpleState1"));

                Assert.AreEqual(5, count);
            }
        }

        [Test]
        public void TestMethod08()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState1", effect: () => Action(0))),
                    new SimpleState("SimpleState1", entry: () => Action(1), exit: () => Action(2)).Add(
                        new Transition<int>("SimpleState2", new CallTrigger<int>(Call), name: "T1", effect: i => {
                            Action(i);
                            Call(6);
                        })),
                    new SimpleState("SimpleState2", entry: () => Action(4), exit: () => Action(5)).Add(
                        new Transition<int>("SimpleState3", new CallTrigger<int>(Call), name: "T2", effect: Action)),
                    new SimpleState("SimpleState3", entry: () => Action(7), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                IEnumerable<State> exited = null;
                TransitionBase t = null;
                IEnumerable<State> entered = null;

                sm.TransitionExecuted += (o, e) => {
                    Executed ex = e;
                    exited = ex.Exited;
                    t = ex.Transition;
                    entered = ex.Entered;
                };

                Call( 3);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState3"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".SimpleState1"));
                Assert.IsFalse(sm.IsActive(".SimpleState2"));

                Assert.AreEqual(8, count);

                Assert.That(exited.First().FullName, Is.EqualTo(".SimpleState2"));
                Assert.That(t.Name, Is.EqualTo( "T2" ));
                Assert.That(entered.First().FullName, Is.EqualTo(".SimpleState3") );
            }
        }

        [Test]
        public void TestMethod09()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState1", effect: () => Action(0))),
                    new SimpleState("SimpleState1", entry: () => Action(1), exit: () => Action(2)).Add(
                        new Transition<int>("SimpleState2", new CallTrigger<int>(Call), effect: Action)),
                    new SimpleState("SimpleState2", entry: () => Action(4), exit: () => Action(5)).Add(
                        new Transition<int>("SimpleState3", new CallTrigger<int>(Call), effect: Action)),
                    new SimpleState("SimpleState3", entry: () => Action(7), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Call(3);
                Call(6);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState3"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".SimpleState1"));
                Assert.IsFalse(sm.IsActive(".SimpleState2"));

                Assert.AreEqual(8, count);
            }
        }

        [Test]
        public void TestMethod10()
        {
            const int delay = 50;

            var sw = new Stopwatch();

            using (var stateChanged = new AutoResetEvent(false))
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState1", effect: () => Action(0))),
                    new SimpleState("SimpleState1", entry: () => {
                        Action(1);
                        sw.Start();
                    }, exit: () => Action(2)).Add(new Transition("SimpleState2",
                        new TimeSpanTrigger(() => TimeSpan.FromMilliseconds(delay)), effect: () => Action(3))),
                    new SimpleState("SimpleState2", entry: () => {
                        Action(4);
                        sw.Stop();
                    }, exit: () => Action(-1)));

                sm.StateChanged += (sender, args) => stateChanged.Set();

                using (var context = new DispatcherContextTestAdapter(this, "TestDispatcherContext", ApartmentState.STA))
                {
                    context.PostAssertTimeout(sm.Initialize);

                    stateChanged.WaitOneAssertTimeout();
                    stateChanged.WaitOneAssertTimeout();

                    Assert.IsTrue(sm.IsActive("."));
                    Assert.IsTrue(sm.IsActive(".SimpleState2"));

                    Assert.IsFalse(sm.IsActive(".InitialState"));
                    Assert.IsFalse(sm.IsActive(".SimpleState1"));

                    Assert.AreEqual(5, count);
                    logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "ElapsedMilliseconds",
                        sw.ElapsedMilliseconds);

                    Assert.IsTrue(delay <= sw.ElapsedMilliseconds && sw.ElapsedMilliseconds < delay*10);
                }
            }
        }

        [Test]
        public void TestMethod11()
        {
            int stateChangedCount = 0;

            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState", effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(-1)).Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(2))),
                        new SimpleState("SimpleState", entry: () => Action(3), exit: () => Action(-1)).Add(
                            new InternalTransition<int>(new CallTrigger<int>(Call), guard: i => i == 4, effect: Action)))
                       .Add(new InternalTransition<int>(new CallTrigger<int>(Call), guard: i => i == 5, effect: Action))
                            ).Add(new InternalTransition<int>(new CallTrigger<int>(Call), guard: i => i == 6, effect: Action));

                sm.StateChanged += (sender, args) => stateChangedCount++;

                RunAssertTimeout(sm.Initialize);
                Call(4);

                Assert.AreEqual(2, stateChangedCount);
                Call(5);
                Assert.AreEqual(2, stateChangedCount);
                Call(6);
                Assert.AreEqual(2, stateChangedCount);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".CompositeState"));
                Assert.IsTrue(sm.IsActive(".CompositeState.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.InitialState"));

                Assert.AreEqual(7, count);
            }
        }

        [Test]
        public void TestMethod12()
        {
            var s_3_6 = new Sequence(3, 6);

            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState", effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(-1)).Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(2))),
                        new SimpleState("SimpleState", entry: () => Action(s_3_6.Next), exit: () => Action(4)).Add(
                            new Transition<int>("SimpleState", new CallTrigger<int>(Call), effect: Action))));

                RunAssertTimeout(sm.Initialize);

                Call(5);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".CompositeState"));
                Assert.IsTrue(sm.IsActive(".CompositeState.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.InitialState"));

                Assert.AreEqual(7, count);
            }
        }

        [Test]
        public void TestMethod13()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState", effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(-1)).Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(2))),
                        new SimpleState("SimpleState", entry: () => Action(3), exit: () => Action(-1))).Add(
                            new InternalTransition<int>(new CallTrigger<int>(Call), effect: Action)));
                RunAssertTimeout(sm.Initialize);

                Call(4);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".CompositeState"));
                Assert.IsTrue(sm.IsActive(".CompositeState.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.InitialState"));

                Assert.AreEqual(5, count);
            }
        }

        [Test]
        public void TestMethod14()
        {
            var s_1_7 = new Sequence(1, 7);
            var s_2_8 = new Sequence(2, 8);
            var s_3_9 = new Sequence(3, 9);

            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState", effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(s_1_7.Next), exit: () => Action(5)).Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState",
                            effect: () => Action(s_2_8.Next))),
                        new SimpleState("SimpleState", entry: () => Action(s_3_9.Next), exit: () => Action(4))).Add(
                            new Transition<int>("CompositeState", new CallTrigger<int>(Call), effect: Action)));

                RunAssertTimeout(sm.Initialize);

                Call(6);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".CompositeState"));
                Assert.IsTrue(sm.IsActive(".CompositeState.SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.InitialState"));

                Assert.AreEqual(10, count);
            }
        }

        [Test]
        public void TestMethod15()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("CompositeState", effect: () => Action(0))),
                    new CompositeState("CompositeState", entry: () => Action(1), exit: () => Action(5)).Add(
                        new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(2))),
                        new SimpleState("SimpleState", entry: () => Action(3), exit: () => Action(4)).Add(
                            new Transition(".SimpleState", effect: () => Action(6)))),
                    new SimpleState("SimpleState", entry: () => Action(7), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.InitialState"));
                Assert.IsFalse(sm.IsActive(".CompositeState.SimpleState"));

                Assert.AreEqual(8, count);
            }
        }

        [Test]
        public void TestMethod16()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(0))),
                    new SimpleState("SimpleState", entry: () => Action(1), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                sm.TriggerCompletionEvents();

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));

                Assert.AreEqual(2, count);
            }
        }

        [Test]
        public void TestMethod17()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState", effect: () => Action(0))),
                    new SimpleState("SimpleState", entry: () => Action(1), exit: () => Action(-1))).Add(
                        new InternalTransition<int>(new CallTrigger<int>(Call), effect: Action));

                RunAssertTimeout(sm.Initialize);

                Call(2);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState"));

                Assert.IsFalse(sm.IsActive(".InitialState"));

                Assert.AreEqual(3, count);
            }
        }

        [Test]
        public void TestMethod18()
        {
            const int entryCount = 1;

            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState1", effect: () => Action(0))),
                    new SimpleState("SimpleState1", entry: () => Action(entryCount), exit: () => Action(-1)).Add(
                        new InternalTransition<int>(new CallTrigger<int>(Call), effect: Action)));

                sm.StateChanged += (o, args) => Action(2);

                RunAssertTimeout(sm.Initialize);

                Call(3);

                Assert.IsTrue(sm.IsActive("."));
                Assert.IsTrue(sm.IsActive(".SimpleState1"));

                Assert.IsFalse(sm.IsActive(".InitialState"));

                Assert.AreEqual(4, count);
            }
        }

        [Test]
        public void TestMethod19()
        {
            using (sm = new StateMachine(entry: () => Action(-1), exit: () => Action(-1)))
            {
                sm.Add(
                    new InitialState("InitialState").Add(new Transition("SimpleState1", effect: () => Action(0))),
                    new SimpleState("SimpleState1", entry: () => Action(1), exit: () => Action(2)).Add(
                        new Transition<int>("SimpleState2", new CallTrigger<int>(Call),
                            effect: i => throw new InvalidOperationException())),
                    new SimpleState("SimpleState2", entry: () => Action(4), exit: () => Action(-1)));

                RunAssertTimeout(sm.Initialize);

                Assert.Throws<StateMachineException>(() => Call(3));
            }
        }

        [Test, Unstable]
        public void DoActivity_Complex_42() //TODO fix
        {
            //TODO add Action(x)
            int iterations = 3;
            var history = new List<string>();

            using (sm = new StateMachine())
            {
                sm.Add(
                    new CompositeState("Run").Add(
                    new InitialState("Init")
                        .Add(new Transition("Start")),
                    new SimpleState("Start", doActivity: async () =>
                    {
                        await TaskEx.Delay(50).ConfigureAwait(false);
                        iterations--;
                        if (iterations > 0)
                            throw new InvalidOperationException();
                    })
                        .Add(new Transition("M"))
                        .Add(new Transition<Exception>(".Idle.Fail", new ExceptionTrigger<Exception>())),
                    new SimpleState("M"))
                );

                sm.Add(
                    new InitialState("Init2").Add(new Transition(".Run")),
                    new CompositeState("Idle").Add(
                        new SimpleState("Fail", doActivity: async () => await TaskEx.Delay(50).ConfigureAwait(false))
                            .Add(new Transition("Failed")),
                        new SimpleState("Failed")
                            .Add(new Transition(".Run", new TimeSpanTrigger(new TimeSpanExpression(() => TimeSpan.FromMilliseconds(50) ))))
                    ));

                sm.StateChanged += (sender, args) => {
                    foreach (State s in sm.ActiveStates.Where(l => l.FullName != "."))
                        history.Add(s.FullName);
                };

                using (var context = new DispatcherContextTestAdapter(this, "TestDispatcherContext", ApartmentState.STA))
                {
                    context.PostAssertTimeout(sm.Initialize);
                    TaskEx.Delay(1000).Wait();
                    Assert.AreEqual(".Run", history[0]);
                    Assert.AreEqual(".Run.Init", history[1]);
                    Assert.AreEqual(".Run", history[2]);
                    Assert.AreEqual(".Run.Start", history[3]);
                    Assert.AreEqual(".Idle", history[4]);
                    Assert.AreEqual(".Idle.Fail", history[5]);
                    Assert.AreEqual(".Idle", history[6]);
                    Assert.AreEqual(".Idle.Failed", history[7]);
                    Assert.IsTrue(history.Count > 8);
                }
            }
        }

        [Test]
        public void TestRecursiveStateDefinition()
        {
            using (sm = new StateMachine())
            {
                sm.Add(new InitialState("InitialState").Add(new Transition("SimpleState1")), new SimpleState("SimpleState1"));

                var compositeState = new CompositeState("CompositeState");
                sm.Add(compositeState);
                compositeState.Add(compositeState);

                Assert.Throws<StateMachineException>(() => RunAssertTimeout(sm.Initialize));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0026:Possible unassigned object created by 'new'", Justification = "<Pending>")]
        [Test]
        public static void Validate_Code_Contract_Exception()
        {
            try
            {
                Assert.IsNotNull(new SimpleState(""));
            }
            catch (Exception e)
            {
                AssertIsContractException(e);
            }
        }

        private void Action(int expected)
        {
            Assert.AreEqual(expected, count);
            count++;
        }

        private void Call(int i) => RunAssertTimeout(() => sm.TriggerCallEvent(new CallTrigger<int>(Call), i));

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private int count;
        private StateMachine sm;
    }

    internal class Sequence
    {
        public Sequence(params object[] args) => sequence = args;
        public int Next => (int) sequence[i++];

        private readonly object[] sequence;
        private int i;
    }
}

#pragma warning restore CC0021 // Use nameof
