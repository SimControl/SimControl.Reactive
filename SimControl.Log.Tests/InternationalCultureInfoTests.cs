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
    public class InternationalCultureInfoTests: TestFrame
    {
        [Test]
        public static void LogAttribute_Tests()
        {
            try { throw new InvalidOperationException(); }
            catch (InvalidOperationException e)
            {
                logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), e);

                Assert.That(e.Message, Is.EqualTo("Operation is not valid due to the current state of the object."));
            }
        }

        [Test]
        public static void LogAttribute_Tests2()
        {
            var cultureInfo = new CultureInfo( "de-AT", false );
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            try { throw new InvalidOperationException(); }
            catch (InvalidOperationException e)
            {
                logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), e);

                Assert.That(e.Message, Is.EqualTo("Der Vorgang ist aufgrund des aktuellen Zustands des Objekts ungültig."));
            }

            InternationalCultureInfo.SetCurrentThreadCulture();
            //LogMethod.SetDefaultThreadCulture();

            try { throw new InvalidOperationException(); }
            catch (InvalidOperationException e)
            {
                logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), e);

                Assert.That(e.Message, Is.EqualTo("Operation is not valid due to the current state of the object."));
            }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
