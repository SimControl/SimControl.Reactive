// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

using NUnit.Framework;
using SimControl.Logging;

namespace SimControl.TestUtils.Tests;

[Log]
[TestFixture]
public class InteractiveTest : TestFrame
{
    [Test, InteractiveTest/*, ExclusivelyUses(nameof(InteractiveTestAttribute))*/]
    public static void InteractiveTest__DisplayMessageBox__ContinueAfterClickYes() => Task.Run(() =>
        Assert.That(MessageBox.Show("Press Yes", TestContext.CurrentContext.Test.FullName, MessageBoxButtons.YesNo),
            Is.EqualTo(DialogResult.Yes))).AssertTimeoutAsync(InteractiveTimeout).Wait();
}
