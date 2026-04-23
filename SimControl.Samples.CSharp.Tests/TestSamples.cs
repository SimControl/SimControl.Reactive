// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using NUnit.Framework;
using SimControl.Logging;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests;

[Log]
[TestFixture]
public class TestSamples : TestFrame
{
    private class PayRecord
    {
        public double Gross;
    }

    [Test]
    public static void PayRecordTest()
    {
        List<PayRecord> _records = [
            new PayRecord { Gross = 652 },
            new PayRecord { Gross = 418 },
            new PayRecord { Gross = 2202 },
            new PayRecord { Gross = 1104 },
            new PayRecord { Gross = 1797.45 }
        ];

        Assert.That(_records.Select(x => x.Gross), Is.EqualTo(new double[] { 652, 418, 2202, 1104, 1797.45 }));
        Assert.AreEqual(_records.Select(x => x.Gross), new double[] { 652, 418, 2202, 1104, 1797.45 });
    }
}
