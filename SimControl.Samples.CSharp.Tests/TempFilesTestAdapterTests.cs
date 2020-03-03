// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.
/*
using System.IO;
using NUnit.Framework;
using SimControl.LogEx;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Tests
{
    [Log]
    [TestFixture]
    public class TempFilesTestAdapterTests : TestFrame
    {
        #region Test

        [SetUp]
        public new void SetUp() => RegisterTestAdapter(new TempFilesTestAdapter("tempfile1", "tempfile2"));

        #endregion

        [Test]
        public static void CopyFileTestAdapter() => File.Copy(
            TestContext.CurrentContext.TestDirectory + "\\" + "NLog.config",
            TestContext.CurrentContext.TestDirectory + "\\" + "tempfile2", true);
        [Test, ExclusivelyUses(FileName)]
        public static void CopyFileTestAdapter__fileCreationAndDeletion__succeeds()
        {
            string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

            if (File.Exists(fullPath)) File.Delete(fullPath);

            TestAdapter copyFileTestAdapter = new CopyFileTestAdapter("NLog.config", FileName);

            Assert.That(File.Exists(fullPath));

            copyFileTestAdapter.Dispose();

            Assert.That(!File.Exists(fullPath));
        }

        public const string FileName = "NLog2.config";
    }
}
*/
