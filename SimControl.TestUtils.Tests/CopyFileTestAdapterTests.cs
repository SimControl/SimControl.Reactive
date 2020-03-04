// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.IO;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.TestUtils.Tests
{
    [Log]
    [TestFixture]
    public class CopyFileTestAdapterTests: TestFrame
    {
        [Test, ExclusivelyUses(FileName)]
        public static void CopyFileTestAdapter__CreateAndDispose__FileIsCreatedAndDeleted()
        {
            string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

            if (File.Exists(fullPath)) File.Delete(fullPath);

            var copyFileTestAdapter = new CopyFileTestAdapter("NLog.config", FileName);

            Assert.That(File.Exists(fullPath));

            copyFileTestAdapter.Dispose();

            Assert.That(!File.Exists(fullPath));
        }

        public const string FileName = "CopyFileTestAdapterTests.tmp";
    }
}
