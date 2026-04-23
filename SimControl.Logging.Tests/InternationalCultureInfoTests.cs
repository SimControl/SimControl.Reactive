// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using NUnit.Framework;
using SimControl.TestUtils;
using System.Globalization;

namespace SimControl.Logging.Tests;

[Log]
[TestFixture]
public class InternationalCultureInfoTests : TestFrame
{
#if !NET5_0 // TODO fix InternationalCultureInfo.SetCurrentThreadCulture for net5.0

    [Test/*, Isolated*/]
    public static void SetCurrentThreadCulture()
    {
        CultureInfo germanCultureInfo = new("de-AT", false);

        InternationalCultureInfo.SetCurrentThreadCulture(germanCultureInfo, germanCultureInfo);

        try
        { throw new InvalidOperationException(); }
        catch (InvalidOperationException e)
        {
            Assert.That(e.Message, Is.EqualTo(
                "Der Vorgang ist aufgrund des aktuellen Zustands des Objekts ungültig."));
        }

        InternationalCultureInfo.SetCurrentThreadCulture();

        try
        { throw new InvalidOperationException(); }
        catch (InvalidOperationException e)
        {
            Assert.That(e.Message, Is.EqualTo("Operation is not valid due to the current state of the object."));
        }

        using (StreamWriter writer = new("filePath"))
        {
            Console.WriteLine("");
        }

        using StreamWriter writer2 = new("filePath");
        Console.WriteLine("");
    }

#endif

    [Test/*, Isolated*/]
    public static void SetDefaultThreadCulture_german()
    {
        CultureInfo germanCultureInfo = new("de-AT", false);

        InternationalCultureInfo.SetDefaultThreadCulture(germanCultureInfo, germanCultureInfo);

        // TODO start thread and validate thread culture
    }

    [Test/*, Isolated*/]
    public static void SetDefaultThreadCulture_InternationalCultureInfo() => InternationalCultureInfo.SetDefaultThreadCulture(
            InternationalCultureInfo.Instance, InternationalCultureInfo.Instance);// TODO start thread and validate thread culture

    [Test/*, Isolated*/]
    public static void SetDefaultThreadCulture_without_args() => InternationalCultureInfo.SetDefaultThreadCulture();// TODO start thread and validate thread culture
}
