// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading.Tasks;
using Nito.AsyncEx;

namespace SimControl.TestUtils
{
    public class AsyncContextThreadAdapter: TestAdapter
    {
        public AsyncContextThreadAdapter() => timeout = TestFrame.DefaultTestTimeout;

        public AsyncContextThreadAdapter(int timeout) => this.timeout = timeout;

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing && asyncContextThread != null)
            {
                asyncContextThread.JoinAsync().WaitAssertTimeout(timeout);
                asyncContextThread.Dispose();
                asyncContextThread = null;
            }
        }

        public TaskFactory Factory => asyncContextThread.Factory;

        private readonly int timeout;
        private AsyncContextThread asyncContextThread = new AsyncContextThread();
    }
}
