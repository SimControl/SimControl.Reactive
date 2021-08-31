// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

#if !NET5_0

using System.Threading.Tasks;
using System.Windows.Forms;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Tests
{
    [Log]
    [TestFixture]
    public class InteractiveTest: TestFrame
    {
        [Test, InteractiveTest, ExclusivelyUses(nameof(InteractiveTest))]
        public static void InteractiveTest__DisplayMessageBox__ContinueAfterClickYes() => Task.Run(() =>
            Assert.That(MessageBox.Show("Press Yes", "InteractiveTest", MessageBoxButtons.YesNo),
                Is.EqualTo(DialogResult.Yes))).AssertTimeoutAsync(InteractiveTimeout).Wait();
    }
}

#endif
