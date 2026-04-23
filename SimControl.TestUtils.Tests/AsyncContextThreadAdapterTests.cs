// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using NUnit.Framework;
using SimControl.Logging;
using System.Threading.Channels;

namespace SimControl.TestUtils.Tests;

public sealed class SingleThreadSynchronizationContext
        : SynchronizationContext
{
    private readonly Channel<WorkItem> queue = Channel.CreateUnbounded<WorkItem>();

    public override void Post(SendOrPostCallback d, object state) => queue.Writer.TryWrite(new WorkItem(d, state));

    public async void RunAsync(CancellationToken cancellation = default)
    {
        WorkItem workItem;

        for (; ; )
        {
            workItem = await queue.Reader.ReadAsync(cancellation);

            workItem.Action(workItem.State);
        }
    }
}

public class WorkItem
{
    public SendOrPostCallback Action { get; set; }
    public object State { get; set; }

    public WorkItem(SendOrPostCallback action, object state)
    {
        Action = action;
        State = state;
    }
}

[Log, TestFixture]
public class AsyncContextThreadAdapterTests : TestFrame
{
    [Test]
    public static void Xxx()
    {
        //using (var acta = new AsyncContextThreadAdapter())
        //{
        //    var ready = new AutoResetEvent(false);

        //    Task task = acta.Factory.Run(() => { ContextSwitch(); ready.Set(); });

        //    ready.WaitOneAssertTimeout();
        //    task.WaitAssertTimeout();
        //}
    }
}
