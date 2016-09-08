// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel.Composition;
using SimControl.Log;
using SimControl.Samples.CSharp.Mef.Contracts;

namespace SimControl.Samples.CSharp.ClassLibrary.Component
{
    /// <summary>Element counter.</summary>
    [Log]
    public class Counter
    {
        /// <summary>Increments this instance.</summary>
        /// <returns></returns>
        public int Increment() => count++;

        private int count;
    }
}
