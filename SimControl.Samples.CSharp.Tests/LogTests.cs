// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;
using SimControl.Log;

#if OLD_PROJECT_TYPE
namespace SimControl.Samples.CSharp.TestsOld
#else
namespace SimControl.Samples.CSharp.Tests
#endif
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
