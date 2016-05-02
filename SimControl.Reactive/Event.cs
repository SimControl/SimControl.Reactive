// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO: CR

namespace SimControl.Reactive
{
    internal class StateMachineEvent
    {
        internal StateMachineEvent(Trigger trigger, object[] args = null)
        {
            Trigger = trigger;
            Args = args;
        }

        internal readonly object[] Args;

        internal readonly Trigger Trigger;

        //public override string ToString() { return Log.Object(typeof(ActionEvent), Effect.Target, Effect.Target.GetType().FullName + "." + Effect.Method.Name, DueDate.ToString() + "." + DueDate.Millisecond); }

        //internal ActiveObject Target { get { return (ActiveObject) Effect.Target; } }
    }

    /*
        public class ActionEvent
        {
            internal ActionEvent(Delegate eventHandler, object[] args)
            {
                Effect = eventHandler;
                Args = args;
            }

            internal ActionEvent(Delegate eventHandler, object[] args, TimeSpan delay)
            {
                Effect = eventHandler;
                Args = args;
                DueDate = DateTime.Now + delay;
            }

            //public override string ToString() { return Log.Object(typeof(ActionEvent), Effect.Target, Effect.Target.GetType().FullName + "." + Effect.Method.Name, DueDate.ToString() + "." + DueDate.Millisecond); }

            //internal ActiveObject Target { get { return (ActiveObject) Effect.Target; } }

            internal readonly Delegate Effect;
            internal readonly object[] Args;
            internal readonly DateTime DueDate;
        }
     */
}
