// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Diagnostics.Contracts;
using System.IO;
using NUnit.Framework;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for automatically deleting temporary files.</summary>
    /// <seealso cref="TestAdapter"/>
    public class TempFilesTestAdapter: TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="TempFilesTestAdapter"/> class.</summary>
        /// <remarks>The temporary files will be automatically deleted before and after test execution.</remarks>
        /// <param name="tempFiles">The temporary files.</param>
        public TempFilesTestAdapter(params string[] tempFiles)
        {
            Contract.Requires(Contract.ForAll(tempFiles, x => !string.IsNullOrEmpty(x)));

            this.tempFiles = tempFiles;
            DeleteTempFiles();
        }

        /// <summary>Deletes the temporary files.</summary>
        public void DeleteTempFiles()
        {
            foreach (string file in tempFiles)
            {
                string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + file;

                if (File.Exists(fullPath)) File.Delete(fullPath);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                DeleteTempFiles();
        }

        private readonly string[] tempFiles;
    }
}
