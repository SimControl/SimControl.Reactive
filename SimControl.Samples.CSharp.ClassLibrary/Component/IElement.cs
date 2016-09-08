// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.Samples.CSharp.ClassLibrary.Component
{
    /// <summary>MEF sample resource interface contract.</summary>
    public interface IElement
    {
        /// <summary>Gets the name of the element.</summary>
        /// <value>The name of the element.</value>
        string ElementName { get; }
    }
}
