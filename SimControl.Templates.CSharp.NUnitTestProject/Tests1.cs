// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Templates.CSharp.NUnitTests
{
    [Log, TestFixture]
    public class Tests1: TestFrame
    {
        #region Test SetUp/TearDown

        [SetUp]
        public new void SetUp()
        {
        }

        [TearDown]
        public new void TearDown()
        {
        }

        #endregion

        [Test]
        public void Arrange__act__assert()
        {
            LogManager.GetCurrentClassLogger().Trace("Some Test");
            Assert.That(true);
        }
    }
}
