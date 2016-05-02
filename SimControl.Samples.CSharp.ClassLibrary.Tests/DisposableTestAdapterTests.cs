// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class DisposableTestAdapterTests: TestFrame
    {
        #region Test

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SetUp]
        new public void SetUp()
        {
            autoResetEvent = RegisterTestAdapter(
                new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false))).Disposable;
        }

        #endregion

        [Test]
        public void DisposableTestAdapter_createAutoResetEvent_succeds()
        {
            autoResetEvent.Set();
            autoResetEvent.WaitOneAssertTimeout();
        }

        private AutoResetEvent autoResetEvent;
    }
}
