// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class SetUpExceptionTest: TestFrame
    {
        #region Test

        [OneTimeSetUp]
        new public static void OneTimeSetUp()
        {
        }

        [OneTimeTearDown]
        new public static void OneTimeTearDown() => throw new InvalidOperationException();

        [SetUp]
        new public static void SetUp() => throw new InvalidOperationException();

        [TearDown]
        new public static void TearDown() => throw new InvalidOperationException();

        #endregion

        [Test, Ignore("Will fail by design"), Unstable]
        public static void ClassInitializeException_TestMethodNotInvoked_TestCleanupInvoked() { }
    }
}
