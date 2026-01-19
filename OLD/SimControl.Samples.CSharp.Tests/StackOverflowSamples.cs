// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests;

public static class LongTimeOperation
{
    static LongTimeOperation()
    {
        Thread.Sleep(1);
        Resource = (++count).ToString();
    }

    public static string Resource { get; set; }
    private static int count = 0;
}

public class BarObject
{
    public string info { get; set; }
    public string name { get; set; }
}

public abstract class BaseFixture
{
    protected BaseFixture(string device)
    {
        TestManager.SetDevice(device);

        this.device = device;
    }

    [Test]
    public void ShouldA() => Assert.That(TestManager.Device, Is.EqualTo(device));

    private string device;
}

[TestFixture]
public class DesktopTest: BaseFixture
{
    public DesktopTest() : base("desktop") { }

    [Test]
    public void ShouldAdesktop() => Assert.That(TestManager.Device, Is.EqualTo("desktop"));
}

public class Foo
{
    public Foo()
    { }

    public IEnumerable<BarObject> Bars { get; set; } =
        new List<BarObject>() { new BarObject() { name = "johndoe", info = "wierdo" } };
}

[TestFixture]
public class FooTests
{
    [Test]
    public void TestDefaultInList()
    {
        Foo foo = new Foo();

        Assert.That(foo.Bars.GetEnumerator().Current,
            Is.EqualTo(new BarObject() { name = "johndoe", info = "wierdo" }));

        Assert.That(foo.Bars, Is.EquivalentTo(new[] { new BarObject() { name = "johndoe", info = "wierdo" } }));
    }
}

[TestFixture]
public class MobileTest: BaseFixture
{
    public MobileTest() : base("mobile") { }

    [Test]
    public void ShouldAmobile() => Assert.That(TestManager.Device, Is.EqualTo("mobile"));
}

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

[TestFixture]
public class TestFixture1
{
    [OneTimeSetUp]
    public void OneTimeSetUp() => resource = LongTimeOperation.Resource;

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

public class TestManager
{
    public static void SetDevice(string device) => Device = device;

    public static string Device { get; private set; }
}
