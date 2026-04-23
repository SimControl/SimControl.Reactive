// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using NUnit.Framework;
using SimControl.Logging;
using SimControl.TestUtils;
using System.Threading.Channels;

namespace SimControl.Templates.CSharp.Tests;

[Log, TestFixture]
public class TemplateTests : TestFrame
{
    [Test]
    public static void ClassLibrary_Class1__invoke_constructor__succeeds() =>
        Assert.That(new ClassLibrary.Class1().ToString(), Is.Not.Null);

    //#if !NET5_0 // TODO ConsoleApp tests for net5.0

    [Test, IntegrationTest/*, ExclusivelyUses(ProcessName)*/]
    public static void ConsoleApp__start_process__exits_with_0()
    {
        ProcessTestAdapter.KillProcesses(ProcessName);

        using ProcessTestAdapter process = new(ProcessName, "", out ChannelReader<string> standardOutput,
            out _);

        standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("MainAssembly"))
            .AssertTimeoutAsync().Wait();

        process.Process?.StandardInput.Close();

        standardOutput.ReadUntilAssertTimeoutAsync(s => s.Contains("Exit"))
            .AssertTimeoutAsync().Wait();

        Assert.That(process.WaitForExitAssertTimeout(), Is.EqualTo(0));
    }

    //#endif
    // TODO SimControl.Templates.CSharp.WcfServiceLibrary tests

    public const string ProcessName = "SimControl.Templates.CSharp.ConsoleApp";
}
