// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Diagnostics.Contracts;
using System.IO;
using NUnit.Framework;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for copying files.</summary>
    /// <seealso cref="TestAdapter"/>
    public class CopyFileTestAdapter: TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="CopyFileTestAdapter"/> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public CopyFileTestAdapter(string source, string target)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));
            Contract.Requires(!string.IsNullOrEmpty(target));

            destination = TestContext.CurrentContext.TestDirectory + "\\" + target;

            File.Copy(TestContext.CurrentContext.TestDirectory + "\\" + source, destination, true);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing && File.Exists(destination))
                File.Delete(destination);
        }

        private readonly string destination;
    }
}
