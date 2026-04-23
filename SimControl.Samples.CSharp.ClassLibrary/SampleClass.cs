// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

using NLog;
using SimControl.Logging;

namespace SimControl.Samples.CSharp.ClassLibrary;

/// <summary>SampleClass implementation.</summary>
[Log]
public class SampleClass
{
    static SampleClass() => logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(),
        typeof(SampleClass).AssemblyQualifiedName);

    /// <summary>Increment the static counter</summary>
    public static void IncrementStaticCounter() => StaticCounter++;

    /// <summary>Does something</summary>
    /// <returns></returns>
    public bool DoSomething()
    {
        logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), nameof(DoSomething));

        counter++;

        return true;
    }

    /// <inheritdoc/>
    public override string ToString() => LogFormat.FormatObject(typeof(SampleClass), StaticCounter, counter);

    /// <summary>Get the static counter</summary>
    public static int StaticCounter { get; private set; }

    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private int counter;
}
