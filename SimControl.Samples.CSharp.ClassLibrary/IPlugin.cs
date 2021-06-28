// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO

using System;
using System.Diagnostics.Contracts;

namespace SimControl.Samples.CSharp.Mef.Contracts
{
    /// <summary>MEF sample plugin interface contract.</summary>
    [ContractClass(typeof(PluginInterfaceContract))]
    public interface IPlugin
    {
        /// <summary>Get the resource name</summary>
        /// <returns></returns>
        string ResourceName();
    }

    [ContractClassFor(typeof(IPlugin))]
    internal abstract class PluginInterfaceContract: IPlugin
    {
        /// <inheritdoc/>
        public string ResourceName()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            throw new InvalidOperationException();
        }
    }
}
