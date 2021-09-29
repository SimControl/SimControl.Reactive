// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimControl.TestUtils
{
    /// <summary>Provides a thread context test adapter for asynchronous operations.</summary>
    /// <seealso cref="TestAdapter"/>
    public class AsyncContextTestAdapter: TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="AsyncContextTestAdapter"/> class.</summary>
        public AsyncContextTestAdapter(string name) : this(TestFrame.Timeout) { }

        public AsyncContextTestAdapter(object o, string name, ApartmentState state) : this(TestFrame.Timeout) { }

        /// <summary>Initializes a new instance of the <see cref="AsyncContextTestAdapter"/> class.</summary>
        /// <param name="timeout">The timeout.</param>
        public AsyncContextTestAdapter(int timeout) { }// => this.timeout = timeout;

        public Task SendAsync(Action action) => Task.CompletedTask;

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            //if (disposing && asyncContextThread != null)
            //{
            //    asyncContextThread.JoinAsync().WaitAssertTimeout(timeout);
            //    asyncContextThread.Dispose();
            //    asyncContextThread = null;
            //}
        }

        /// <summary>Gets the <see cref="TaskFactory"/>.</summary>
        /// <value>The task factory.</value>
        //public TaskFactory Factory => asyncContextThread.Factory;

        //private AsyncContextThread asyncContextThread = new AsyncContextThread();
    }
}
