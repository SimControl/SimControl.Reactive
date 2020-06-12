// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Runtime.Serialization;
using System.ServiceModel;

namespace SimControl.Templates.CSharp.WcfServiceLibrary
{
    /// <summary>Interface for .</summary>
    [ServiceContract]
    public interface IService1
    {
        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        int GetData(int value);

        /// <summary></summary>
        /// <param name="composite"></param>
        /// <returns></returns>
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

        /// <summary></summary>
        /// <value></value>
        [DataMember]
        public string StringValue { get; set; } = "Hello ";
    }
}
