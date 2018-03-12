// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;
using SimControl.Log;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class CopyFileTestAdapterTests : TestFrame
    {
        #region Test

        [SetUp]
        public new void SetUp() => RegisterTestAdapter(new CopyFileTestAdapter("NLog.config", "NLog2.config"));

        #endregion

        [Test]
        public static void CopyFileTestAdapter() { }
    }
}
