// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    /// <summary>Test whether teardown methods are ivoked regardless of previous errors.</summary>
    /// <remarks>Test output must be checked manually, as test will always fail.</remarks>
    [Log, TestFixture]
    public class SetUpExceptionTest: TestFrame
    {
        #region Test SetUp/TearDown

        [OneTimeSetUp]
        public static new void OneTimeSetUp() { }

        [OneTimeTearDown] // should be invoked regardless of exception during SetUp()
        public static new void OneTimeTearDown() { }

        [SetUp]
        public static new void SetUp() => throw new InvalidOperationException();

        [TearDown] // should be invoked regardless of exception during SetUp()
        public static new void TearDown() => throw new InvalidOperationException();

        #endregion

        [Test, Ignore("Will always fail by design, check output manually")]
        public static void ClassInitializeException_TestMethodNotInvoked_TestCleanupInvoked() { }
    }
}
