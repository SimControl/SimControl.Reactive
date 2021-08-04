// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

namespace SimControl.Samples.CSharp.ClassLibraryEx.Component
{
    /// <summary>MEF sample resource interface // Contract.</summary>
    public interface IElement
    {
        /// <summary>Gets the name of the element.</summary>
        /// <value>The name of the element.</value>
        string ElementName { get; }
    }
}
