// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel.Composition;
using SimControl.Log;
using SimControl.Samples.CSharp.Mef.Contracts;

namespace SimControl.Samples.CSharp.ClassLibrary.Component
{
    /// <summary>Autofac sample component.</summary>
    [Log]
    public class Component1 : IComponent
    {
        /// <summary>Initializes a new instance of the <see cref="Component1"/> class.</summary>
        /// <param name="element">The element.</param>
        public Component1(IElement element) { this.element = element; }

        /// <summary>Get the resource Name</summary>
        /// <returns>Resource name</returns>
        public string ElementName() => element.ElementName;

        private readonly IElement element;
    }
}
