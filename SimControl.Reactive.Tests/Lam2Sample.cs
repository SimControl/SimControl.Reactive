// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Reactive.Tests
{
    [Log]
    [TestFixture]
    public class Lam2Sample: TestFrame
    {
        //[Test]
        public static void Lamp2_OnOff()
        {
            using (var lamp2 = new Lamp2())
            {
                lamp2.On();
                lamp2.Off();
                lamp2.Fault("Error");

                Assert.That(lamp2.Counter, Is.EqualTo(1));
            }
        }
    }
}
