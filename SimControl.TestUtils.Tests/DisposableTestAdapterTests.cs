// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

using NUnit.Framework;
using SimControl.Logging;

namespace SimControl.TestUtils.Tests;

[Log, TestFixture]
public class DisposableTestAdapterTests : TestFrame
{
    [Test]
    public void Create_and_dispose_with_SemaphoreSlim__succeeds()
    {
        using DisposableTestAdapter<SemaphoreSlim> disposableTestAdapter = new(new SemaphoreSlim(0, 1));
        _ = disposableTestAdapter.Disposable.Release();
        disposableTestAdapter.Disposable.WaitAsync().AssertTimeoutAsync().Wait();
    }
}
