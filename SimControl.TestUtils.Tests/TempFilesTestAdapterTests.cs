// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.IO;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture, ExclusivelyUses(FileName)]
    public class TempFilesTestAdapterTests: TestFrame
    {
        [Test] // Create_and_dispose__file_is_copied_and_deleted
        public static void Conctructor__create_temp_file_create__temp_file_is_deleted()
        {
            string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

            if (File.Exists(fullPath)) File.Delete(fullPath);

            using var copyFileTestAdapter = new CopyFileTestAdapter("NLog.config", FileName);
            Assert.That(File.Exists(fullPath));

            using var tempFileTestAdapter = new TempFilesTestAdapter(FileName, "tempfile2");
            Assert.That(!File.Exists(fullPath));
        }

        [Test]
        public static void Dispose__create_temp_file_create__temp_file_is_deleted()
        {
            string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

            if (File.Exists(fullPath)) File.Delete(fullPath);

            TestAdapter copyFileTestAdapter;

            using (var tempFileTestAdapter = new TempFilesTestAdapter(FileName, "tempfile2"))
            {
                copyFileTestAdapter = new CopyFileTestAdapter("NLog.config", FileName);
                Assert.That(File.Exists(fullPath));
            }
            Assert.That(!File.Exists(fullPath));

            copyFileTestAdapter.Dispose();
        }

        public const string FileName = "TempFilesTestAdapterTests.tmp";
    }
}
