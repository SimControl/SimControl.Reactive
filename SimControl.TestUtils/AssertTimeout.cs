// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using NLog;
using SimControl.Log;

// UNDONE implement AssertTimeout extension methods

namespace SimControl.TestUtils
{
    /// <summary>Test extensions for asserting timeouts.</summary>
    public static class AssertTimeoutExtensions
    {
        /// <summary>
        /// Wrapper that calls <see cref="ClientBase{TChannel}.Abort()"/> or <see cref="ClientBase{TChannel}.Close()"/>
        /// while asserting the test timeout.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="client">The client to act on.</param>
        /// <param name="timeout">The timeout.</param>
        //public static void AbortOrCloseAssertTimeout(this ICommunicationObject client, int timeout = TestFrame.Timeout)
        //{
        //    if (client != null)
        //        if (client.State == CommunicationState.Opened)
        //            try
        //            {
        //                client.Close(TimeSpan.FromMilliseconds(timeout));
        //            }
        //            catch (Exception)
        //            {
        //                client.Abort();
        //                throw;
        //            }
        //        else if (client.State != CommunicationState.Closed)
        //            client.Abort();
        //}


        public static async Task AssertTimeoutAsync(this Task task, int timeout = TestFrame.Timeout)
        {
            if (task != await Task.WhenAny(task, Task.Delay(TestFrame.DebugTimeout(timeout))))
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                task.ContinueWith(t => logger.Warn(t.Exception.InnerException, LogMethod.GetCurrentMethodName()),
                    CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
#pragma warning restore CS4014

                throw new AssertTimeoutException(timeout);
            };

            await task;
        }


        public static async Task<T> AssertTimeoutAsync<T>(this Task<T> task, int timeout = TestFrame.Timeout)
        {
            if (task != await Task.WhenAny(task, Task.Delay(TestFrame.DebugTimeout(timeout))))
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                task.ContinueWith(t => logger.Warn(t.Exception.InnerException, LogMethod.GetCurrentMethodName()),
                    CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
#pragma warning restore CS4014

                throw new AssertTimeoutException(timeout);
            };

            return await task;
        }

        /// <summary><see cref="Thread.Join()"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="thread">The thread to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="AssertTimeoutException"></exception>
        public static void JoinAssertTimeout(this Thread thread, int timeout = TestFrame.Timeout)
        {
            if (!thread.Join(TestFrame.DebugTimeout(timeout)))
                throw new AssertTimeoutException(timeout);
        }

        public static async Task<IEnumerable<T>> ReadUntilAssertTimeoutAsync<T>(
            this ChannelReader<T> asyncCollection, Func<T, bool> func, int timeout = TestFrame.Timeout)
        {
            var result = new List<T>();

            var timeoutCancel = new CancellationTokenSource(TestFrame.DebugTimeout(timeout));
            for (; ; )
                try
                {
                    T item = await asyncCollection.ReadAsync(timeoutCancel.Token);

                    result.Add(item);

                    if (func(item))
                        return result;
                }
                catch (OperationCanceledException) { throw new AssertTimeoutException(timeout); }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
