// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Tests
{
    [Log, TestFixture]
    public class SetUpTearDownTests: TestFrame
    {
        #region Test SetUpTearDown

        [OneTimeSetUp]
        public new void OneTimeSetUp() => count = 1;

        [OneTimeTearDown] // should be invoked regardless of exception during SetUp()
        public new void OneTimeTearDown() => Assert.That(++count, Is.EqualTo(5));

        [SetUp]
        public new void SetUp() => Assert.That(++count, Is.EqualTo(2));

        [TearDown] // should be invoked regardless of exception during SetUp()
        public new void TearDown() => Assert.That(++count, Is.EqualTo(4));

        #endregion

        [Test]
        public void Increment() => Assert.That(++count, Is.EqualTo(3));

        private int count = 0;
    }
}
