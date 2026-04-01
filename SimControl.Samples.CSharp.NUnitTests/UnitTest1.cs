using NLog;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.NUnitTests
{
    [Log, TestFixture]
    public class Tests : TestFrame
    {
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

        [Test]
        public void MethodName__state_under_test__expected_behavior()
        {
            // TODO Arrange - Act - Assert

            LogManager.GetCurrentClassLogger().Trace("Some Test");
            Console.WriteLine("Some Test");

            Assert.Pass();
        }
    }
}
