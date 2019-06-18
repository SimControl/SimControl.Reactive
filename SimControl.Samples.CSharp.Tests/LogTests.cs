// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;
using SimControl.Log;

namespace SimControl.Templates.CSharp.Tests
{
    [Log, TestFixture]
    public class LogTests
    {
#if NETFRAMEWORK
        [Test]
        public void ClassLibraryOld_LogClass__Foo()
        {
            new ClassLibraryOld.LogClass().VoidMethod();
        }
#endif

#if (NET472 || NETCOREAPP)
        [Test]
        public void LogClass_Foo()
        {
            new ClassLibraryOld.LogClass().VoidMethod();
        }
#endif
    }
}
