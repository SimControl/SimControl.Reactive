﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using SimControl.Log;

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
