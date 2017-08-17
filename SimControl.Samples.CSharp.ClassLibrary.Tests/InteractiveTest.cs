// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Windows.Forms;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class InteractiveTest: TestFrame
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        [Test, InteractiveTest, ExclusivelyUses(nameof(InteractiveTest))]
        public static void InteractiveTest_DisplayMessageBoxAndThenContinue() => RunAssertTimeout((Action) (() => MessageBox.Show("InteractiveTest - Press OK")), DefaultInteractiveTestTimeout);
    }
}
