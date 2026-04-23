// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using NUnit.Framework;
using SimControl.Logging;

namespace SimControl.TestUtils.Tests;

[Log, TestFixture/*, ExclusivelyUses(FileName)*/]
public class TempFilesTestAdapterTests : TestFrame
{
    [Test] // Create_and_dispose__file_is_copied_and_deleted
    public static void Conctructor__create_temp_file_create__temp_file_is_deleted()
    {
        string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        using CopyFileTestAdapter copyFileTestAdapter = new("NLog.config", FileName);
        Assert.That(File.Exists(fullPath));

        using TempFilesTestAdapter tempFileTestAdapter = new(FileName, "tempfile2");
        Assert.That(!File.Exists(fullPath));
    }

    [Test]
    public static void Dispose__create_temp_file_create__temp_file_is_deleted()
    {
        string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        TestAdapter copyFileTestAdapter;

        using (TempFilesTestAdapter tempFileTestAdapter = new(FileName, "tempfile2"))
        {
            copyFileTestAdapter = new CopyFileTestAdapter("NLog.config", FileName);
            Assert.That(File.Exists(fullPath));
        }
        Assert.That(!File.Exists(fullPath));

        copyFileTestAdapter.Dispose();
    }

    public const string FileName = "TempFilesTestAdapterTests.tmp";
}
