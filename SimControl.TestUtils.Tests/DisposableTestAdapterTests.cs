﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log]
    [TestFixture]
    public class DisposableTestAdapterTests: TestFrame
    {
        [Test]
        public void DisposableTestAdapter__CreateAndDispose__succeeds()
        {
            var disposableTestAdapter = new DisposableTestAdapter<AutoResetEvent>(new AutoResetEvent(false));

            disposableTestAdapter.Disposable.Set();
            disposableTestAdapter.Disposable.WaitOneAssertTimeout();

            disposableTestAdapter.Dispose();
        }
    }
}
