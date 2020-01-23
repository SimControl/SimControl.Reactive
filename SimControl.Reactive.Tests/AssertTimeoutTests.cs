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
    public class AssertTimeoutTests : TestFrame
    {
        [Test]
        public static void JoinAssertTimeout_Thread()
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
            var thread = new Thread(() => Task.Delay(int.MaxValue).Wait());
            thread.Start();

            _ = Assert.Throws<TimeoutException>(() => thread.JoinAssertTimeout(MinTimerResolution));
        }

        [Test]
        public static void ResultAssertTimeout_Function()
        {
            bool ret = RunAssertTimeout(() => true);

            Assert.IsTrue(ret);
        }

        [Test]
        public static void ResultAssertTimeout_Task()
        {
            bool ret = Task.Run(() => true).ResultAssertTimeout();

            Assert.IsTrue(ret);
        }

        [Test]
        public static void ResultAssertTimeout_Task_ExceptionIsCaught() =>
            Assert.Throws<InvalidOperationException>(() => Task<bool>.Factory.StartNew(() =>
                throw new InvalidOperationException()).ResultAssertTimeout());

        [Test]
        public static void ResultAssertTimeout_Task_TimeoutException() =>
            Assert.Throws<TimeoutException>(() => Task.Run(() => {
                Task.Delay(int.MaxValue).Wait();
                return true;
            }).ResultAssertTimeout(MinTimerResolution));

        [Test]
        public static void RunAction_ExceptionIsCaught() =>
            Assert.Throws<InvalidOperationException>(() => RunAssertTimeout(() =>
                throw new InvalidOperationException()));

        [Test]
        public static void RunAssertTimeout_Action() => RunAssertTimeout(() =>
            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod()));

        [Test]
        public static void RunAssertTimeout_Action_TimeoutException() => Assert.Throws<TimeoutException>(() =>
            RunAssertTimeout(() => Task.Delay(int.MaxValue).Wait(), MinTimerResolution));

        [Test]
        public static void RunAssertTimeout_Function_TimeoutException()
        {
            bool ret;

            _ = Assert.Throws<TimeoutException>(() => ret = RunAssertTimeout(() => {
                Task.Delay(int.MaxValue).Wait();
                return true;
            }, MinTimerResolution));
        }

        [Test]
        public static void RuntAssertTimeout_Function_ExceptionIsCaught()
        {
            bool ret;
            _ = Assert.Throws<InvalidOperationException>(() => ret = RunAssertTimeout<bool>(
                () => throw new InvalidOperationException()));
        }

        [Test]
        public static void TakeAssertTimeout_BlockingCollection()
        {
            using (var blockingCollection = new BlockingCollection<bool>())
            {
                var task = Task.Run(() => blockingCollection.Add(true));

                Assert.IsTrue(blockingCollection.TakeAssertTimeout());

                task.WaitAssertTimeout();
            }
        }

        [Test]
        public static void TakeAssertTimeout_BlockingCollection_TimeoutException()
        {
            using (var blockingCollection = new BlockingCollection<bool>())
                _ = Assert.Throws<TimeoutException>(
                    () => Assert.IsTrue(blockingCollection.TakeAssertTimeout(MinTimerResolution)));
        }

        [Test]
        public static void WaitAssertTimeout_Task() =>
            Task.Run(() => logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod())).Wait();

        [Test]
        public static void WaitAssertTimeout_Task_ExceptionIsCaught() =>
            Assert.Throws<InvalidOperationException>(() => Task.Run(() =>
                throw new InvalidOperationException()).WaitAssertTimeout());

        [Test]
        public static void WaitAssertTimeout_Task_TimeoutException() => Assert.Throws<TimeoutException>(() =>
            Task.Run(() => Task.Delay(int.MaxValue).Wait()).WaitAssertTimeout(MinTimerResolution));

        [Test]
        public static void WaitOneAssertTimeout_WaitHandle()
        {
            using (var ready = new AutoResetEvent(false))
            {
                Task task = Task.Run(() => ready.Set());

                ready.WaitOneAssertTimeout();
                task.WaitAssertTimeout();
            }
        }

        [Test]
        public static void WaitOneAssertTimeout_WaitHandle_TimeoutException()
        {
            using (var ready = new AutoResetEvent(false))
                _ = Assert.Throws<TimeoutException>(() => ready.WaitOneAssertTimeout(MinTimerResolution));
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
