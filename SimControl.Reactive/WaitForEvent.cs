// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

#if EXPERIMENTAL
using System;
using System.Threading;

namespace SimControl.Reactive
{
    public class EventWait: IDisposable
    {
        public void EventHandler(object o)
        {
            asyncEvent.Set();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Wait()
        {
            asyncEvent.WaitOne();
        }

        public void Wait(TimeSpan timeSpan)
        {
            if (!asyncEvent.WaitOne(timeSpan, false)) throw new TimeoutException();
        }

        public void Reset() { asyncEvent.Reset(); }

        private void Dispose(bool disposing)
        {
            if (disposing)
                asyncEvent.Close();
        }

        private AutoResetEvent asyncEvent = new AutoResetEvent(false);
    }

    public class EventWait<T>: IDisposable where T: EventArgs
    {
        public void EventHandler(object o, T eventArgs)
        {
            this.eventArgs = eventArgs;
            asyncEvent.Set();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T Wait()
        {
            asyncEvent.WaitOne();

            return eventArgs;
        }

        public T Wait(TimeSpan timeSpan)
        {
            if (!asyncEvent.WaitOne(timeSpan, false)) throw new TimeoutException();

            return eventArgs;
        }

        public void Reset() { asyncEvent.Reset(); }

        private void Dispose(bool disposing)
        {
            if (disposing)
                asyncEvent.Close();
        }

        private T eventArgs;

        private AutoResetEvent asyncEvent = new AutoResetEvent(false);
    }
}
#endif
