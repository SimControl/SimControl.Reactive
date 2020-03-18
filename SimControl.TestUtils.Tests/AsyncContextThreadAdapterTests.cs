// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

#if !NET472 //UNDONE ProjectReference/SetTargetFramework 

using System.Threading;
using System.Threading.Tasks;
using NCrunch.Framework;
using Nito.AsyncEx;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log]
    [TestFixture]
    public class AsyncContextThreadAdapterTests: TestFrame
    {
        [Test, ExclusivelyUses(FileName)]
        public static void CopyFileTestAdapter__create_and_Dispose__file_is_created_and_deleted()
        {
            using (var acta = new AsyncContextThreadAdapter())
            using (var cts = new CancellationTokenSource())
            {
                var ready = new AutoResetEvent(false);

                Task task = acta.Factory.Run(() => {
#if NET40
                    TaskEx.Delay(-1, cts.Token);
#else
                    Task.Delay(-1, cts.Token);
#endif
                    ready.Set();
                });

                cts.Cancel();

                ready.WaitOneAssertTimeout();
                task.WaitAssertTimeout();
            }
        }

        public const string FileName = "CopyFileTestAdapterTests.tmp";
    }
}

#endif
