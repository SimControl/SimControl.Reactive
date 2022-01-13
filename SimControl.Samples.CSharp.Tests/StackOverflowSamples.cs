// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class StackOverflowSamples: TestFrame
    {
        [TestCase("ears")]
        [TestCase("eyes", Ignore = "Bug is JIRA #FOO", Until = "2099-02-15")]
        [TestCase("nose")]
        [TestCase("mouth")]
        [TestCase("touch")]
        public async Task CanUseSense(string sense)
        {
            Assert.That(sense, Is.Not.EqualTo("eyes"));
        }
    }

    public static class LongTimeOperation
    {
        static LongTimeOperation()
        {
            Thread.Sleep(10000);
            Resource = (++count).ToString();
        }

        public static string Resource { get; set; }
        private static int count = 0;
    }

    [TestFixture]
    public class TestFixture1
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            resource = LongTimeOperation.Resource;
        }

        [Test]
        public void Test1() => Assert.That(resource, Is.EqualTo("1"));

        [Test]
        public void Test2() => Assert.That(resource, Is.EqualTo("1"));

        private string resource;
    }

    [TestFixture]
    public class TestFixture2
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            resource = LongTimeOperation.Resource;
        }

        [Test]
        public void Test1() => Assert.That(resource, Is.EqualTo("1"));

        [Test]
        public void Test2() => Assert.That(resource, Is.EqualTo("1"));

        private string resource;
    }
}
