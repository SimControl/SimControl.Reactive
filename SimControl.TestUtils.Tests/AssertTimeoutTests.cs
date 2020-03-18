// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

// TODO TakeAssertTimeout tests TODO TakeUntilAssertTimeout tests TODO SemaphoreSlim.WaitAssertTimeout tests

namespace SimControl.TestUtils.Tests
{
    // UNDONE [Log]
    [TestFixture]
    public class AssertTimeoutTests: TestFrame
    {
        [Test]
        public static void AssertTimeoutAsync__Canceled()
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();

                async Task f(CancellationToken token)
                {
                    await ThreadSwitchDelay(); token.ThrowIfCancellationRequested();
                }

                _ = Assert.ThrowsAsync(Is.InstanceOf(typeof(OperationCanceledException)),
                    () => f(cts.Token).AsyncAssertTimeoutAsync());
            }
        }

        [Test]
        public static void AssertTimeoutAsync__Faulted() => Assert.ThrowsAsync<InvalidOperationException>(() =>
            AssertTimeoutAsync(async () => { await ThreadSwitchDelay(); throw new InvalidOperationException(); }));

        [Test]
        public static async Task AssertTimeoutAsync__RanToCompletion() =>
            await ThreadSwitchDelay().AsyncAssertTimeoutAsync();

        [Test]
        public static void AssertTimeoutAsync__TimeoutException() =>
            Assert.ThrowsAsync<TimeoutException>(() => AssertTimeoutAsync(async () =>
#if NET40
                { await TaskEx.Delay(1000); throw new InvalidOperationException(); }, MinTimerResolution));

#else
                { await Task.Delay(1000); throw new InvalidOperationException(); }, MinTimerResolution));
#endif

        [Test]
        public static async Task AssertTimeoutAsync_T() => Assert.That(await ((Func<Task<int>>) (async () => { await ThreadSwitchDelay(); return 1; }))().AssertTimeoutAsync(), Is.EqualTo(1));

        [Test]
        public static void JoinAssertTimeout()
        {
            var thread = new Thread(() => { } );

            thread.Start();

            thread.JoinAssertTimeout();
        }

        [Test]
        public static void ResultAssertTimeoutAsync_T() =>
            ((Func<Task<int>>) (async () => { await ThreadSwitchDelay(); return 1; }))().ResultAssertTimeout();

        [Test]
        public static void RunAssertTimeout() => ((Action) (() => ThreadSwitchDelay().Wait())).RunAssertTimeout();

        [Test]
        public static void RunAssertTimeout_T() => Assert.That(((Func<int>) (() => { ThreadSwitchDelay().Wait(); return 1; })).RunAssertTimeout(), Is.EqualTo(1));

        [Test]
        public static void WaitAssertTimeout() => ThreadSwitchDelay().WaitAssertTimeout();

        [Test]
        public static void WaitOneAssertTimeout()
        {
            using (var ready = new AutoResetEvent(true))
                ready.WaitOneAssertTimeout();
        }
    }
}
