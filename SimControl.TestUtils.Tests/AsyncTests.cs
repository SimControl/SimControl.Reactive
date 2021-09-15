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
        [Test, Apartment(ApartmentState.MTA)]
        public static async Task AsyncTestMethod__Apartment_MTA__current_SynchronizationContext_is_null__Async()
        {
            await Task.CompletedTask.ConfigureAwait(false);

            Assert.That(SynchronizationContext.Current, Is.Null);
        }

        [Test]
        public static async Task AsyncTestMethod__Apartment_none__current_SynchronizationContext_is_null__Async()
        {
            await Task.CompletedTask.ConfigureAwait(false);

            Assert.That(SynchronizationContext.Current, Is.Null);
        }

        [Test, Apartment(ApartmentState.STA)]
        public static async Task AsyncTestMethod__Apartment_STA__current_SynchronizationContext_is_not_null__Async()
        {
            await Task.CompletedTask.ConfigureAwait(false);

            Assert.That(SynchronizationContext.Current, Is.Not.Null);
        }

        [Test]
        public static async Task AsyncTestMethod__succeeds__Async()
        {
            await ForceContextSwitchAsync().ConfigureAwait(false);

            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "Finished");
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
            using var ready = new SemaphoreSlim(0, 1);

            Task task = Task.Run(async () => {
                await PermitContextSwitchAsync().ConfigureAwait(false);
                ready.Release();
                await ready.WaitAsync().AssertTimeoutAsync().ConfigureAwait(false);
            });

            ready.WaitAsync().AssertTimeoutAsync().Wait();
            ready.Release();

            task.AssertTimeoutAsync().Wait();
        }

        [Test, Apartment(ApartmentState.MTA)]
        public static void TestMethod__Apartment_MTA__current_SynchronizationContext_is_null() =>
            Assert.That(SynchronizationContext.Current, Is.Null);

        [Test]
        public static void TestMethod__Apartment_none__current_SynchronizationContext_is_null() =>
            Assert.That(SynchronizationContext.Current, Is.Null);

        [Test, Apartment(ApartmentState.STA)]
        public static void TestMethod__Apartment_STA__current_SynchronizationContext_is_null() =>
            Assert.That(SynchronizationContext.Current, Is.Null);

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
