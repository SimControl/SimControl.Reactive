// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class TestSamples: TestFrame
    {
        private class PayRecord
        {
            public double Gross;
        }

        [Test]
        public static void PayRecordTest()
        {
            var _records = new List<PayRecord>() {
                new PayRecord { Gross = 652 },
                new PayRecord { Gross = 418 },
                new PayRecord { Gross = 2202 },
                new PayRecord { Gross = 1104 },
                new PayRecord { Gross = 1797.45 }
            };

            Assert.That(_records.Select(x => x.Gross), Is.EqualTo(new double[] { 652, 418, 2202, 1104, 1797.45 }));
            Assert.AreEqual(_records.Select(x => x.Gross), new double[] { 652, 418, 2202, 1104, 1797.45 });
        }
    }
}
