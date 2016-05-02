// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Reflection;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Reactive.Tests
{
    public class Lamp: IDisposable
    {
        public Lamp()
        {
            sm.Add(
                new InitialState("*").Add(new Transition("LampOff")),
                new SimpleState("LampOff",
                    entry: () => logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "LampOff.Entry", Counter),
                    exit: () => logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "LampOff.Exit", Counter))
                    .Add(new Transition("LampOn", new CallTrigger(On), effect: () => {
                        logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "LampOff-LampOn.Effect", Counter);
                        Counter++;
                    })),
                new SimpleState("LampOn",
                    entry: () => logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "LampOn.Entry", Counter),
                    exit: () => logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "LampOff.Exit", Counter))
                    .Add(new Transition("LampOff", new CallTrigger(Off),
                        effect:
                            () =>
                            logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "LampOn-LampOff.Effect",
                                Counter))));

            sm.Initialize();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsActive(string state) => sm.IsActive(state);

        public void Off()
        { sm.TriggerCallEvent(new CallTrigger(Off)); }

        public void On()
        { sm.TriggerCallEvent(new CallTrigger(On)); }

        public override string ToString() => LogFormat.FormatObject(typeof(Lamp), sm.ActiveStates);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && sm != null)
            {
                sm.Dispose();
                sm = null;
            }
        }

        public int Counter
        { get; private set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private StateMachine sm = new StateMachine();
    }

    [Log]
    [TestFixture]
    public class LampSample: TestFrame
    {
        [Test, Example]
        public void Lamp_OnOffTriggered_CounterIs1()
        {
            using (Lamp lamp = new Lamp())
            {
                RunAssertTimeout(lamp.On);
                RunAssertTimeout(lamp.Off);

                Assert.That(lamp.IsActive(".LampOff"));
                Assert.That(lamp.Counter, Is.EqualTo(1));
            }
        }
    }
}
