// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

// UNDONE delete AsyncQueue

namespace SimControl.TestUtils
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncQueue<T>: System.IDisposable
    {
        /// <summary>Dequeues the asynchronous.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// </returns>
        public async Task<T> DequeueAsync(CancellationToken cancellationToken = default)
        {
            for (; ; )
            {
                await sem.WaitAsync(cancellationToken);

                //if (queue.TryDequeue(out T item))
                //    return item;
                queue.TryDequeue(out T item);
                return item;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Enqueues the specified item.</summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            queue.Enqueue(item);
            sem.Release();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (sem != null)
            {
                sem.Dispose();
                sem = null;
            }
        }

        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        private SemaphoreSlim sem = new SemaphoreSlim(0);
    }
}
