// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Templates.CSharp.NUnitTests
{
    [Log]
    [TestFixture]
    public class Tests1: TestFrame
    {
        #region Test SetUp/TearDown

        [SetUp]
        public new void SetUp()
        {
            logger.Trace(nameof(SetUp));
            Assert.That(true);
        }

        [TearDown]
        public new void TearDown()
        {
            logger.Trace(nameof(TearDown));
            Assert.That(true);
        }

        #endregion

        [Test]
        public void Arrange__act__assert()
        {
            logger.Trace(nameof(Arrange__act__assert));
            Assert.That(true);
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
