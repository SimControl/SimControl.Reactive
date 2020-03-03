// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Tests
{
    [Log]
    [TestFixture]
    public class DisposableTestAdapterTests: TestFrame
    {
        #region Test

        [SetUp]
        public new void SetUp() => autoResetEvent = RegisterTestAdapter(
                new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;

        #endregion

        [Test]
        public void DisposableTestAdapter__createAutoResetEvent__succeeds()
        {
            autoResetEvent.Set();
            autoResetEvent.WaitOneAssertTimeout();
        }

        private AutoResetEvent autoResetEvent;
    }
}
