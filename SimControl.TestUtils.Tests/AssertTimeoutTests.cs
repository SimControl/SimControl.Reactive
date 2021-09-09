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
        // TODO CloseAssertTimeoutAsync

        [Test]
        public static void AssertTimeout__task_is_cancled__OperationCanceledException_is_thrown()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.That(() => Task.Run(() => {
                ForceContextSwitch();
                cts.Token.ThrowIfCancellationRequested();
            }).AssertTimeoutAsync(), Throws.TypeOf<OperationCanceledException>());
        }

        [Test]
        public static void AssertTimeout__task_terminates_faulted__exception_is_rethrown() => Assert.That(() =>
            Task.Run(() => {
                ForceContextSwitch();
                throw new InvalidOperationException();
            }).AssertTimeoutAsync(), Throws.TypeOf<InvalidOperationException>());

        [Test]
        public static void AssertTimeout__task_terminates_successful_no_exception_is_thrown() =>
            Task.Run(ForceContextSwitch).AssertTimeoutAsync().Wait();

        [Test]
        public static void AssertTimeoutAsync__task_not_completed_within_timeout__AssertTimeoutException_is_thrown()
        {
            using var sem = new SemaphoreSlim(0, 1);

            Assert.That(() => Task.Run(async () => await sem.WaitAsync().ConfigureAwait(false)).AssertTimeoutAsync(1),
                Throws.TypeOf<AssertTimeoutException>());
        }

        [Test]
        public static void AssertTimeoutAsync__task_not_completed_within_timeout__UnobservedTaskException_not_triggerd()
        {
            using var sem = new SemaphoreSlim(0, 1);

            Assert.That(() => Task.Run(async () => {
                await sem.WaitAsync().ConfigureAwait(false);
                //throw new InvalidOperationException();
            }).AssertTimeoutAsync(1), Throws.InstanceOf<AssertTimeoutException>());

            sem.Release();
        }

        [Test]
        public static void AssertTimeoutAsyncT__task_not_completed_within_timeout__AssertTimeoutException_is_thrown()
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
        public static void AssertTimeoutT__task_is_cancled__OperationCanceledException_is_thrown()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.That(() => Task.Run(() => {
                ForceContextSwitch();
                cts.Token.ThrowIfCancellationRequested();
                return 1;
            }).AssertTimeoutAsync(), Throws.TypeOf<OperationCanceledException>());
        }

        [Test]
        public static void AssertTimeoutT__task_terminates_faulted__exception_is_rethrown() => Assert.That(() =>
            Task.Run(() => {
                ForceContextSwitch();
                throw new InvalidOperationException();
                return 1;
            }).AssertTimeoutAsync(), Throws.TypeOf<InvalidOperationException>());

        [Test]
        public static void AssertTimeoutT__task_terminates_successful_no_exception_is_thrown() =>
            Assert.That(Task.Run(() => {
                ForceContextSwitch();
                return 1;
            }).AssertTimeoutAsync().Result, Is.EqualTo(1));

        [Test]
        public static void JoinAssertTimeout__thread_does_not_join_within_timeout__AssertTimeoutException_is_thrown()
        {
            using var sem = new SemaphoreSlim(0, 1);

            var thread = new Thread(() => sem.Wait());
            thread.Start();

            Assert.That(() => thread.JoinAssertTimeout(TestFrame.MinTimerResolution),
                Throws.TypeOf<AssertTimeoutException>());

            sem.Release();
        }

        [Test]
        public static void JoinAssertTimeout__thread_joines_within_timeout__successfully()
        {
            var sem = new SemaphoreSlim(0, 1);

            var thread = new Thread(async () => {
                sem.Release();
                await sem.WaitAsync().AssertTimeoutAsync();
                ;
            });
            thread.Start();

            sem.WaitAsync().AssertTimeoutAsync().Wait();
            sem.Release();

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

            Assert.That(result, Is.EqualTo(new[] { 0, 1 }));
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
