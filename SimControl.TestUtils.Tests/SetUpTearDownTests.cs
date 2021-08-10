// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    /// <summary>
    /// Verify that <see cref="OneTimeSetUp"/>, <see cref="SetUp"/>, <see cref="TestMethod"/>, <see cref="TearDown"/>
    /// and <see cref="OneTimeTearDown"/> are invoked in the defined order.
    /// </summary>
    [Log, TestFixture]
    public class SetUpTearDownTests: TestFrame
    {
        #region Test SetUpTearDown

        [OneTimeSetUp]
        public new void OneTimeSetUp() => Assert.That(++count, Is.EqualTo(1));

        [OneTimeTearDown]
        public new void OneTimeTearDown() => Assert.That(++count, Is.EqualTo(5));

        [SetUp]
        public new void SetUp() => Assert.That(++count, Is.EqualTo(2));

        [TearDown]
        public new void TearDown() => Assert.That(++count, Is.EqualTo(4));

        #endregion

        [Test]
        public void TestMethod() => Assert.That(++count, Is.EqualTo(3));

        private int count;
    }
}
