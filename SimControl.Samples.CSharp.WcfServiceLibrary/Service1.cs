// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO CR

using System;

namespace SimControl.Samples.CSharp.WcfServiceLibrary
{
    /// <summary>A service for accessing samples information.</summary>
    public class SampleService: ISampleService
    {
        /// <inheritdoc/>
        public string GetData(int value) => string.Format("You entered: {0}", value);

        /// <inheritdoc/>
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
