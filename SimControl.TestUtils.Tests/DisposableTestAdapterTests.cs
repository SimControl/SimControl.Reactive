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
        public void Create_and_dispose_with_SemaphoreSlim__succeeds()
        {
            using var disposableTestAdapter = new DisposableTestAdapter<SemaphoreSlim>(new SemaphoreSlim(0, 1));
            disposableTestAdapter.Disposable.Release();
            disposableTestAdapter.Disposable.WaitAsync().AssertTimeoutAsync().Wait();
        }
    }
}
