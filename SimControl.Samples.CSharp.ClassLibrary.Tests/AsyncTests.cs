// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Reactive;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class AsyncTests: TestFrame
    {
        #region Test

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SetUp]
        new public void SetUp()
        {
            UnhandledExceptionEvent += UnhandledException;
            unhandledException = null;

            unhandledExceptionEvent = RegisterTestAdapter(
                new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;
        }

        [TearDown]
        new public void TearDown()
        {
            UnhandledExceptionEvent -= UnhandledException;
        }

        #endregion

        [Test, Example]
        public void AsyncTests_AsyncAutoResetEvent_SetInTaskOnOtherThread_IsSignaled()
        {
            var ready = new AsyncAutoResetEvent(false);

            Task task = TaskEx.Run(async () => {
                await TaskEx.Delay(MinTimerResolution).ConfigureAwait(false);
                ready.Set();
            });

            ready.WaitAsync().WaitAssertTimeout();

            task.WaitAssertTimeout();
        }

        [Test]
        public async Task AsyncTests_AsyncTest_awaitTastExDelay_Async()
        {
            await TaskEx.Delay(MinTimerResolution).ConfigureAwait(false);
            logger.Info("Delay finished");
        }

        [Test, Example]
        public void AsyncTests_AutoResetEvent_SetInTaskOnOtherThread_IsSignaled()
        {
            using (AutoResetEvent ready = new AutoResetEvent(false))
            {
                Task task = TaskEx.Run(async () => {
                    await TaskEx.Delay(MinTimerResolution).ConfigureAwait(false);
                    ready.Set();
                });

                ready.WaitOneAssertTimeout();
                task.WaitAssertTimeout();
            }
        }

        [Test]
        public void AsyncTests_Exception_ThrownInTaskOnOtherThread_IsCaughtInWait()
        {
            Assert.That(Assert.Catch<AggregateException>(() => TaskEx.Run(() => {
                TaskEx.Delay(MinTimerResolution).Wait();
                throw new InvalidOperationException();
            }).Wait()).InnerExceptions[0], Is.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void AsyncTests_Exception_ThrownInTaskOnOtherThread_IsCaughtOnAwait()
        {
            Assert.Catch<InvalidOperationException>(() => TestHelperAsync().WaitAssertTimeout());
        }

        [Test, Example]
        public void AsyncTests_ObserveTaskException()
        {
            using (AutoResetEvent ready = new AutoResetEvent(false))
            {
                Task task = TaskEx.Run(async () => {
                    await TaskEx.Delay(MinTimerResolution).ConfigureAwait(false);
                    ready.Set();
                    throw new InvalidOperationException();
                }).ContinueWith(t => logger.Exception(LogLevel.Error, MethodBase.GetCurrentMethod(), null,
                    t.Exception.InnerException), TaskContinuationOptions.OnlyOnFaulted);

                ready.WaitOneAssertTimeout();
                task.WaitAssertTimeout();
            }
        }

        [Test]
        public async Task AsyncTests_SynchronizationContextIsNull_Async()
        {
            await TaskEx.Delay(0).ConfigureAwait(false);

            Assert.That(SynchronizationContext.Current, Is.Null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect")]
        [Test, Ignore("Unstable"), Unstable]
        public void AsyncTests_UnhandledAsyncExceptions_TaskSchedulerUnobservedTaskException()
        {
            TaskEx.Run(() => { throw new InvalidOperationException(); });

            TaskEx.Delay(1000).Wait();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            unhandledExceptionEvent.WaitOneAssertTimeout();

            ClearUnhandledException();
        }

        [Test]
        public void AsyncTests_UnhandledAsyncExceptions_UnexpectedExceptionHandler()
        {
            TaskEx.Run(() => {
                try
                {
                    throw new InvalidOperationException("Some exception");
                }
                catch (InvalidOperationException e)
                {
                    SetUnhandledException(e);
                }
            }).WaitAssertTimeout();

            unhandledExceptionEvent.WaitOneAssertTimeout();

            Assert.That(unhandledException.Message, Is.EqualTo("Some exception"));
            ClearUnhandledException();
        }

        private async Task TestHelperAsync()
        {
            await TaskEx.Run(async () => {
                await TaskEx.Delay(MinTimerResolution).ConfigureAwait(false);
                throw new InvalidOperationException();
            }).ConfigureAwait(false);
            logger.Info("TaskEx.Run finished");
        }

        private void UnhandledException(object sender, EventArgs<Exception> args)
        {
            unhandledExceptionEvent.Set();
            unhandledException = args;
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Exception unhandledException;
        private AutoResetEvent unhandledExceptionEvent;
    }
}
