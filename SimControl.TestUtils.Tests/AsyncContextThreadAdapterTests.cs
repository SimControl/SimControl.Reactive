using NUnit.Framework;
using SimControl.Log;

// UNDONE AsyncContextThreadAdapter test

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture]
    public class AsyncContextThreadAdapterTests: TestFrame
    {
        [Test]
        public static void CopyFileTestAdapter__create_and_Dispose__file_is_created_and_deleted()
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
