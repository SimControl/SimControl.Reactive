// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log]
    [TestFixture]
    public class AssertTimeoutTests: TestFrame
    {
        [Test]
        public static void AssertTimeoutAsync__Canceled()
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();

                async Task f(CancellationToken token) {
#if NET40
                    await TaskEx.Delay(1); token.ThrowIfCancellationRequested(); }
#else
                    await Task.Delay(1); token.ThrowIfCancellationRequested(); };
#endif
                Assert.ThrowsAsync<OperationCanceledException>(() => f(cts.Token).AsyncAssertTimeoutAsync());
            }
        }

        [Test]
        public static void AssertTimeoutAsync__Faulted() =>
            Assert.ThrowsAsync<InvalidOperationException>(() => AssertTimeout.AssertTimeoutAsync(async () =>
#if NET40
                { await TaskEx.Delay(1); throw new InvalidOperationException(); }));
#else
                { await Task.Delay(1); throw new InvalidOperationException(); }));
#endif

        [Test]
        public static async Task AssertTimeoutAsync__RanToCompletion() =>
#if NET40
            await TaskEx.Delay(10).AsyncAssertTimeoutAsync();
#else
            await Task.Delay(10).AsyncAssertTimeoutAsync();
#endif

        [Test]
        public static void AssertTimeoutAsync__TimeoutException() =>
            Assert.ThrowsAsync<TimeoutException>(() => AssertTimeout.AssertTimeoutAsync(async () =>
#if NET40
                { await TaskEx.Delay(1000); throw new InvalidOperationException(); }, 1));
#else
                { await Task.Delay(1000); throw new InvalidOperationException(); }, 1));
#endif

        [Test]
        public static async Task AssertTimeoutAsync_T() => Assert.That(await ((Func<Task<int>>) (async () =>
#if NET40
            { await TaskEx.Delay(10); return 1; }))().AssertTimeoutAsync(), Is.EqualTo(1));

#else
            { await Task.Delay(10); return 1; }))().AssertTimeoutAsync(), Is.EqualTo(1));
#endif

        [Test]
        public static void JoinAssertTimeout()
        {
            var thread = new Thread(() => { } );

            thread.Start();

            thread.JoinAssertTimeout();
        }

        [Test]
        public static void ResultAssertTimeoutAsync_T() =>
#if NET40
        ((Func<Task<int>>) (async () => { await TaskEx.Delay(10); return 1; }))().ResultAssertTimeout();

#else
        ((Func<Task<int>>) (async () => { await Task.Delay(10); return 1; }))().ResultAssertTimeout();
#endif

        [Test]
        public static void RunAssertTimeout() =>
#if NET40
            ((Action) (() => TaskEx.Delay(10))).RunAssertTimeout();

#else
            ((Action) (() => Task.Delay(10))).RunAssertTimeout();
#endif

        [Test]
        public static void RunAssertTimeout_T() =>
#if NET40
            Assert.That(((Func<int>) (() => { TaskEx.Delay(10); return 1; })).RunAssertTimeout(), Is.EqualTo(1));

#else
            Assert.That(((Func<int>) (() => { Task.Delay(10); return 1;})).RunAssertTimeout(), Is.EqualTo(1));
#endif

        [Test]
        public static void WaitAssertTimeout() =>
#if NET40
            TaskEx.Delay(10).WaitAssertTimeout();

#else
            Task.Delay(10).WaitAssertTimeout();
#endif
    }
}
