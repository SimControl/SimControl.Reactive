// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using NUnit.Framework;
using SimControl.Logging;
using SimControl.TestUtils;

// TODO CR

namespace SimControl.Reactive.Tests;

[Log]
[TestFixture]
public class Lamp2Sample : TestFrame
{
    [Test]
    public static void Lamp2_OnOff()
    {
        using (Lamp2 lamp2 = new())
        {
            lamp2.On();
            lamp2.Off();
            lamp2.Fault("Error");

            Assert.That(lamp2.Counter, Is.EqualTo(1));
        }
    }
}
