// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    [Log, TestFixture]
    public class DisposableTestAdapterTests: TestFrame
    {
        [Test]
        public void DisposableTestAdapter__CreateAndDispose__succeeds()
        {
            using var disposableTestAdapter = new DisposableTestAdapter<SemaphoreSlim>(new SemaphoreSlim(0));
            disposableTestAdapter.Disposable.Release();
            disposableTestAdapter.Disposable.WaitAsync().AssertTimeoutAsync().Wait();
        }
    }
}
