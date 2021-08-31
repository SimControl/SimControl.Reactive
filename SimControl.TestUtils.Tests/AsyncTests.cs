// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log]
    [TestFixture]
    public class AsyncTests: TestFrame
    {
        [Test]
        public static async Task AsyncTestMethod__current_SynchronizationContext_is_null__Async()
        {
            await ForceContextSwitchAsync().ConfigureAwait(false);

            Assert.That(SynchronizationContext.Current, Is.Null);
        }

        [Test]
        public static async Task AsyncTestMethod__succeeds__Async()
        {
            await ForceContextSwitchAsync().ConfigureAwait(false);

            logger.Info("Finished");
        }

        [Test]
        public static void Exception_thrown_in_task_on_other_thread__is_caught_in_wait() =>
            Assert.That(Assert.Catch<AggregateException>(() => Task.Run(async () => {
                await ForceContextSwitchAsync().ConfigureAwait(false);
                throw new InvalidOperationException();
            }).Wait()).InnerException, Is.InstanceOf<InvalidOperationException>());

        [Test]
        public static void SemaphoreSlim__released_in_task_on_other_thread__is_signaled()
        {
            var ready = new SemaphoreSlim(0);

            Task task = Task.Run(async () => {
                await PermitContextSwitchAsync().ConfigureAwait(false);
                ready.Release();
            });

            ready.WaitAsync().AssertTimeoutAsync().Wait();

            task.AssertTimeoutAsync().Wait();
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
