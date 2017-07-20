// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Reactive.Tests
{
    [Log]
    [TestFixture]
    public class LogTests: TestFrame
    {
        [Test]
        public void LogAttribute_Tests()
        {
            using (var testClass = new TestClass())
            {
                testClass.Foo(456, "jkl");
                testClass.Foo2(456, "jkl");
                TestClass.StaticFoo(789, "mno");
            }
        }

        [Test]
        public void LambdaExpressionLogging_Tests()
        {
            int x = Foo(1);

            Func<int, int> func = i => i + 1;

            func(x);
        }

        private int Foo(int i) => i + 1;

        [Test]
        public void LogFormat_LongArray_MaxCollectionElementsAreFormatted() => logger.Message(LogLevel.Info,
                MethodBase.GetCurrentMethod(),
                "Message",
                new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });

        [Test]
        public void LogFormat_Tests()
        {
            logger.Entry(LogLevel.Debug, MethodBase.GetCurrentMethod(), this);
            logger.Entry(LogLevel.Debug, MethodBase.GetCurrentMethod(), this, 1);
            logger.Entry(LogLevel.Debug, MethodBase.GetCurrentMethod(), this, 1, 2, 3);
            logger.Entry(LogLevel.Debug, MethodBase.GetCurrentMethod(), this, new object[] { 1, 2, 3 });

            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod());
            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "Message1");
            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), "Message2", 123);
            logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "Message3", 1, 2, 3);

            logger.Exception(LogLevel.Trace, MethodBase.GetCurrentMethod(), this, new InvalidOperationException());

            logger.Exit(LogLevel.Debug, MethodBase.GetCurrentMethod(), this, 123);
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }

    [Log(LogLevel = LogAttributeLevel.Trace)]
    [Log(AttributeExclude = true, AttributePriority = 1, AttributeTargetMembers = ".cctor*")]
    public class TestClass: IDisposable
    {
        ~TestClass() => Dispose(false);

        public static string StaticFoo(int par1, string par2) => par1.ToString(CultureInfo.InvariantCulture) + par2;

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public string Foo(int par1, string par2) => par1.ToString(CultureInfo.InvariantCulture) + par2;

        [Log(AttributeExclude = true, AttributePriority = 2)]
        public string Foo2(int par1, string par2) => par1.ToString(CultureInfo.InvariantCulture) + par2;

        public override string ToString() => LogFormat.FormatObject(typeof(TestClass), member1, member2, member3, member4, member5, staticMember);

        private void Dispose(bool disposing)
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
        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);
    }
}
