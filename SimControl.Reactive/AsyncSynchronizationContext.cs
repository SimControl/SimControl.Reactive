// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace SimControl.Reactive
{
    /// <summary>Test extensions for asserting timeouts.</summary>
    public static class AsyncSynchronizationContext
    {
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
    }
}
