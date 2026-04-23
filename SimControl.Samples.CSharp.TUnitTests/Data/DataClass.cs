// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using TUnit.Core.Interfaces;

namespace SimControl.Samples.CSharp.Tests;

public class DataClass : IAsyncInitializer, IAsyncDisposable
{
    public Task InitializeAsync() => Console.Out.WriteLineAsync("Classes can be injected into tests, and they can perform some initialisation logic such as starting an in-memory server or a test container.");

    public async ValueTask DisposeAsync() => await Console.Out.WriteLineAsync("And when the class is finished with, we can clean up any resources.");
}
