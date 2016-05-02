// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

#if EXPERIMENTAL
using System;
using System.Threading;
using NLog;
using SimControl.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimControl.Reactive.Tests
{
    [TestFixture, LogAttribute(typeof(SchedulerTests), AttributeTargetMembers = "Test*")]
    public class SchedulerTests
    {
        [Test]
        public void TestDispose1()
        {
            Counter counter = new Counter("counter", 1);
            Scheduler scheduler = new Scheduler("scheduler", counter);

            counter.Start();
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 100));
            counter.Stop();

            Assert.IsTrue(counter.Value > 1);

            counter.Delete();
            scheduler.Delete();
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 100));

            AssertAllObjectsDisposed();
            AssertNoExcpetions();
        }

        [Test]
        public void TestDispose2()
        {
            Counter counter = new Counter("counter", 1);
            Scheduler scheduler = new Scheduler("scheduler", counter);

            counter.Start();
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 100));
            counter.Stop();

            Assert.IsTrue(counter.Value > 1);

            scheduler.Delete();
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 100));

            AssertAllObjectsDisposed();
            AssertNoExcpetions();
        }

        private static void AssertAllObjectsDisposed() { Assert.IsTrue(ActiveObject.ActiveObjectCount == 0); Assert.IsTrue(Scheduler.SchedulerCount == 0); }
        private static void AssertNoExcpetions() { Assert.IsTrue(Scheduler.ExceptionCount == 0); }
    }
}

#endif
