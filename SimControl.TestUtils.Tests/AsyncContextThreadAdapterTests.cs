// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture]
    public class AsyncContextThreadAdapterTests: TestFrame
    {
        [Test]
        public static void CopyFileTestAdapter__create_and_Dispose__file_is_created_and_deleted()
        {
            using (var acta = new AsyncContextThreadAdapter())
            {
                var ready = new AutoResetEvent(false);

                Task task = acta.Factory.Run(() => { ContextSwitch(); _ = ready.Set(); });

                ready.WaitOneAssertTimeout();
                task.WaitAssertTimeout();
            }
        }
    }
}
