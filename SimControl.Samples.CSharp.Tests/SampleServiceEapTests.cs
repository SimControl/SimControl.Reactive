// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

//using System;
//using System.Diagnostics;
//using System.ServiceModel;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SimControl.Log;
//using SimControl.Samples.CSharp.Wcf.Client.SampleServiceEap;

//namespace SimControl.Samples.CSharp.Wcf.Service.Tests
//{
//    [Log]
//    [TestFixture]
//    public class SampleServiceEapPerSessionInstanceWSDualHttpBindingTests: SampleServiceEapBaseTests
//    {
//        #region Additional test attributes

//        [TestInitialize]
//        public new void TestInitialize()
//        {
//            base.TestInitialize();

//            TestBaseAddress = "http://localhost:8733/Design_Time_Addresses/" + Guid.NewGuid();

//            client = new SampleServiceClient(new InstanceContext(new SampleServiceEapCallback()),
//                typeof(SampleServiceClient).FullName, new EndpointAddress(TestBaseAddress));
//            ((WSDualHttpBinding) client.Endpoint.Binding).ClientBaseAddress = new Uri(TestBaseAddress + "/Callback");
//        }

//        #endregion

//        protected override void InitializeServiceHost()
//        {
//            host = new ServiceHost(typeof(SampleServicePerSessionInstance), new Uri(TestBaseAddress));

//            host.Faulted += ServiceHostFaulted;

//            host.Open();
//        }

//        private string TestBaseAddress;
//    }

//    [Log]
//    [TestFixture]
//    public class SampleServiceEapSingleInstanceWSDualHttpBindingTests: SampleServiceEapBaseTests
//    {
//        #region Additional test attributes

//        [TestInitialize]
//        public new void TestInitialize()
//        {
//            base.TestInitialize();

//            TestBaseAddress = "http://localhost:8733/Design_Time_Addresses/" + Guid.NewGuid();

//            client = new SampleServiceClient(new InstanceContext(new SampleServiceEapCallback()),
//                typeof(SampleServiceClient).FullName, new EndpointAddress(TestBaseAddress));
//            ((WSDualHttpBinding) client.Endpoint.Binding).ClientBaseAddress = new Uri(TestBaseAddress + "/Callback");
//        }

//        #endregion

//        protected override void InitializeServiceHost()
//        {
//            host = new ServiceHost(typeof(SampleServiceSingleInstance), new Uri(TestBaseAddress));

//            host.Faulted += ServiceHostFaulted;

//            host.Open();
//        }

//        protected string TestBaseAddress;
//    }
//}
