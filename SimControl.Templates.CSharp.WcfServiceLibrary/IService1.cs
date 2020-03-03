// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Runtime.Serialization;
using System.ServiceModel;

namespace SimControl.Templates.CSharp.WcfServiceLibrary
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        int GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public bool BoolValue { get; set; } = true;

        [DataMember]
        public string StringValue { get; set; } = "Hello ";
    }
}
