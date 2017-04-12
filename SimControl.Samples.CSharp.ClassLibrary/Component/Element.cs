// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SimControl.Log;
using SimControl.Samples.CSharp.Mef.Contracts;

namespace SimControl.Samples.CSharp.ClassLibrary.Component
{
    /// <summary>Autofac sample element.</summary>
    [Log]
    public class Element : IElement
    {
        /// <summary>Initializes a new instance of the <see cref="Element"/> class.</summary>
        /// <param name="counter">The counter.</param>
        /// <param name="name">The name.</param>
        public Element(Counter counter, string name)
        {
            Contract.Requires(counter != null);
            Contract.Requires(name != null);

            count = counter.Increment();
            this.name = name;
        }

        /// <summary>Get the element name</summary>
        public string ElementName => typeof(Element).Name + "." + count + "." + name;

        private readonly int count;
        private readonly string name;
    }
}
