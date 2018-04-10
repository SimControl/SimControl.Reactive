// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Diagnostics.Contracts;
using System.Globalization;
using SimControl.Log;

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
        public string ElementName =>
            typeof(Element).Name + "." + count.ToString(CultureInfo.InvariantCulture) + "." +name;

        private readonly int count;
        private readonly string name;
    }
}
