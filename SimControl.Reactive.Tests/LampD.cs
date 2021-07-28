// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NLog;
using SimControl.Log;

namespace SimControl.Reactive.Tests
{
    // TODO [Log]
    public class Lamp2: IDisposable
    {
        public Lamp2()
        {
            sm.Add(
                new InitialState("*")
                    .Add(new Transition("Ready", effect: () => logger.Message(LogLevel.Debug, ".* -> .Ready"))),
                new CompositeState("Ready").Add(
                    new InitialState("*")
                        .Add(new Transition("Off", effect: () => logger.Message(LogLevel.Debug, ".Ready.* -> .Ready.Off"))),
                    new SimpleState("Off",
                        entry: () => logger.Message(LogLevel.Debug, ".Ready.Off - entry"),
                        exit: () => logger.Message(LogLevel.Debug, ".Ready.Off - exit"))
                        .Add(new Transition("On",
                            new CallTrigger(On), effect: () => {
                                logger.Message(LogLevel.Debug, ".Ready.Off -> .Ready.On");
                                Counter++;
                            })),
                    new SimpleState("On",
                        entry: () => logger.Message(LogLevel.Debug, ".Ready.On - entry"),
                        exit: () => logger.Message(LogLevel.Debug, ".Ready.On - exit"))
                        .Add(new Transition("Off", new CallTrigger(Off), effect: () => logger.Message(LogLevel.Debug, ".Ready.On -> .Ready.Off")))
                )
                .Add(new Transition("Faulted", new CallTrigger(On), effect: () => logger.Message(LogLevel.Debug, ".Ready -> .Faulted"))),
                new SimpleState("Faulted", () => logger.Message(LogLevel.Debug, ".Faulted - entry"))
            );

            sm.Initialize();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Fault(string message) => sm.TriggerCallEvent(new CallTrigger<string>(Fault), message);

        public void Off() => sm.TriggerCallEvent(new CallTrigger(Off));

        public void On() => sm.TriggerCallEvent(new CallTrigger(On));

        public override string ToString() => LogFormat.FormatObject(typeof(Lamp), sm.ActiveStates, Counter);

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
}
