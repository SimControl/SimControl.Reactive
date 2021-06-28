// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

// UNDONE TakeUntilAssertTimeout tests
// UNDONE SemaphoreSlim.WaitAssertTimeout tests
// UNDONE AssertTimeoutTests

namespace SimControl.TestUtils.Tests
{
    // UNDONE [Log]
    [TestFixture]
    public class AssertTimeoutTests: TestFrame
    {
        [Test]
        public static void AssertTimeout__Canceled()
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();

                _ = Assert.ThrowsAsync(Is.InstanceOf(typeof(OperationCanceledException)), () => Task.Run(() => {
                    ContextSwitch();cts.Token.ThrowIfCancellationRequested(); }).AssertTimeout());
            }
        }

        [Test]
        public static void AssertTimeout__Faulted() => Assert.ThrowsAsync<InvalidOperationException>(() =>
            Task.Run(() => { ContextSwitch(); throw new InvalidOperationException(); }).AssertTimeout());

        [Test]
        public static void AssertTimeout__RanToCompletion() => Task.Run(() => ContextSwitch()).AssertTimeout();

        [Test]
        public static void AssertTimeout__TimeoutException() => Assert.ThrowsAsync<TimeoutException>(() =>
            Task.Run(() => Thread.Sleep(1000)).AssertTimeout(1));

        /*
            [Test]
            public static void AssertTimeoutAsync__Canceled()
            {
                using (var cts = new CancellationTokenSource())
                {
                    cts.Cancel();

                    _ = Assert.ThrowsAsync(Is.InstanceOf(typeof(OperationCanceledException)),
                        () => Run(async () => {
                            await ContextSwitchDelay(); cts.Token.ThrowIfCancellationRequested();
                        }).AssertTimeoutAsync());
                }
            }

            [Test]
            public static void AssertTimeoutAsync__Faulted() => Assert.ThrowsAsync<InvalidOperationException>(() => Run(
                async () => { await ContextSwitchDelay(); throw new InvalidOperationException(); }).AssertTimeoutAsync());

            [Test]
            public static Task AssertTimeoutAsync__RanToCompletion() =>
                Run(async () => await ContextSwitchDelay()).AssertTimeoutAsync();

            [Test]
            public static void AssertTimeoutAsync__TimeoutException() =>
                Assert.ThrowsAsync<TimeoutException>((NUnit.Framework.AsyncTestDelegate)(() => AssertTimeoutAsync((AsyncTestDelegate) (() =>
    (Task) AssertTimeout.AssertTimeoutAsync(Run((Action) (async () => await Delay((int) 1000))), (int) ContextSwitch)))));

            [Test]
            public static async Task AssertTimeoutAsync_T() => Assert.That(await ((Func<Task<int>>) (async () => { await ContextSwitchDelay(); return 1; }))().AssertTimeoutAsync(), Is.EqualTo(1));

            [Test]
            public static void JoinAssertTimeout__WaitUntilThreadStarted__ThreadHasJoined()
            {
                using (var ready = new AutoResetEvent(false))
                {
                    var thread = new Thread(() => ready.Set());
                    thread.Start();

                    ready.WaitOneAssertTimeout();
                    thread.JoinAssertTimeout();
                }
            }

            [Test]
            public static void JoinAssertTimeout_Thread_TimeoutException()
            {
                using (var ready = new AutoResetEvent(false))
                {
    #if NET40
                    var thread = new Thread(() => { _ = ready.Set(); TaskEx.Delay(-1).Wait(); });
    #else
                    var thread = new Thread(() => { _ = ready.Set(); Task.Delay(-1).Wait(); });
    #endif
                    thread.Start();

                    ready.WaitOneAssertTimeout();
                    _ = Assert.Throws<TimeoutException>(() => thread.JoinAssertTimeout(ContextSwitch));
                }
            }

            [Test]
            public static void ResultAssertTimeoutAsync_T() =>
                ((Func<Task<int>>) (async () => { await ContextSwitchDelay(); return 1; }))().ResultAssertTimeout();

            [Test]
            public static void ResultAssertTimeoutAsync_T__ThrowException__IsCaught() =>
                Assert.Throws<InvalidOperationException>(() => ((Func<Task<int>>) (async () => {
                    await ContextSwitchDelay(); throw new InvalidOperationException(); }))().ResultAssertTimeout());

            [Test]
            public static void TakeAssertTimeout_BlockingCollection()
            {
                using (var blockingCollection = new BlockingCollection<bool>())
                {
    #if NET40
                    Task task = TaskEx.Run(() => blockingCollection.Add(true));
    #else
                    Task task = Task.Run(() => blockingCollection.Add(true));
    #endif
                    Assert.That(blockingCollection.TakeAssertTimeout(), Is.True);

                    task.WaitAssertTimeout();
                }
            }

            [Test]
            public static void TakeAssertTimeout_BlockingCollection_TimeoutException()
            {
                using (var blockingCollection = new BlockingCollection<bool>())
                    _ = Assert.Throws<TimeoutException>(
                        () => Assert.That(blockingCollection.TakeAssertTimeout(ContextSwitch), Is.True));
            }


            [Test]
            public static void WaitAssertTimeout_Task() =>
                Run(() => ContextSwitchDelay().Wait()).WaitAssertTimeout();

            [Test]
            public static void WaitAssertTimeout_Task_ExceptionIsCaught() =>
                Assert.Throws<InvalidOperationException>(() => Run(() => {
                    ContextSwitchDelay().Wait(); throw new InvalidOperationException(); }).WaitAssertTimeout());

            [Test]
            public static void WaitAssertTimeout_Task_TimeoutException() => Assert.Throws<TimeoutException>(() =>
                Run(() => Delay(-1).Wait()).WaitAssertTimeout(ContextSwitch));

            [Test]
            public static void WaitOneAssertTimeout_AsynchronousSetAutoResetEvent()
            {
                using (var ready = new AutoResetEvent(false))
                {
    #if NET40
                    Task task = TaskEx.Run(() => ready.Set());
    #else
                    Task task = Task.Run(() => ready.Set());
    #endif

                    ready.WaitOneAssertTimeout();
                    task.WaitAssertTimeout();
                }
            }

            [Test]
            public static void WaitOneAssertTimeout_WaitHandle_TimeoutException()
            {
                using (var ready = new AutoResetEvent(false))
                    _ = Assert.Throws<TimeoutException>(() => ready.WaitOneAssertTimeout(ContextSwitch));
            }
        }
            [Test]
            public static void ResultAssertTimeout_Task_TimeoutException() =>
                Assert.Throws<TimeoutException>(() => TaskEx.Run(() => {
                    TaskEx.Delay(int.MaxValue).Wait();
                    return true;
                }).ResultAssertTimeout(ContextSwitch));

            [Test]
            public static void RunAction_ExceptionIsCaught() =>
                Assert.Throws<InvalidOperationException>(() => RunAssertTimeout(() =>
                    throw new InvalidOperationException()));

            [Test]
            public static void RunAssertTimeout_Action() => RunAssertTimeout(() =>
                logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod()));

            [Test]
            public static void RunAssertTimeout_Action_TimeoutException() => Assert.Throws<TimeoutException>(() =>
                RunAssertTimeout(() => TaskEx.Delay(int.MaxValue).Wait(), ContextSwitch));

            [Test]
            public static void RunAssertTimeout_Function_TimeoutException()
            {
                bool ret;

                Assert.Throws<TimeoutException>(() => ret = RunAssertTimeout(() => {
                    TaskEx.Delay(int.MaxValue).Wait();
                    return true;
                }, ContextSwitch));
            }

            [Test]
            public static void RuntAssertTimeout_Function_ExceptionIsCaught()
            {
                bool ret;
                Assert.Throws<InvalidOperationException>(() => ret = RunAssertTimeout<bool>(
                    () => throw new InvalidOperationException()));
            }


            [Test]
            public static void WaitAssertTimeout_Task() =>
                TaskEx.Run(() => logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod())).Wait();

            [Test]
            public static void WaitAssertTimeout_Task_ExceptionIsCaught() =>
                Assert.Throws<InvalidOperationException>(() => TaskEx.Run(() =>
                    throw new InvalidOperationException()).WaitAssertTimeout());

            [Test]
            public static void WaitAssertTimeout_Task_TimeoutException() => Assert.Throws<TimeoutException>(() =>
                TaskEx.Run(() => TaskEx.Delay(int.MaxValue).Wait()).WaitAssertTimeout(ContextSwitch));

    */
    }
}
