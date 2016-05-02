// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

#if EXPERIMENTAL
using System;
using System.Collections.Generic;
using System.Text;
using SimControl.Reactive;

namespace SimControl.Reactive.Tests
{
    public class Counter: ActiveObjectNotifyProperties
    {
        public Counter(string name, int count): base(name) { this.count = count; }

        public void Start() { RegisterEvent(new Effect(StartAction)); }
        public void Stop() { RegisterEvent(new Effect(StopAction)); }
        public void Increment() { RegisterEvent(() => SetValueAction(count + 1)); }

        public int Value
        {
            get { AssertIsValid(); return count; }
            set { RegisterEvent<int>(new Effect<int>(SetValueAction), value); }
        }

        public bool State { get { AssertIsValid(); return nextEvent != null; } }

        private void StartAction()
        {
            if (nextEvent == null)
            {
                IncrementPeriodically();
                OnPropertyChanged("State");
            }
        }

        private void StopAction()
        {
            if (nextEvent != null)
            {
                UnregisterEvent(nextEvent);
                nextEvent = null;
                OnPropertyChanged("State");
            }
        }

        private void IncrementAction() { SetValueAction(count + 1); }

        private void IncrementPeriodically()
        {
            IncrementAction();
            nextEvent = RegisterEvent(new TimeSpan(0, 0, 1), new Effect(IncrementPeriodically));
        }

        private void SetValueAction(int count)
        {
            this.count = count;
            OnPropertyChanged("Value");
        }

        public override string ToString() { return Log.Object(typeof(Counter), base.ToString(), count); }

        private int count;
        private ActionEvent nextEvent;
    }

    public class Sample3
    {
        public String Property
        {
            get { lock( locker ) return property; }

            //[AsyncCall]
            set { lock( locker ) property = value; }
        }

        //[Locked]
        public String Property2
        {
            get;

            //[AsyncCall]
            set;
        }

        //[Locked]
        public String Property3
        {
            get;
            protected set;
        }

        protected Object locker;
        string property;
    }
}

#endif
