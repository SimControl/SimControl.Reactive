﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Globalization;
using NUnit.Framework;
using SimControl.TestUtils;

namespace SimControl.Log.Tests
{
    [Log]
    [TestFixture]
    public class InternationalCultureInfoTests: TestFrame
    {
        [Test]
        public static void LogAttribute_Tests2()
        {
            var germanCultureInfo = new CultureInfo("de-AT", false);

            InternationalCultureInfo.SetCurrentThreadCulture(germanCultureInfo, germanCultureInfo);

            try { throw new InvalidOperationException(); }
            catch (InvalidOperationException e)
            {
                Assert.That(e.Message, Is.EqualTo(
                    "Der Vorgang ist aufgrund des aktuellen Zustands des Objekts ungültig."));
            }

            InternationalCultureInfo.SetCurrentThreadCulture();
            InternationalCultureInfo.SetDefaultThreadCulture();

            try { throw new InvalidOperationException(); }
            catch (InvalidOperationException e)
            {
                Assert.That(e.Message, Is.EqualTo("Operation is not valid due to the current state of the object."));
            }
        }
    }
}
