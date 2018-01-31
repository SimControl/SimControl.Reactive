// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

namespace SimControl.Samples.CSharp.Mef.Contracts
{
    /// <summary>
    /// MEF sample resource interface contract.
    /// </summary>
    public interface IResource
    {
        /// <inheritdoc />
        string ResourceName { get; }
    }
}
