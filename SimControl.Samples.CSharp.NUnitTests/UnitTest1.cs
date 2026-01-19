using SimControl.Samples.CSharp.ClassLibrary;

namespace SimControl.Samples.CSharp.NUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Console.WriteLine("Hello from NUnit Test!");

            new SampleClass(new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory());

            Assert.Pass();
        }
    }
}
