// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

using NUnit.Framework;
using SimControl.Logging;
using System.Threading.Channels;

namespace SimControl.TestUtils.Tests;

[TestFixture, Log]
public class AssertTimeoutTests : TestFrame
{
    // TODO CloseAssertTimeoutAsync

    [Test, /*Isolated*/]
    public void AssertTimeoutAsync__task_completed_with_exception__UnobservedTaskException_triggered()
    {
        using SemaphoreSlim sem = new(0, 1);

        ThrowUnhandledExceptionInAsyncTask(sem, false);

        _ = sem.Release();

        LongContextSwitch(50);
        ForceGarbageCollection();

        Exception? e = TryTakePendingException();

        Assert.That(e, Is.Not.Null);
        Assert.That(e, Is.TypeOf(typeof(AggregateException)));
        Assert.That(e.InnerException, Is.TypeOf(typeof(InvalidOperationException)));
        Assert.That(e.InnerException.Message, Is.EqualTo(nameof(ThrowUnhandledExceptionInAsyncTask)));
    }

    [Test, /*Isolated*/]
    public void AssertTimeoutAsync__task_completed_with_exception__UnobservedTaskException_triggered__T()
    {
        using SemaphoreSlim sem = new(0, 1);

        ThrowUnhandledExceptionInAsyncTask__T(sem, false);

        _ = sem.Release();

        LongContextSwitch(50);
        ForceGarbageCollection();

        Exception? e = TryTakePendingException();

        Assert.That(e, Is.Not.Null);
        Assert.That(e, Is.TypeOf(typeof(AggregateException)));
        Assert.That(e.InnerException, Is.TypeOf(typeof(InvalidOperationException)));
        Assert.That(e.InnerException.Message, Is.EqualTo(nameof(ThrowUnhandledExceptionInAsyncTask__T)));
    }

    [Test]
    public void AssertTimeoutAsync__task_is_cancled__OperationCanceledException_is_thrown()
    {
        using CancellationTokenSource cts = new();
        cts.Cancel();

        _ = Assert.ThrowsAsync<OperationCanceledException>(() => Task.Run(() =>
        {
            ForceContextSwitch();
            cts.Token.ThrowIfCancellationRequested();
        }).AssertTimeoutAsync());
    }

    [Test]
    public void AssertTimeoutAsync__task_is_cancled__OperationCanceledException_is_thrown__T()
    {
        using CancellationTokenSource cts = new();
        cts.Cancel();

        _ = Assert.ThrowsAsync<OperationCanceledException>(() => Task.Run(() =>
        {
            ForceContextSwitch();
            cts.Token.ThrowIfCancellationRequested();
            return 1;
        }).AssertTimeoutAsync());
    }

    [Test]
    public void AssertTimeoutAsync__task_not_completed_within_timeout__AssertTimeoutException_is_thrown()
    {
        using SemaphoreSlim sem = new(0, 1);

        _ = Assert.ThrowsAsync<AssertTimeoutException>(() => Task.Run(async () =>
            await sem.WaitAsync().ConfigureAwait(false)).AssertTimeoutAsync(1));

        _ = sem.Release();
    }

    [Test]
    public void AssertTimeoutAsync__task_not_completed_within_timeout__AssertTimeoutException_is_thrown__T()
    {
        using SemaphoreSlim sem = new(0, 1);

        _ = Assert.ThrowsAsync<AssertTimeoutException>(() => Task.Run(async () =>
        {
            await sem.WaitAsync().ConfigureAwait(false);
            return 1;
        }).AssertTimeoutAsync(1));

        _ = sem.Release();
    }

    [Test]
    public void AssertTimeoutAsync__task_not_completed_within_timeout__UnobservedTaskException_not_triggered()
    {
        using SemaphoreSlim sem = new(0, 1);

        ThrowUnhandledExceptionInAsyncTask(sem, true);

        _ = sem.Release();

        LongContextSwitch(10);
    }

    [Test]
    public void AssertTimeoutAsync__task_not_completed_within_timeout__UnobservedTaskException_not_triggered__T()
    {
        using SemaphoreSlim sem = new(0, 1);

        ThrowUnhandledExceptionInAsyncTask__T(sem, true);

        _ = sem.Release();

        LongContextSwitch(10);
    }

    [Test]
    public void AssertTimeoutAsync__task_terminates_faulted__exception_is_rethrown() =>
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            Task.Run(() => throw new InvalidOperationException()).AssertTimeoutAsync());

    [Test]
    public void AssertTimeoutAsync__task_terminates_faulted__exception_is_rethrown__T() =>
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            Task.Run(ThrowExceptionInvalidOperationException).AssertTimeoutAsync());

    [Test]
    public void AssertTimeoutAsync__task_terminates_successful_no_exception_is_thrown() =>
        Task.Run(PermitContextSwitch).AssertTimeoutAsync().Wait();

    [Test]
    public void AssertTimeoutAsync__task_terminates_successful_no_exception_is_thrown__T() =>
        Task.Run(IntMethod).AssertTimeoutAsync().Wait();

    [Test]
    public void JoinAssertTimeout__thread_does_not_join_within_timeout__AssertTimeoutException_is_thrown()
    {
        using SemaphoreSlim sem = new(0, 1);

        Thread thread = new(sem.Wait);
        thread.Start();

        _ = Assert.Throws<AssertTimeoutException>(() => thread.JoinAssertTimeout(TestFrame.MinTimerResolution));

        _ = sem.Release();

        thread.JoinAssertTimeout();
    }

    [Test]
    public void JoinAssertTimeout__thread_joins_within_timeout__successfully()
    {
        using SemaphoreSlim sem = new(0, 1);

        Thread thread = new(() => sem.Release());
        thread.Start();

        sem.WaitAsync().AssertTimeoutAsync().Wait();

        thread.JoinAssertTimeout();
    }

    [Test]
    public void ReadUntilAssertTimeout()
    {
        Channel<int> channel = Channel.CreateUnbounded<int>(
            new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });

        Assert.That(channel.Writer.TryWrite(0));
        Assert.That(channel.Writer.TryWrite(1));
        Assert.That(channel.Writer.TryWrite(2));

        IEnumerable<int>? result = channel.Reader.ReadUntilAssertTimeoutAsync(i => i == 1).Result;

        Assert.That(result, Is.EqualTo(new[] { 0, 1 }));
    }

    [Test]
    public void ReadUntilAssertTimeout__AssertTimeoutException()
    {
        Channel<int> channel = Channel.CreateUnbounded<int>(
            new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });

        Assert.That(channel.Writer.TryWrite(0));

        _ = Assert.ThrowsAsync<AssertTimeoutException>(() =>
            channel.Reader.ReadUntilAssertTimeoutAsync(i => i == 1, MinTimerResolution));
    }

    private static int IntMethod() => 1;

    private static int ThrowExceptionInvalidOperationException() => throw new InvalidOperationException();

    private static void ThrowUnhandledExceptionInAsyncTask(SemaphoreSlim sem, bool throwAssertTimeoutException)
    {
        Task task = Task.Run(async () =>
        {
            await sem.WaitAsync().ConfigureAwait(false);
            throw new InvalidOperationException(nameof(ThrowUnhandledExceptionInAsyncTask));
        });

        if (throwAssertTimeoutException)
            _ = Assert.ThrowsAsync<AssertTimeoutException>(() => task.AssertTimeoutAsync(1));
    }

    private static void ThrowUnhandledExceptionInAsyncTask__T(SemaphoreSlim sem, bool throwAssertTimeoutException)
    {
        Task task = Task.Run(() => WaitForSemaphoreThrowExceptionInvalidOperationExceptionAsync(sem));

        if (throwAssertTimeoutException)
            _ = Assert.ThrowsAsync<AssertTimeoutException>(() => task.AssertTimeoutAsync(1));
    }

    private static async Task<int> WaitForSemaphoreThrowExceptionInvalidOperationExceptionAsync(SemaphoreSlim sem)
    {
        await sem.WaitAsync().ConfigureAwait(false);
        throw new InvalidOperationException(nameof(ThrowUnhandledExceptionInAsyncTask__T));
    }
}
