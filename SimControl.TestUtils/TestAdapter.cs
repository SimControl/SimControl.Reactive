// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.TestUtils
{
    /// <summary>Abstract base class for test adapters.</summary>
    /// <remarks>TestAdapters are (if registered properly) automatically destroyed in the test cleanup methods.</remarks>
    /// <seealso cref="IDisposable"/>
    public abstract class TestAdapter: IDisposable
    {
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to
        /// release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);
    }
}
