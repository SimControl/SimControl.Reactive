// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using NLog;
using SimControl.Log;

namespace SimControl.TestUtils
{
    /// <summary>Test extensions for asserting timeouts.</summary>
    public static class AssertTimeoutExtensions
    {
        /// <summary>Assert the <see cref="Task"/> finishes within the specified timeout asynchronous.</summary>
        /// <param name="task">The task.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="AssertTimeoutException"></exception>
        public static async Task AssertTimeoutAsync(this Task task, int timeout = TestFrame.Timeout)
        {
            if (task != await Task.WhenAny(task, Task.Delay(TestFrame.DebugTimeout(timeout))).ConfigureAwait(false))
            {
#pragma warning disable CS4014 // Because this call is not awaited,
                // execution of the current method continues before the call is completed
                task.ContinueWith(t =>
                    logger.Message(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, t.Exception.InnerException),
                    CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
#pragma warning restore CS4014

                throw new AssertTimeoutException(timeout);
            }

            await task.ConfigureAwait(false);
        }

        /// <summary>Assert the <see cref="Task"/> finishes within the specified timeout asynchronous.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns><br/></returns>
        /// <exception cref="AssertTimeoutException"></exception>
        public static async Task<T> AssertTimeoutAsync<T>(this Task<T> task, int timeout = TestFrame.Timeout)
        {
            if (task != await Task.WhenAny(task, Task.Delay(TestFrame.DebugTimeout(timeout))).ConfigureAwait(false))
            {
#pragma warning disable CS4014 // Because this call is not awaited,
                // execution of the current method continues before the call is completed
                task.ContinueWith(t =>
                    logger.Message(LogLevel.Error, LogMethod.GetCurrentMethodName(), null, t.Exception.InnerException),
                    CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
#pragma warning restore CS4014

                throw new AssertTimeoutException(timeout);
            }

            return await task.ConfigureAwait(false);
        }

#if !NET5_0_OR_GREATER

        /// <summary>
        /// Assert that the <see cref="ICommunicationObject"/> closes within the specified timeout asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="AssertTimeoutException"></exception>
        public static async Task CloseAssertTimeoutAsync(this ICommunicationObject client,
            int timeout = TestFrame.Timeout)
        {
            try
            {
                await Task.Factory.FromAsync(client.BeginClose, client.EndClose, TimeSpan.FromMilliseconds(timeout),
                    null).ConfigureAwait(false);
            }
            catch (CommunicationObjectFaultedException)
            {
                client.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                client.Abort();
                throw new AssertTimeoutException(timeout);
            }
        }

#endif

        /// <summary>Assert <see cref="Thread.Join()"/> finishes within the specified timeout asynchronous.</summary>
        /// <param name="thread">The thread to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="AssertTimeoutException"></exception>
        public static void JoinAssertTimeout(this Thread thread, int timeout = TestFrame.Timeout)
        {
            if (!thread.Join(TestFrame.DebugTimeout(timeout)))
                throw new AssertTimeoutException(timeout);
        }

        /// <summary>Read items until <paramref name="func"/> is true while asserting the specified timeout.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncCollection">The asynchronous collection.</param>
        /// <param name="func">The function.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns><br/></returns>
        /// <exception cref="AssertTimeoutException"></exception>
        public static async Task<IEnumerable<T>> ReadUntilAssertTimeoutAsync<T>(
            this ChannelReader<T> asyncCollection, Func<T, bool> func, int timeout = TestFrame.Timeout)
        {
            var result = new List<T>();

            using var timeoutCancel = new CancellationTokenSource(TestFrame.DebugTimeout(timeout));
            for (; ; )
                try
                {
                    T item = await asyncCollection.ReadAsync(timeoutCancel.Token).ConfigureAwait(false);

                    result.Add(item);

                    if (func(item))
                        return result;
                }
                catch (OperationCanceledException) { throw new AssertTimeoutException(timeout); }
        }

        ///// <summary>Dispatch a message to a synchronization context asynchronous.</summary>
        ///// <param name="context">The context.</param>
        ///// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
        ///// <param name="state">The object passed to the delegate.</param>
        ///// <returns><see cref="Task"/></returns>
        //[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        //public static Task SendAsync(this SynchronizationContext context, SendOrPostCallback d, object? state = null)
        //{
        //    var tcs = new TaskCompletionSource<bool>();

        // context.Post(delegate { try { d(state); tcs.SetResult(true); } catch (Exception e) { tcs.SetException(e); }
        // }, null);

        //    return tcs.Task;
        //}

        /// <summary>Dispatch a message to a synchronization context asynchronous.</summary>
        /// <param name="context">The context.</param>
        /// <param name="action"></param>
        /// <returns><see cref="Task"/></returns>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public static Task SendAsync(this SynchronizationContext context, Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            context.Post(delegate {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception e) { tcs.SetException(e); }
            }, null);

            return tcs.Task;
        }

        /// <summary>Dispatch a message to a synchronization context asynchronous.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="func"></param>
        /// <returns><see cref="Task"/></returns>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public static Task<T> SendAsync<T>(this SynchronizationContext context, Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();

            context.Post(delegate {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e) { tcs.SetException(e); }
            }, null);

            return tcs.Task;
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
