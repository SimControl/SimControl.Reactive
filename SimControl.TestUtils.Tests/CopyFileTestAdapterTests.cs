// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using NUnit.Framework;
using SimControl.Logging;

namespace SimControl.TestUtils.Tests;

[Log, TestFixture]
public class CopyFileTestAdapterTests : TestFrame
{
    [Test/*, ExclusivelyUses(FileName)*/]
    public static void Create_and_dispose__file_is_copied_and_deleted()
    {
        string fullPath = TestContext.CurrentContext.TestDirectory + "\\" + FileName;

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        using (CopyFileTestAdapter copyFileTestAdapter = new("NLog.config", FileName))
            Assert.That(File.Exists(fullPath));

        Assert.That(!File.Exists(fullPath));
    }

    public const string FileName = "CopyFileTestAdapterTests.tmp";
}
