// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ServiceModel;
using System.Threading;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Reactive;
using SimControl.Samples.CSharp.Wcf.Client.SampleServiceTap;
using SimControl.Samples.CSharp.Wcf.ServiceContract;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.Wcf.Service.Tests
{
    //    [Log]
    //    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    //    public class SampleServiceTapCallback: ISampleServiceCallback
    //    {
    //        public CompositeType Callback(CompositeType compositeType) => compositeType.Increment();

    //        public void OneWayCallback(CompositeType compositeType) { }
    //    }

    [Log]
[TestFixture]
public class SampleServiceTapPerSessionTests: TestFrame
{
        [Test]
        public void SomeTest() { }
        //        //private ITtuSpServiceCallback       ttuSpCallback = Substitute.For<ITtuSpServiceCallback>();
        //        //    ttuSpCallback.When( x => x.UiConnectionCountChanged( Arg.Any<int>() )                       ).Do( x => { uiConnectionCountChanged = x.Arg<int>();                       uiConnectionCountChangedReady.Set(); } );
        //        //private AutoResetEvent              uiConnectionCountChangedReady = new AutoResetEvent( false );

        //        //[TestInitialize]
        //        //        public new void TestInitialize()
        //        //        {
        //        //            base.TestInitialize();
        //        //            baseAddress = new Uri("http://localhost:8733/Design_Time_Addresses/" + typeof(SampleServiceTapPerSessionTests).FullName + "/" + Process.GetCurrentProcess().Id.ToString());
        //        //
        //        //            client = new SampleServiceClient(new System.ServiceModel.InstanceContext(new SampleServiceTapCallback()), typeof(SampleServiceClient).FullName, new EndpointAddress(baseAddress));
        //        //            ((WSDualHttpBinding) client.Endpoint.Binding).ClientBaseAddress = new Uri("http://localhost:8733/Design_Time_Addresses/"  + typeof(SampleServiceTapPerSessionTests).FullName + "/Callback/" + Process.GetCurrentProcess().Id.ToString());
        //        //        }
        //        //
        //        //        [TestCleanup]
        //        //        public new void TestCleanup()
        //        //        {
        //        //            if (host != null)
        //        //                host.Close();
        //        //
        //        //            base.TestCleanup();
        //        //        }
        //        //
        //        //        [Test, Category(TestCategories.UnitTest)]
        //        //        public void SampleService_Connect__should__execute_without_exception()
        //        //        {
        //        //            InitializeServiceHost();
        //        //
        //        //            client.Connect();
        //        //
        //        //            client.Close();
        //        //        }
        //        //
        //        //        [Test, Category(TestCategories.UnitTest)]
        //        //        public async Task SampleService_ConnectAsync__should__execute_without_exception()
        //        //        {
        //        //            InitializeServiceHost();
        //        //
        //        //            await client.ConnectAsync();
        //        //
        //        //            client.Close();
        //        //        }
        //        //
        //        //        [Test, Category(TestCategories.UnitTest)]
        //        //        public async Task SampleService_OperationAsync__should__execute_without_exception()
        //        //        {
        //        //            InitializeServiceHost();
        //        //
        //        //            await client.ConnectAsync();
        //        //            await client.OperationAsync(OperationMode.Valid);
        //        //
        //        //            client.Close();
        //        //        }
        //        //
        //        //        [Test, Category(TestCategories.UnitTest)]
        //        //        public async Task SampleService_GetDataUsingDataContractAsync__should__execute_without_exception()
        //        //        {
        //        //            InitializeServiceHost();
        //        //
        //        //            await client.ConnectAsync();
        //        //            CompositeType result = await client.InvokeCallbackAsync(new CompositeType { IntValue = 1 });
        //        //            Assert.AreEqual(3, result.IntValue);
        //        //
        //        //            client.Close();
        //        //        }
        //        //
        //        //[Test, Category(TestCategories.UnitTest)]
        //        //public async Task SampleService_GetDataUsingDataContractOnewayAsync__should__execute_without_exception()
        //        //{
        //        //    InitializeServiceHost();

        //        //    await client.ConnectAsync();
        //        //    await client.InvokeCallbackOneWay(new CompositeType { IntValue = 1 });

        //        //    client.Close();
        //        //}

        //        //private void InitializeServiceHost()
        //        //{
        //        //    host = new ServiceHost(typeof(SampleServicePerSessionInstance), new Uri(TestBaseAddress));

        //        //    host.Faulted += ServiceHostFaulted;

        //        //    host.Open();
        //        //}

        //        //private void SampleServiceDisposed(object sender, EventArgs e) { disposed.Set(); }

        //        //private void ServiceHostFaulted(object sender, EventArgs e)
        //        //{
        //        //    UnhandledException(new InvalidOperationException());
        //        //}

        //        //private void WaitUntilServiceDisposed()
        //        //{
        //        //    if (!disposed.WaitOne(4000))
        //        //        throw new InvalidOperationException("Service not Disposed");
        //        //}

        //        //private SampleServiceClient client;
        //        //[System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S2930:\"IDisposables\" should be disposed", Justification = "<Pending>")]
        //        //private readonly AutoResetEvent disposed = new AutoResetEvent(false);

        //        //[System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S2930:\"IDisposables\" should be disposed", Justification = "<Pending>")]
        //        //private ServiceHost host;
    }
}
