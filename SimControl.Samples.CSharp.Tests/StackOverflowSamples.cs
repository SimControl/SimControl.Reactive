// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

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
}
