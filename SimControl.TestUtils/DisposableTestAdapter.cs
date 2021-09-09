// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for automatically disposing an <see cref="IDisposable"/> object.</summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <seealso cref="TestAdapter"/>
    public class DisposableTestAdapter<TDisposable>: TestAdapter where TDisposable : class, IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="DisposableTestAdapter{TDisposable}"/> class.</summary>
        /// <param name="disposable">The disposable.</param>
        public DisposableTestAdapter(TDisposable disposable) => Disposable = disposable;

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing && Disposable is not null)
                Disposable.Dispose();
        }

        /// <summary>Gets the disposable object.</summary>
        /// <value>The disposable object.</value>
        public TDisposable Disposable { get; }
    }
}
