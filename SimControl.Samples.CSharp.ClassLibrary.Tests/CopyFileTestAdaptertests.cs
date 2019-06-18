// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.LogEx;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Tests
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
