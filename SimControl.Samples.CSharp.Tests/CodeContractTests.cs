// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NUnit.Framework;
using SimControl.Log;

namespace CodeContractTests.Tests
{
    public static class ContractClass
    {
        public static string Method(string s)
        {
            // Contract.Requires(s != null);
            // Contract.Requires(s.Length == 0);

            return s + "123";
        }
    }

    [Log, TestFixture]
    public class LogTests
    {
        [Test]
        public void InvokeMethod_with_empty_string() => ContractClass.Method("");

        [Test]
        public void InvokeMethod_with_null() => ContractClass.Method(null);

        [Test]
        public void InvokeMethod_with_string() => ContractClass.Method("abc");
    }
}
