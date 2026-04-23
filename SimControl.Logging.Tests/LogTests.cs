// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using NLog;
using NUnit.Framework;
using SimControl.TestUtils;
using System.Globalization;

// TODO implement

namespace SimControl.Logging.Tests;

[Log]
[TestFixture]
public class LogTests : TestFrame
{
    [Test]
    public static void LogAttribute_Tests()
    {
        using (TestClass testClass = new())
        {
            _ = testClass.Foo(456, "jkl");
            _ = testClass.Foo2(456, "jkl");
            _ = TestClass.StaticFoo(789, "mno");
        }
    }

    [Test]
    public static void LambdaExpressionLogging_Tests()
    {
        int x = Foo(1);

        static int func(int i) => i + 1;

        _ = func(x);
    }

    private static int Foo(int i) => i + 1;

    [Test]
    public static void LogFormat_LongArray_MaxCollectionElementsAreFormatted() => logger.Message(LogLevel.Info,
            LogMethod.GetCurrentMethodName(),
            "Message",
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });

    [Test]
    public void LogFormat_Tests()
    {
        logger.Entry(LogLevel.Debug, LogMethod.GetCurrentMethodName(), this);
        logger.Entry(LogLevel.Debug, LogMethod.GetCurrentMethodName(), this, 1);
        logger.Entry(LogLevel.Debug, LogMethod.GetCurrentMethodName(), this, 1, 2, 3);
        logger.Entry(LogLevel.Debug, LogMethod.GetCurrentMethodName(), this, new object[] { 1, 2, 3 });

        logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName());
        logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "Message1");
        logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), "Message2", 123);
        logger.Message(LogLevel.Debug, LogMethod.GetCurrentMethodName(), "Message3", 1, 2, 3);

        logger.Exception(LogLevel.Trace, LogMethod.GetCurrentMethodName(), this, new InvalidOperationException());

        logger.Exit(LogLevel.Debug, LogMethod.GetCurrentMethodName(), this, 123);
    }

    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
}

[Log(LogLevel = LogAttributeLevel.Trace)]
public class TestClass : IDisposable
{
    ~TestClass() => Dispose(false);

    [LogExcludeAttribute]
    public static string StaticFoo(int par1, string par2) => par1.ToString(CultureInfo.InvariantCulture) + par2;

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    public string Foo(int par1, string par2) =>
        par1.ToString(CultureInfo.InvariantCulture) + par2 + member2.ToString(CultureInfo.CurrentCulture);

    [LogExcludeAttribute]
    public string Foo2(int par1, string par2) =>
        par1.ToString(CultureInfo.InvariantCulture) + par2 + member2.ToString(CultureInfo.CurrentCulture);

    public override string ToString() => LogFormat.FormatObject(typeof(TestClass), member1, member2, member3, member4, member5, staticMember);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && autoResetEvent != null)
        {
            autoResetEvent.Dispose();
            autoResetEvent = null;
        }
    }

    private const int member1 = 123;
    private const double member4 = 12345.6789;
    private const int staticMember = 0;
    private readonly string member2 = "abc";

    private readonly string[] member3 = { "def", "ghi" };
    private readonly DateTime member5 = DateTime.Now;
    private AutoResetEvent autoResetEvent = new(false);
}
