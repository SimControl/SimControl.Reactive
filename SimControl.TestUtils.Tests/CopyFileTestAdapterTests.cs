// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.IO;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture]
    public class CopyFileTestAdapterTests: TestFrame
    {
        [Test, ExclusivelyUses(FileName)]
        public static void Create_and_dispose__file_is_copied_and_deleted()
        {
            string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

            if (File.Exists(fullPath)) File.Delete(fullPath);

            using (var copyFileTestAdapter = new CopyFileTestAdapter("NLog.config", FileName))
                Assert.That(File.Exists(fullPath));

            Assert.That(!File.Exists(fullPath));
        }

        public const string FileName = "CopyFileTestAdapterTests.tmp";
    }
}
