// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using System;

using SimControl.Logging;

namespace SimControl.Templates.CSharp.WcfServiceLibrary
{
    /// <summary>.</summary>
    /// <seealso cref="T:SimControl.Templates.CSharp.WcfServiceLibrary.IService1"/>
    [Log]
    public class Service1 : IService1
    {
        /// <inheritdoc/>
        public int GetData(int value) => value + 1;

        /// <inheritdoc/>
        public CompositeType GetDataUsingDataContract(CompositeType composite) =>
            composite ?? throw new ArgumentNullException(nameof(composite));
    }
}
