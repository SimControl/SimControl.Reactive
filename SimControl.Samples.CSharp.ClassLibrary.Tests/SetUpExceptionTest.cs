// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;
using SimControl.LogEx;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Tests
{
    [Log]
    [TestFixture]
    public class SetUpExceptionTest: TestFrame
    {
        #region Test

        [OneTimeSetUp]
        public static new void OneTimeSetUp()
        {
        }

        [OneTimeTearDown]
        public static new void OneTimeTearDown() => throw new InvalidOperationException();

        [SetUp]
        public static new void SetUp() => throw new InvalidOperationException();

        [TearDown]
        public static new void TearDown() => throw new InvalidOperationException();

        #endregion

        [Test, Ignore("Will fail by design"), Unstable]
        public static void ClassInitializeException_TestMethodNotInvoked_TestCleanupInvoked() { }
    }
}
