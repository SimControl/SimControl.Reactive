// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Runtime.Serialization;
using System.ServiceModel;

namespace SimControl.Samples.CSharp.WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both
    // code and config file together.
    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations. You can add
    // XSD files into the project. After building the project, you can directly use the data types defined there, with
    // the namespace "SimControl.Samples.CSharp.WcfServiceLibrary.ContractType".
    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public bool BoolValue { get; set; } = true;

        [DataMember]
        public string StringValue { get; set; } = "Hello ";
    }
}
