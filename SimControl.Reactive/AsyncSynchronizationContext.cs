// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

namespace SimControl.Reactive;

/// <summary>Test extensions for asserting timeouts.</summary>
public static class AsyncSynchronizationContext
{
    /// <summary>Dispatch a message to a synchronization context asynchronous.</summary>
    /// <param name="context">The context.</param>
    /// <param name="action"></param>
    /// <returns><see cref="Task"/></returns>
    public static Task SendAsync(this SynchronizationContext context, Action action)
    {
        TaskCompletionSource<bool> tcs = new();

        context.Post(delegate
        {
            try
            {
                action();
                tcs.SetResult(true);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }, null);

        return tcs.Task;
    }
}
