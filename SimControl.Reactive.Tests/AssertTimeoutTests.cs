// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Reactive.Tests
{
    [Log]
    [TestFixture]
    public class AssertTimeoutTests: TestFrame
    {
        [Test]
        public void JoinAssertTimeout_Thread()
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
        public void JoinAssertTimeout_Thread_TimeoutException()
        {
            var thread = new Thread(() => TaskEx.Delay(int.MaxValue).Wait());
            thread.Start();

            Assert.Throws<TimeoutException>(() => thread.JoinAssertTimeout(MinTimerResolution));
        }

        [Test]
        public void ResultAssertTimeout_Function()
        {
            bool ret = RunAssertTimeout(() => true);

            Assert.IsTrue(ret);
        }

        [Test]
        public void ResultAssertTimeout_Task()
        {
            bool ret = TaskEx.Run(() => true).AssertTimeout();

            Assert.IsTrue(ret);
        }

        [Test]
        public void ResultAssertTimeout_Task_ExceptionIsCaught() => Assert.Throws<InvalidOperationException>(() => Task<bool>.Factory.StartNew(
                                                                      () => { throw new InvalidOperationException(); }).AssertTimeout());

        [Test]
        public void ResultAssertTimeout_Task_TimeoutException() => Assert.Throws<TimeoutException>(() => TaskEx.Run(() =>
        {
            TaskEx.Delay(int.MaxValue).Wait();
            return true;
        }).AssertTimeout(MinTimerResolution));

        [Test]
        public void RunAction_ExceptionIsCaught() => Assert.Throws<InvalidOperationException>(() => RunAssertTimeout(
                                                       () => { throw new InvalidOperationException(); }));

        [Test]
        public void RunAssertTimeout_Action() => RunAssertTimeout(() => logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod()));

        [Test]
        public void RunAssertTimeout_Action_TimeoutException() => Assert.Throws<TimeoutException>(
            () => RunAssertTimeout(() => TaskEx.Delay(int.MaxValue).Wait(), MinTimerResolution));

        [Test]
        public void RunAssertTimeout_Function_TimeoutException()
        {
            bool ret;

            Assert.Throws<TimeoutException>(() => ret = RunAssertTimeout(() => {
                TaskEx.Delay(int.MaxValue).Wait();
                return true;
            }, MinTimerResolution));
        }

        [Test]
        public void RuntAssertTimeout_Function_ExceptionIsCaught()
        {
            bool ret;
            Assert.Throws<InvalidOperationException>(() => ret = RunAssertTimeout<bool>(
                () => { throw new InvalidOperationException(); }));
        }

        [Test]
        public void TakeAssertTimeout_BlockingCollection()
        {
            using (var blockingCollection = new BlockingCollection<bool>())
            {
                Task task = TaskEx.Run(() => blockingCollection.Add(true));

                Assert.IsTrue(blockingCollection.TakeAssertTimeout());

                task.AssertTimeout();
            }
        }

        [Test]
        public void TakeAssertTimeout_BlockingCollection_TimeoutException()
        {
            using (var blockingCollection = new BlockingCollection<bool>())
                Assert.Throws<TimeoutException>(
                    () => Assert.IsTrue(blockingCollection.TakeAssertTimeout(MinTimerResolution)));
        }

        [Test]
        public void WaitAssertTimeout_Task() => TaskEx.Run(() => logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod())).Wait();

        [Test]
        public void WaitAssertTimeout_Task_ExceptionIsCaught() => Assert.Throws<InvalidOperationException>(() => TaskEx.Run(
                                                                    () => { throw new InvalidOperationException(); }).AssertTimeout());

        [Test]
        public void WaitAssertTimeout_Task_TimeoutException() => Assert.Throws<TimeoutException>(() => TaskEx.Run(
                                                                   () => TaskEx.Delay(int.MaxValue).Wait()).AssertTimeout(MinTimerResolution));

        [Test]
        public void WaitOneAssertTimeout_WaitHandle()
        {
            using (var ready = new AutoResetEvent(false))
            {
                Task task = TaskEx.Run(() => ready.Set());

                ready.WaitOneAssertTimeout();
                task.AssertTimeout();
            }
        }

        [Test]
        public void WaitOneAssertTimeout_WaitHandle_TimeoutException()
        {
            using (var ready = new AutoResetEvent(false))
                Assert.Throws<TimeoutException>(() => ready.WaitOneAssertTimeout(MinTimerResolution));
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
