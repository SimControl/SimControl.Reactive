// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SimControl.Log;

// UNDONE AssertTimeout extension methods

namespace SimControl.TestUtils
{
    /// <summary>Test extensions for asserting timeouts.</summary>
    public static class AssertTimeoutExtensions
    {
        ///// <summary>
        ///// Wrapper that calls <see cref="ClientBase{TChannel}.Abort()"/> or <see cref="ClientBase{TChannel}.Close()"/>
        ///// while asserting the test timeout.
        ///// </summary>
        ///// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        ///// <param name="client">The client to act on.</param>
        ///// <param name="timeout">The timeout.</param>
        //[Log( LogLevel = LogAttributeLevel.Off )]
        //public static void AbortOrCloseAssertTimeout(this ICommunicationObject client, int timeout = TestFrame.Timeout)
        //{
        //    if( client != null )
        //        if( client.State == CommunicationState.Opened )
        //            try
        //            {
        //                client.Close( TimeSpan.FromMilliseconds( timeout ) );
        //            }
        //            catch( Exception )
        //            {
        //                client.Abort();
        //                throw;
        //            }
        //        else if( client.State != CommunicationState.Closed )
        //            client.Abort();
        //}

        /// <summary><see cref="Task"/> wrapper that asserts the test timeout.</summary>
        /// <param name="task">The task to act on.</param>
        /// <returns>An asynchronous result.</returns>
        public static Task AssertTimeout(this Task task)
        {
            // UNDONE Contract.Requires(task != null);

            return task.AssertTimeout(TestFrame.Timeout);
        }

        /// <summary><see cref="Task"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <typeparam name="T">.</typeparam>
        /// <param name="task">The task to act on.</param>
        /// <returns>The task.</returns>
        public static Task<T> AssertTimeout<T>(this Task<T> task)
        {
            // UNDONE Contract.Requires(task != null);

            return task.AssertTimeout(TestFrame.Timeout);
        }

        /// <summary><see cref="Task"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="task">The task to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>An asynchronous result.</returns>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static Task AssertTimeout(this Task task, int timeout)
        {
            // UNDONE Contract.Requires(task != null);

            try
            {
                if (!task.Wait(TestFrame.DebugTimeout(timeout)))
                {
                    IgnoreFaults(task);
                    throw new AssertTimeoutException(timeout);
                }
            }
            catch (AggregateException e) { throw e.InnerException; }

            return task;
        }

        /// <summary><see cref="Task"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <exception cref="System.TimeoutException">Test timeout " + timeout.ToString( CultureInfo.InvariantCulture )
        /// + " expired.</exception>
        /// <typeparam name="T">.</typeparam>
        /// <param name="task">The task to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>An asynchronous result that yields a T.</returns>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static async Task<T> AssertTimeout<T>(this Task<T> task, int timeout)
        {
            // UNDONE Contract.Requires(task != null);

            return task == await Task.WhenAny(task, Task.Delay(TestFrame.DebugTimeout(timeout))).ConfigureAwait(false) ?
                await task.ConfigureAwait(false) : throw new AssertTimeoutException(timeout);
        }

        /// <summary>A Task extension method that causes the task continuation to ignore faults.</summary>
        /// <param name="task">The task to act on.</param>
        public static void IgnoreFaults(this Task task) => task.ContinueWith(t => { },
            CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);

        /// <summary><see cref="Thread.Join()"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="thread">The thread to act on.</param>
        /// <param name="timeout">The timeout.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static void JoinAssertTimeout(this Thread thread, int timeout = TestFrame.Timeout)
        {
            // UNDONE Contract.Requires(thread != null);

            if (!thread.Join(TestFrame.DebugTimeout(timeout)))
                throw new AssertTimeoutException(timeout);
        }

        /// <summary><see cref="Task{T}.Result"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="task">The task to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A T.</returns>
        //[Log(LogLevel = LogAttributeLevel.Off)]
        //public static T ResultAssertTimeout<T>(this Task<T> task, int timeout = TestFrame.Timeout)
        //{
        //    // UNDONE Contract.Requires(task != null);

        //    try
        //    {
        //        if (!task.Wait(TestFrame.DebugTimeout(timeout)))
        //        {
        //            IgnoreFaults(task);
        //            throw new AssertTimeoutException(timeout);
        //        }
        //    }
        //    catch (AggregateException e) { throw e.InnerException; }

        //    return task.Result;
        //}

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <param name="action">The action.</param>
        /// <param name="timeout">The timeout.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static Task RunAssertTimeout(this Action action, int timeout = TestFrame.Timeout)
        {
            // UNDONE Contract.Requires(action != null);

            Task task = Task.Run(action);

            try
            {
                if (!task.Wait(TestFrame.DebugTimeout(timeout)))
                {
                    IgnoreFaults(task);
                    throw new AssertTimeoutException(timeout);
                }
            }
            catch (AggregateException e) { throw e.InnerException; }

            return task;
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A T.</returns>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static T RunAssertTimeout<T>(this Func<T> function, int timeout = TestFrame.Timeout)
        {
            // UNDONE Contract.Requires(function != null);

            Task<T> task = Task.Run(function);

            try
            {
                if (!task.Wait(TestFrame.DebugTimeout(timeout)))
                {
                    IgnoreFaults(task);
                    throw new AssertTimeoutException(timeout);
                }
            }
            catch (AggregateException e) { throw e.InnerException; }

            return task.Result;
        }

        /// <summary><see cref="BlockingCollection{T}.Take()"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="blockingCollection">The asyncCollection to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A T.</returns>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static T TakeAssertTimeout<T>(
            this BlockingCollection<T> blockingCollection, int timeout = TestFrame.Timeout)
        {
            // UNDONE Contract.Requires(blockingCollection != null);

            return !blockingCollection.TryTake(out T result, TestFrame.DebugTimeout(timeout)) ?
                throw new AssertTimeoutException(timeout) : result;
        }

        [Log(LogLevel = LogAttributeLevel.Off)]
        public static IEnumerable<T> TakeUntilAssertTimeout<T>(
            this ChannelReader<T> asyncCollection, Func<T, bool> func, int timeout = TestFrame.Timeout)
        {
            var result = new List<T>();

            var timeoutCancel = new CancellationTokenSource(TestFrame.DebugTimeout(timeout));
            for (; ; )
                try
                {
                    T item = asyncCollection.ReadAsync(timeoutCancel.Token).AsTask().Result;

                    result.Add(item);

                    if (func(item))
                        return result;
                }
                catch (OperationCanceledException) { throw new AssertTimeoutException(timeout); }
        }

        [Log(LogLevel = LogAttributeLevel.Off)]
        public static async Task<IEnumerable<T>> TakeUntilAssertTimeoutAsync<T>(
            this ChannelReader<T> asyncCollection, Func<T, bool> func, int timeout = TestFrame.Timeout)
        {
            var timeoutCancel = new CancellationTokenSource(TestFrame.DebugTimeout(timeout));
            for (; ; )
                try
                {
                    T item = await asyncCollection.ReadAsync(timeoutCancel.Token);

                    //result.Add(item);

                    if (func(item))
                        return null;
                    //return result;
                }
                catch (OperationCanceledException) { throw new AssertTimeoutException(timeout); }
        }

        /// <summary><see cref="Task.Wait()"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="task">The task to act on.</param>
        /// <param name="timeout">The timeout.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static void WaitAssertTimeout(this Task task, int timeout = TestFrame.Timeout)
        {
            // UNDONE Contract.Requires(task != null);

            try
            {
                if (!task.Wait(TestFrame.DebugTimeout(timeout)))
                {
                    IgnoreFaults(task);
                    throw new AssertTimeoutException(timeout);
                }
            }
            catch (AggregateException e) { throw e.InnerException; }
        }

        /// <summary>Waits until the <see cref="SemaphoreSlim"/> instance has been released <paramref name="count"/>
        /// times, while asserting the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="semaphore">The semaphore.</param>
        /// <param name="count">The count.</param>
        /// <param name="timeout">(Optional) The timeout.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static void WaitAssertTimeout(this SemaphoreSlim semaphore, int count, int timeout = TestFrame.Timeout)
        {
            // UNDONE Contract.Requires(semaphore != null);

            var timeoutCancel = new CancellationTokenSource(TestFrame.DebugTimeout(timeout));
            try
            {
                while (count-- > 0) semaphore.Wait(timeoutCancel.Token);
            }
            catch (OperationCanceledException) { throw new AssertTimeoutException(timeout); }
        }

        /// <summary><see cref="WaitHandle.WaitOne()"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="waitHandle">The waitHandle to act on.</param>
        /// <param name="timeout">The timeout.</param>
        [Log(LogLevel = LogAttributeLevel.Off)]
        public static void WaitOneAssertTimeout(this WaitHandle waitHandle, int timeout = TestFrame.Timeout)
        {
            // UNDONE Contract.Requires(waitHandle != null);

            if (!waitHandle.WaitOne(TestFrame.DebugTimeout(timeout)))
                throw new AssertTimeoutException(timeout);
        }

    }
}
