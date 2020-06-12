// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Threading;

//namespace SimControl.Rective
//{
//    /// <summary>Encapsulates a Windows (WPF) DispatcherSynchronizationContext.</summary>
//    public sealed class TestDispatcherContext: IDisposable
//    {
//        private TestDispatcherContext()
//        {
//            Dispatcher = Dispatcher.CurrentDispatcher;
//            SynchronizationContext = new DispatcherSynchronizationContext(Dispatcher);
//            Thread = Thread.CurrentThread;
//            SynchronizationContext.SetSynchronizationContext(SynchronizationContext);
//            Dispatcher.UnhandledException += this.testFrame.TestDispatcherContextUnhandledException;
//        }

//        /// <summary>Finalizer.</summary>
//        ~TestDispatcherContext()
//        {
//            Dispose(false);
//        }

//        /// <summary>Creates a windows dispatcher context.</summary>
//        /// <param name="testFrame">The test frame.</param>
//        /// <param name="apartmentState">(Optional) state of the apartment.</param>
//        /// <returns>The new windows dispatcher context.</returns>
//        public static TestDispatcherContext Create(ApartmentState apartmentState = ApartmentState.MTA)
//        {
//            var tcs = new TaskCompletionSource<TestDispatcherContext>();

//            var thread = new Thread(() =>
//            {
//                tcs.SetResult(new TestDispatcherContext(testFrame));
//                Dispatcher.Run();
//            });

//            thread.SetApartmentState(apartmentState);
//            thread.Start();

//            TestDispatcherContext context = tcs.Task.ResultAssertTimeout();

//            return context;
//        }

//        /// <inheritdoc/>
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        /// <summary>Post this message.</summary>
//        /// <param name="action">The action.</param>
//        /// <returns>A Task.</returns>
//        public Task Post(Action action)
//        {
//            var tcs = new TaskCompletionSource<object>();

//            SynchronizationContext.Post(o =>
//            {
//                try
//                {
//                    action();
//                    tcs.SetResult(null);
//                }
//                catch (Exception e)
//                {
//                    tcs.SetException(e);
//                }
//            }, null);

//            return tcs.Task;
//        }

//        /// <summary>Post this message.</summary>
//        /// <typeparam name="T">Generic type parameter.</typeparam>
//        /// <param name="func">The function.</param>
//        /// <returns>A Task&lt;T&gt;</returns>
//        public Task<T> Post<T>(Func<T> func)
//        {
//            var tcs = new TaskCompletionSource<T>();

//            SynchronizationContext.Post(o =>
//            {
//                try
//                {
//                    tcs.SetResult(func());
//                }
//                catch (Exception e)
//                {
//                    tcs.SetException(e);
//                }
//            }, null);

//            return tcs.Task;
//        }

//        /// <summary>Post this message.</summary>
//        /// <param name="action">The action.</param>
//        /// <returns>A Task.</returns>
//        public void PostAssertTimeout(Action action, TimeSpan timeout)
//        {
//            var tcs = new TaskCompletionSource<object>();

//            SynchronizationContext.Post(o =>
//            {
//                try
//                {
//                    action();
//                    tcs.SetResult(null);
//                }
//                catch (Exception e)
//                {
//                    tcs.SetException(e);
//                }
//            }, null);

//            TaskEx.WhenAny(tcs.Task, TaskEx.Delay(TestFrame.TestTimeout)).Wait();

//            if (tcs.Task.Exception != null)
//                throw tcs.Task.Exception;
//            if (!tcs.Task.IsCompleted)
//                throw new TimeoutException();
//        }

//        /// <summary>Post this message.</summary>
//        /// <typeparam name="T">Generic type parameter.</typeparam>
//        /// <param name="func">The function.</param>
//        /// <returns>A Task&lt;T&gt;</returns>
//        public T PostAssertTimeout<T>(Func<T> func)
//        {
//            var tcs = new TaskCompletionSource<T>();

//            SynchronizationContext.Post(o =>
//            {
//                try
//                {
//                    tcs.SetResult(func());
//                }
//                catch (Exception e)
//                {
//                    tcs.SetException(e);
//                }
//            }, null);

//            TaskEx.WhenAny(tcs.Task, TaskEx.Delay(TestFrame.TestTimeout)).Wait();

//            if (tcs.Task.Exception != null)
//                throw tcs.Task.Exception;
//            if (!tcs.Task.IsCompleted)
//                throw new TimeoutException();
//            return tcs.Task.Result;
//        }

//        /// <summary>Send this message.</summary>
//        /// <param name="action">The action.</param>
//        /// <returns>A Task.</returns>
//        public void SendAssertTimeout(Action action)
//        {
//            var tcs = new TaskCompletionSource<bool>();

//            SynchronizationContext.Send(o =>
//            {
//                try
//                {
//                    action();
//                    tcs.SetResult(true);
//                }
//                catch (Exception e)
//                {
//                    tcs.SetException(e);
//                }
//            }, null);

//            TaskEx.WhenAny(tcs.Task, TaskEx.Delay(TestFrame.TestTimeout)).Wait();

//            if (tcs.Task.Exception != null)
//                throw tcs.Task.Exception;
//            if (!tcs.Task.IsCompleted)
//                throw new TimeoutException();
//        }

//        /// <summary>Send this message.</summary>
//        /// <typeparam name="T">Generic type parameter.</typeparam>
//        /// <param name="func">The function.</param>
//        /// <returns>A Task&lt;T&gt;</returns>
//        public T SendAssertTimeout<T>(Func<T> func)
//        {
//            var tcs = new TaskCompletionSource<T>();

//            SynchronizationContext.Send(o =>
//            {
//                try
//                {
//                    tcs.SetResult(func());
//                }
//                catch (Exception e)
//                {
//                    tcs.SetException(e);
//                }
//            }, null);

//            TaskEx.WhenAny(tcs.Task, TaskEx.Delay(TestFrame.TestTimeout)).Wait();

//            if (!ret)
//                throw new TimeoutException("Test timeout");

//            if (tcs.Task.Exception != null)
//                throw tcs.Task.Exception;
//            if (!tcs.Task.IsCompleted)
//                throw new TimeoutException();
//            return tcs.Task.Result;
//        }

//        private void Dispose(bool disposing)
//        {
//            if (disposing && Dispatcher != null)
//            {
//                new Action(Dispatcher.InvokeShutdown).RunAssertTimeout();
//                Thread.JoinAssertTimeout();
//                Dispatcher.UnhandledException -= testFrame.TestDispatcherContextUnhandledException;

//                Dispatcher = null;
//                SynchronizationContext = null;
//                testFrame = null;
//                Thread = null;
//            }
//        }

//        /// <summary>Gets or sets the dispatcher.</summary>
//        /// <value>The dispatcher.</value>
//        public Dispatcher Dispatcher { get; private set; }

//        /// <summary>Gets or sets a context for the synchronization.</summary>
//        /// <value>The synchronization context.</value>
//        public SynchronizationContext SynchronizationContext { get; private set; }

//        /// <summary>Gets or sets the thread.</summary>
//        /// <value>The thread.</value>
//        public Thread Thread { get; private set; }

//        private TestFrame testFrame;
//    }
//}
