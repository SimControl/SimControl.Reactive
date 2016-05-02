// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.Contracts;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for automatically disposing <see cref="IDisposable"/> objects.</summary>
    public class DisposableTestAdapter<TDisposable> : TestAdapter where TDisposable : class, IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="CancellationTokenTimeoutTestAdapter"/> class.</summary>
        public DisposableTestAdapter(TDisposable disposable)
        {
            Contract.Requires(disposable != null);

            Disposable = disposable;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing && Disposable != null)
                try
                { Disposable.Dispose(); }
                finally { Disposable = null; }
        }

        /// <summary>Gets the disposable object.</summary>
        /// <value>The disposable object.</value>
        public TDisposable Disposable { get; private set; }
    }
}
