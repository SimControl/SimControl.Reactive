// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Reactive.Tests
{
    [Log]
    [TestFixture]
    public class LamDSample: TestFrame
    {
        [Test, Example]
        public void LampD_OnOff()
        {
            using (var lampd = new LampD())
            {
                lampd.On();
                lampd.Off();
                lampd.Fault("Error");

                Assert.That(lampd.Counter, Is.EqualTo(1));
            }
        }
    }
}
