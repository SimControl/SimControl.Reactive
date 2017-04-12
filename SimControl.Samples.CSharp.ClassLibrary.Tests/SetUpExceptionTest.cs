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
        new public void OneTimeSetUp()
        {
        }

        [OneTimeTearDown]
        new public void OneTimeTearDown() => throw new InvalidOperationException();

        [SetUp]
        new public void SetUp() => throw new InvalidOperationException();

        [TearDown]
        new public static void TearDown() => throw new InvalidOperationException();

        #endregion

        [Test, Ignore("Will fail by design"), Unstable]
        public void ClassInitializeException_TestMethodNotInvoked_TestCleanupInvoked() { }
    }
}
