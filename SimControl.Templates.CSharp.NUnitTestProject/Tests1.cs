﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NLog;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Templates.CSharp.NUnitTestProject
{
    [Log, TestFixture]
    public class Tests1 : TestFrame
    {
        #region Test

        [SetUp]
        public void Setup() => logger.Trace("Setup");

        [TearDown]
        public new void TearDown() => logger.Trace("TearDown");

        #endregion

        [Test]
        public void Test1() => Assert.Pass();

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}