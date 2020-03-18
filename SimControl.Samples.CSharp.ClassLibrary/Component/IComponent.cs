// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.Contracts;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Component
{
    /// <summary>Autofac sample component interface contract.</summary>
    [ContractClass(typeof(PluginInterfaceContract))]
    public interface IComponent
    {
        /// <summary>Get the element name</summary>
        /// <returns></returns>
        string ElementName();
    }

    [ContractClassFor(typeof(IComponent))]
    internal abstract class PluginInterfaceContract : IComponent
    {
        /// <inheritdoc/>
        public string ElementName()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            throw new InvalidOperationException();
        }
    }
}
