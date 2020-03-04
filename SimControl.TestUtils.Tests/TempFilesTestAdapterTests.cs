// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.IO;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log]
    [TestFixture, ExclusivelyUses(FileName)]
    public class TempFilesTestAdapterTests: TestFrame
    {
        [Test]
        public static void CreateTempFile__Create_TempFilesTestAdapter__TempFileIsDeleted()
        {
            string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

            if (File.Exists(fullPath)) File.Delete(fullPath);

            var copyFileTestAdapter = new CopyFileTestAdapter("NLog.config", FileName);
            Assert.That(File.Exists(fullPath));

            TestAdapter tempFileTestAdapter = new TempFilesTestAdapter(FileName, "tempfile2");

            Assert.That(!File.Exists(fullPath));

            tempFileTestAdapter.Dispose();
            copyFileTestAdapter.Dispose();
        }

        [Test]
        public static void CreateTempFile__Dispose_TempFilesTestAdapter__TempFileIsDeleted()
        {
            string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

            if (File.Exists(fullPath)) File.Delete(fullPath);

            var tempFileTestAdapter = new TempFilesTestAdapter(FileName, "tempfile2");

            TestAdapter copyFileTestAdapter = new CopyFileTestAdapter("NLog.config", FileName);
            Assert.That(File.Exists(fullPath));

            tempFileTestAdapter.Dispose();

            Assert.That(!File.Exists(fullPath));

            copyFileTestAdapter.Dispose();
        }

        public const string FileName = "TempFilesTestAdapterTests.tmp";
    }
}
