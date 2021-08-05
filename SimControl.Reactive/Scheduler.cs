// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO CR

#if EXPERIMENTAL
using System;
using System.Threading;
using System.Collections.Generic;
using SimControl.Reactive;

namespace SimControl.Reactive
{
    public class Scheduler: ActiveObjectCollection, IDisposable
    {
        public Scheduler(string name, params ActiveObject[] children) : base(name)
        {
            SetParent(null);
            AddAction(children);

            lock (staticLocker)
                SchedulerCount++;

            thread = new Thread(new ThreadStart(RunForever));
            thread.Name = Name;
            thread.Start();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public static int ExceptionCount { get; private set; }
        public static int SchedulerCount { get; private set; }

        private void Initialize()
        {
        }

        [LogAttribute(typeof(Scheduler))]
        private void RunForever()
        {
            try
            {
                while (IsValid)
                {
                    ActionEvent ev = null;
                    TimeSpan ts = TimeSpan.Zero;

                    lock (this)
                        if (events.Count > 0)
                        {
                            ts = events[0].DueDate - DateTime.Now;

                            if (ts <= TimeSpan.Zero)
                            {
                                ev = events[0];
                                events.RemoveAt(0);
                            }
                        }

                    if (ev != null)
                    {
                        try
                        {
                            if (logger.IsTraceEnabled) logger.Trace(Log.LaMessage(this, ev));

                            ev.Effect.DynamicInvoke(ev.Args);
                        }
                        catch (Exception e)
                        {
                            lock (staticLocker) ExceptionCount++;
                            logger.ErrorException(Log.LaException(this), e);
                        }
                    }
                    else if (ts == TimeSpan.Zero)
                        asyncEvent.WaitOne();
                    else
                    {
                        bool exitContext = false;
                        asyncEvent.WaitOne(ts, exitContext);
                    }
                }
            }
            catch (Exception e) { logger.ErrorException(Log.LaException(this), e); }

            thread = null;
        }

        protected internal override void DeleteAction()
        {
            lock (this) events.Clear();

            asyncEvent.Close();
            asyncEvent = null;

            base.DeleteAction();
            events = null;

            lock (staticLocker) SchedulerCount--;
        }

        internal ActionEvent AddEvent(ActiveObject o, TimeSpan delay, Delegate eventHandler, params object[] args)
        {
            ActionEvent ev = new ActionEvent(eventHandler, args, delay);
            AddEvent(o, ev);
            return ev;
        }

        private void AddEvent(ActiveObject o, ActionEvent ev)
        {
            lock (this)
            {
                if (logger.IsTraceEnabled) logger.Trace(Log.LaMessage(this, ev));

                AssertIsValid();
                o.AssertIsValid();

                int i = 0;

                for (; i < events.Count; i++)
                    if (events[i].DueDate > ev.DueDate)
                    {
                        events.Insert(i, ev);
                        break;
                    }

                if (i == events.Count) events.Add(ev);
            }
            asyncEvent.Set();
        }

        internal bool RemoveEvent(ActionEvent ev)
        {
            lock (this)
            {
                if (logger.IsTraceEnabled) logger.Trace(Log.LaMessage(this, ev));
                AssertIsValid();
                return events.Remove(ev);
            }
        }

        internal void DeleteActiveObject(ActiveObject o)
        {
            lock (this)
            {
                for (int i = 0; i < events.Count;)
                    if (events[i].Target == o)
                        events.RemoveAt(i);
                    else
                        i++;
            }
        }

        internal Thread Thread { get { return thread; } }

        private List<ActionEvent> events = new List<ActionEvent>();
        private Thread thread;
        private AutoResetEvent asyncEvent = new AutoResetEvent(false);

        private Object staticLocker = new Object();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
#endif
