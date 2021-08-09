// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [TestFixture, Log]
    public class AssertTimeoutTests: TestFrame
    {
        [Test]
        public static void AssertTimeout__AssertTimeoutException()
        {
            var sem = new SemaphoreSlim(0);

            Assert.That(() => Task.Run(async () => await sem.WaitAsync().ConfigureAwait(false)).AssertTimeoutAsync(1),
                Throws.TypeOf<AssertTimeoutException>());

            sem.Release();
        }

        [Test]
        public static void AssertTimeout__Canceled()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.That(() => Task.Run(() => {
                ContextSwitch();
                cts.Token.ThrowIfCancellationRequested();
            }).AssertTimeoutAsync(), Throws.TypeOf<OperationCanceledException>());
        }

        [Test]
        public static void AssertTimeout__Faulted() => Assert.That(() =>
            Task.Run(() => {
                ContextSwitch();
                throw new InvalidOperationException();
            }).AssertTimeoutAsync(), Throws.TypeOf<InvalidOperationException>());

        [Test]
        public static void AssertTimeout__RanToCompletion() => Task.Run(ContextSwitch).AssertTimeoutAsync().Wait();

        [Test]
        public static void AssertTimeout_T__AssertTimeoutException()
        {
            var sem = new SemaphoreSlim(0);

            Assert.That(() =>
            Task.Run(async () => {
                await sem.WaitAsync().ConfigureAwait(false);
                return 1;
            }).AssertTimeoutAsync(1), Throws.TypeOf<AssertTimeoutException>());
            sem.Release();
        }

        [Test]
        public static void AssertTimeout_T__Canceled()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.That(() => Task.Run(() => {
                ContextSwitch();
                cts.Token.ThrowIfCancellationRequested();
                return 1;
            }).AssertTimeoutAsync(), Throws.TypeOf<OperationCanceledException>());
        }

        [Test]
        public static void AssertTimeout_T__Faulted() => Assert.That(() =>
            Task.Run(() => {
                ContextSwitch();
                throw new InvalidOperationException();
                return 1;
            }).AssertTimeoutAsync(), Throws.TypeOf<InvalidOperationException>());

        [Test]
        public static void AssertTimeout_T__RanToCompletion() => Assert.That(Task.Run(() => {
            ContextSwitch();
            return 1;
        }).AssertTimeoutAsync().Result, Is.EqualTo(1));

        [Test]
        public static void JoinAssertTimeout__AssertTimeoutException()
        {
            var sem = new SemaphoreSlim(0);

            var thread = new Thread(() => sem.Wait());
            thread.Start();

            Assert.That(() => thread.JoinAssertTimeout(TestFrame.MinTimerResolution),
                Throws.TypeOf<AssertTimeoutException>());

            sem.Release();
        }

        [Test]
        public static void JoinAssertTimeout__WaitUntilThreadStarted__ThreadHasJoined()
        {
            var sem = new SemaphoreSlim(0);

            var thread = new Thread(() => sem.Release());
            thread.Start();

            sem.WaitAsync().AssertTimeoutAsync().Wait();
            thread.JoinAssertTimeout();
        }

        [Test]
        public static void ReadUntilAssertTimeout()
        {
            Channel<int> channel = Channel.CreateUnbounded<int>(
                new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });

            ChannelWriter<int> writer = channel.Writer;
            ChannelReader<int> reader = channel.Reader;

            Assert.That(writer.TryWrite(0));
            Assert.That(writer.TryWrite(1));
            Assert.That(writer.TryWrite(2));

            IEnumerable<int>? result = reader.ReadUntilAssertTimeoutAsync(i => i == 1).Result;

            Assert.That(result, Is.EqualTo(new int[] { 0, 1 }));
        }

        [Test]
        public static void ReadUntilAssertTimeout__AssertTimeoutException()
        {
            Channel<int> channel = Channel.CreateUnbounded<int>(
                new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });

            ChannelWriter<int> writer = channel.Writer;
            ChannelReader<int> reader = channel.Reader;

            Assert.That(writer.TryWrite(0));

            Assert.That(() => reader.ReadUntilAssertTimeoutAsync(i => i == 1, MinTimerResolution),
                Throws.TypeOf<AssertTimeoutException>());
        }
    }
}
