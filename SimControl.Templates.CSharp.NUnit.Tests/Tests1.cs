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
            // TODO implement
        }

        [TearDown]
        public new void TearDown()
        {
            // TODO implement
        }

        #endregion

        [Test]
        public void MethodName__state_under_test__expected_behavior()
        {
            // TODO Arrange - Act - Assert

            LogManager.GetCurrentClassLogger().Trace("Some Test");

            Assert.Pass();
        }
    }
}
