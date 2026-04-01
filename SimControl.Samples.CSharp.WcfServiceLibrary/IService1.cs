// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Runtime.Serialization;
using System.ServiceModel;

// TODO implement

namespace SimControl.Samples.CSharp.WcfServiceLibrary
{
    /// <summary>Interface for sample service.</summary>
    [ServiceContract]
    public interface ISampleService
    {
        /// <inheritdoc/>
        [OperationContract]
        string GetData(int value);

        /// <inheritdoc/>
        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    /// <summary>A composite type.</summary>
    [DataContract]
    public class CompositeType
    {
        /// <summary></summary>
        /// <value></value>
        [DataMember]
        public bool BoolValue { get; set; } = true;

        /// <summary>Gets or sets the string value.</summary>
        /// <value>The string value.</value>
        [DataMember]
        public string StringValue { get; set; } = "Hello ";
    }
}
