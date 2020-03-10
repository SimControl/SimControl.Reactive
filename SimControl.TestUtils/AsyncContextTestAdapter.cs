// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading.Tasks;
using Nito.AsyncEx;

namespace SimControl.TestUtils
{
    /// <summary>Provides a thread context test adapter for asynchronous operations.</summary>
    /// <seealso cref="TestAdapter"/>
    public class AsyncContextThreadAdapter: TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="AsyncContextThreadAdapter"/> class.</summary>
        public AsyncContextThreadAdapter() : this(TestFrame.DefaultTestTimeout) { }

        /// <summary>Initializes a new instance of the <see cref="AsyncContextThreadAdapter"/> class.</summary>
        /// <param name="timeout">The timeout.</param>
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

        /// <summary>Gets the <see cref="TaskFactory"/>.</summary>
        /// <value>The task factory.</value>
        public TaskFactory Factory => asyncContextThread.Factory;

        private readonly int timeout;
        private AsyncContextThread asyncContextThread = new AsyncContextThread();
    }
}
