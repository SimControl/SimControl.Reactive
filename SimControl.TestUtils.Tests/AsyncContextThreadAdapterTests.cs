// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;

// UNDONE AsyncContextThreadAdapter test

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture]
    public class AsyncContextThreadAdapterTests: TestFrame
    {
        [Test]
        public static void Xxx()
        {
            //using (var acta = new AsyncContextThreadAdapter())
            //{
            //    var ready = new AutoResetEvent(false);

            //    Task task = acta.Factory.Run(() => { ContextSwitch(); ready.Set(); });

            //    ready.WaitOneAssertTimeout();
            //    task.WaitAssertTimeout();
            //}
        }
    }
}
