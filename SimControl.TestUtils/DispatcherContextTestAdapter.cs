// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Nito.AsyncEx;
using NLog;
using SimControl.Log;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for creating a thread with a <see cref="Dispatcher"/> and a <see cref="DispatcherSynchronizationContext"/>.</summary>
    public class DispatcherContextTestAdapter : TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="DispatcherContextTestAdapter"/> class.</summary>
        /// <param name="testFrame">The test frame.</param>
        /// <param name="threadName">Name of the thread.</param>
        /// <param name="apartmentState">State of the apartment.</param>
        /// <exception cref="ArgumentException">threadName must not be null</exception>
        [Log]
        public DispatcherContextTestAdapter(TestFrame testFrame, string threadName, ApartmentState apartmentState = ApartmentState.MTA)
        {
            if (string.IsNullOrEmpty(threadName))
                throw new ArgumentException("threadName must not be null or empty");
            //TODO Contract fails with NullReferenceException
            //Contract.Requires(testFrame != null);
            //Contract.Requires(!string.IsNullOrEmpty(threadName));

            this.testFrame = testFrame ?? throw new ArgumentException("testFrame must not be null");

            var tcs = new TaskCompletionSource();

            var thread = new Thread(() =>
            {
                Thread.CurrentThread.Name = threadName;

                Dispatcher = Dispatcher.CurrentDispatcher;
                SynchronizationContext = new DispatcherSynchronizationContext(Dispatcher);
                Thread = Thread.CurrentThread;
                SynchronizationContext.SetSynchronizationContext(SynchronizationContext);
                Dispatcher.UnhandledException += testFrame.TestDispatcherContextUnhandledException;

                tcs.SetResult();
                Dispatcher.Run();
            });

            thread.SetApartmentState(apartmentState);
            thread.Start();

            tcs.Task.WaitAssertTimeout();
        }

        /// <summary>Post this message while asserting the test timeout.</summary>
        /// <param name="action">The action.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void PostAssertTimeout(Action action)
        {
            Contract.Requires(action != null);

            PostAssertTimeout(action, TestFrame.DefaultTestTimeout);
        }

        /// <summary>Post this message while asserting the test timeout.</summary>
        /// <param name="action">The action.</param>
        /// <param name="timeout">The timeout.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log(LogLevel = LogAttributeLevel.Off)]
        public void PostAssertTimeout(Action action, int timeout)
        {
            Contract.Requires(action != null);

            var tcs = new TaskCompletionSource();

            SynchronizationContext.Post(o =>
            {
                try
                {
                    action();

                    tcs.SetResult();
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }, null);

            tcs.Task.WaitAssertTimeout(timeout);
        }

        /// <summary>Post this message while asserting the test timeout.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <returns>A T.</returns>
        /// <tparam name="T">Generic type parameter.</tparam>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public T PostAssertTimeout<T>(Func<T> func)
        {
            Contract.Requires(func != null);

            return PostAssertTimeout(func, TestFrame.DefaultTestTimeout);
        }

        /// <summary>Post this message while asserting the test timeout.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A T.</returns>
        /// <tparam name="T">Generic type parameter.</tparam>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log(LogLevel = LogAttributeLevel.Off)]
        public T PostAssertTimeout<T>(Func<T> func, int timeout)
        {
            Contract.Requires(func != null);

            var tcs = new TaskCompletionSource<T>();

            SynchronizationContext.Post(o =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }, null);

            return tcs.Task.ResultAssertTimeout(timeout);
        }

        /// <summary>Post this message.</summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log(LogLevel = LogAttributeLevel.Off)]
        public Task PostAsync(Action action)
        {
            Contract.Requires(action != null);

            var tcs = new TaskCompletionSource();

            SynchronizationContext.Post(o =>
            {
                try
                {
                    action();
                    tcs.SetResult();
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }, null);

            return tcs.Task;
        }

        /// <summary>Post this message.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>A Task&lt;T&gt;</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log(LogLevel = LogAttributeLevel.Off)]
        public Task<T> PostAsync<T>(Func<T> func)
        {
            Contract.Requires(func != null);

            var tcs = new TaskCompletionSource<T>();

            SynchronizationContext.Post(o =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }, null);

            return tcs.Task;
        }

        /// <summary>Send this message while asserting the test timeout.</summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SendAssertTimeout(Action action)
        {
            Contract.Requires(action != null);

            SendAssertTimeout(action, TestFrame.DefaultTestTimeout);
        }

        /// <summary>Send this message while asserting the test timeout.</summary>
        /// <param name="action">The action.</param>
        /// <param name="timeout">The timeout.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log(LogLevel = LogAttributeLevel.Off)]
        public void SendAssertTimeout(Action action, int timeout)
        {
            Contract.Requires(action != null);

            var tcs = new TaskCompletionSource();

            SynchronizationContext.Send(o =>
            {
                try
                {
                    action();
                    tcs.SetResult();
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }, null);

            tcs.Task.WaitAssertTimeout(timeout);
        }

        /// <summary>Send this message.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>A Task&lt;T&gt;</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public T SendAssertTimeout<T>(Func<T> func)
        {
            Contract.Requires(func != null);

            return SendAssertTimeout(func, TestFrame.DefaultTestTimeout);
        }

        /// <summary>Send this message.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A Task&lt;T&gt;</returns>
        /// <tparam name="T">Generic type parameter.</tparam>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log(LogLevel = LogAttributeLevel.Off)]
        public T SendAssertTimeout<T>(Func<T> func, int timeout)
        {
            Contract.Requires(func != null);

            var tcs = new TaskCompletionSource<T>();

            SynchronizationContext.Send(o =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }, null);

            return tcs.Task.ResultAssertTimeout(timeout);
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Log]
        protected override void Dispose(bool disposing)
        {
            if (disposing && Dispatcher != null)
                try
                {
                    Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send);
                    Thread.JoinAssertTimeout();
                }
                finally
                {
                    Dispatcher.UnhandledException -= testFrame.TestDispatcherContextUnhandledException;

                    Dispatcher = null;
                    SynchronizationContext = null;
                    Thread = null;
                }
        }

        /// <summary>Gets or sets the dispatcher.</summary>
        /// <value>The dispatcher.</value>
        public Dispatcher Dispatcher
        { get; private set; }

        /// <summary>Gets or sets a context for the synchronization.</summary>
        /// <value>The synchronization context.</value>
        public SynchronizationContext SynchronizationContext
        { get; private set; }

        /// <summary>Gets or sets the thread.</summary>
        /// <value>The thread.</value>
        public Thread Thread
        { get; private set; }

        private readonly TestFrame testFrame;
    }
}
