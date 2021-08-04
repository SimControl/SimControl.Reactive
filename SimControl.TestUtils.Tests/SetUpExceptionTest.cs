// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.TestUtils.Tests
{
    /// <summary>Test whether teardown methods are invoked regardless of previous errors.</summary>
    /// <remarks>Test output must be checked manually, as test will always fail.</remarks>
    [Log, TestFixture]
    public class SetUpExceptionTest: TestFrame
    {
        #region Test SetUp/TearDown

        [OneTimeSetUp]
        public static new void OneTimeSetUp()
        {
            state = "OneTimeSetUp";
            throw new InvalidOperationException(state);
        }

        [OneTimeTearDown] // should be invoked regardless of exception during OneTimeSetUp(), SetUp() or TearDown()
        public static new void OneTimeTearDown()
        {
            state = "OneTimeTearDown";
            throw new InvalidOperationException(state);
        }

        [SetUp]
        public static new void SetUp()
        {
            state = "SetUp";
            throw new InvalidOperationException(state);
        }

        [TearDown] // should be invoked regardless of exception during OneTimeSetUp(), SetUp()
        public static new void TearDown()
        {
            state = "TearDown";
            throw new InvalidOperationException(state);
        }

        #endregion

        [Test, Ignore("Will always fail by design, check output manually")]
        public static void ClassInitializeException_TestMethodNotInvoked_TestCleanupInvoked() => Assert.Fail();

        public override string ToString() => LogFormat.FormatObject(typeof(SetUpExceptionTest), state);

        private static string state = "Initialized";
    }
}
