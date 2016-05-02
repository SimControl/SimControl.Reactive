// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.TestUtils
{
    /// <summary>Abstract base class for test adapters.</summary>
    /// <remarks>TestAdapters are (if registered properly) automatically destroyed in the test cleanup methods.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public abstract class TestAdapter : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="TestAdapter"/> class.</summary>
        protected TestAdapter() { }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected abstract void Dispose(bool disposing);
    }
}
