// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using SimControl.Log;

namespace SimControl.Templates.CSharp.WcfServiceLibrary
{
    /// <summary>.</summary>
    /// <seealso cref="T:SimControl.Templates.CSharp.WcfServiceLibrary.IService1"/>
    [Log]
    public class Service1: IService1
    {
        /// <inheritdoc/>
        public int GetData(int value) => value+1;

        /// <inheritdoc/>
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null) throw new ArgumentNullException(nameof(composite));

            return composite;
        }
    }
}
