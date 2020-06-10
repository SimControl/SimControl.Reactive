// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

//using System;
//using System.ServiceModel;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NLog;
//using SimControl.Log;
//using SimControl.Samples.CSharp.Wcf.Client.SampleServiceEap;
//using SimControl.Samples.CSharp.Wcf.ServiceContract;
//using SimControl.TestUtils;
//using ISampleServiceCallback = SimControl.Samples.CSharp.Wcf.Client.SampleServiceEap.ISampleServiceCallback;

//namespace SimControl.Samples.CSharp.Wcf.Service.Tests
//{
//    [Log]
//    [TestFixture]
//    public abstract class SampleServiceEapBaseTests: TestFrame
//    {
//        [Test]
//        public void ConnectClose_NoException()
//        {
//            InitializeServiceHost();

//            Task.Factory.FromAsync(client.BeginConnect, client.EndConnect, null).WaitAssertTimeout();

//            Assert.AreEqual(CommunicationState.Opened, client.State);

//            client.Close();

//            Assert.AreEqual(CommunicationState.Closed, client.State);
//        }

//        [TestCleanup]
//        public new void TestCleanup()
//        {
//            TestHandleCleanupExceptions(client.AbortOrCloseAssertTimeout);

//            if (host != null)
//                try
//                {
//                    host.Close();
//                }
//                catch (Exception e)
//                {
//                    logger.Warn(e);
//                }

//            base.TestCleanup();
//        }

//        //[Test, Category(TestCategories.UnitTest)]
//        //public void SampleService_InvokeCallback_Async__should__execute_without_exception()
//        //{
//        //    InitializeServiceHost();

//        //    client.Connect();

//        //    AutoResetEvent ready = new AutoResetEvent(false);

//        //    client.BeginInvokeCallback(new CompositeType(), ar => {
//        //        CompositeType result = client.EndInvokeCallback(ar);

//        //        Assert.AreEqual(3, result.IntValue);

//        //        ready.Set();
//        //    }, null);

//        //    ready.WaitOne();

//        //    Assert.AreEqual(CommunicationState.Opened, client.State);

//        //    client.Close();
//        //    if (perSesseion)
//        //        DisposableMonitor.Current.WaitAllInstancesDisposed();
//        //}

//        //[Test, Category(TestCategories.UnitTest)]
//        //public void SampleService_InvokeCallback__should__execute_without_exception()
//        //{
//        //    InitializeServiceHost();

//        //    client.Connect();
//        //    CompositeType result = client.InvokeCallback(new CompositeType());
//        //    Assert.AreEqual(3, result.IntValue);

//        //    Assert.AreEqual(CommunicationState.Opened, client.State);

//        //    client.Close();
//        //    if (perSesseion)
//        //        DisposableMonitor.Current.WaitAllInstancesDisposed();
//        //}

//        //[Test, Category(TestCategories.UnitTest)]
//        //public void SampleService_Operation__should__execute_without_exception__when__invkoed_with_OperationMode_Valid()
//        //{
//        //    InitializeServiceHost();

//        //    client.Connect();
//        //    client.Operation(OperationMode.Valid);

//        //    Assert.AreEqual(CommunicationState.Opened, client.State);

//        //    client.Close();
//        //    if (perSesseion)
//        //        DisposableMonitor.Current.WaitAllInstancesDisposed();
//        //}

//        //[Test, Category(TestCategories.UnitTest)]
//        //public void SampleService_Operation__should__throw_FaultException_InvalidOperationException()
//        //{
//        //    InitializeServiceHost();

//        //    client.Connect();

//        //    try
//        //    {
//        //        client.Operation(OperationMode.FaultException_InvalidOperationException);
//        //        throw new ApplicationException();
//        //    }
//        //    catch (FaultException<InvalidOperationException>)
//        //    {
//        //        Assert.AreEqual(CommunicationState.Opened, client.State);

//        //        client.Operation(OperationMode.Valid);

//        //        client.Close();
//        //        if (perSesseion)
//        //            DisposableMonitor.Current.WaitAllInstancesDisposed();
//        //    }
//        //}

//        //[Test, Category(TestCategories.UnitTest)]
//        //public void SampleService_Operation__should__throw_InvalidOperationException__when__Connect_has_not_been_invoked_previousley()
//        //{
//        //    InitializeServiceHost();

//        //    try
//        //    {
//        //        client.Operation(OperationMode.Valid);
//        //        throw new ApplicationException();
//        //    }
//        //    catch (InvalidOperationException)
//        //    {
//        //        Assert.AreEqual(CommunicationState.Created, client.State);

//        //        DisposableMonitor.Current.WaitAllInstancesDisposed();

//        //        client.Abort();
//        //    }
//        //}

//        //[Test, Category(TestCategories.UnitTest)]
//        //public void SampleService_Operation__should__throw_FaultException()
//        //{
//        //    InitializeServiceHost();

//        //    client.Connect();

//        //    try
//        //    {
//        //        client.Operation(OperationMode.FaultException);
//        //        throw new ApplicationException();
//        //    }
//        //    catch (FaultException)
//        //    {
//        //        Assert.AreEqual(CommunicationState.Opened, client.State);

//        //        client.Close();

//        //        if (perSesseion)
//        //            DisposableMonitor.Current.WaitAllInstancesDisposed();
//        //    }
//        //}

//        //[Test, Category(TestCategories.UnitTest), Category("Unstablex"), Isolated]
//        //public void SampleService_Operation__should__throw_FaultException__when__SampleService_code_contract_is_violated()
//        //{
//        //    InitializeServiceHost();

//        //    client.Connect();
//        //    try
//        //    {
//        //        client.Operation(OperationMode.CodeContractException);
//        //        throw new ApplicationException();
//        //    }
//        //    catch (FaultException)
//        //    {
//        //        Assert.AreEqual(CommunicationState.Faulted, client.State);

//        //        if (perSesseion)
//        //            DisposableMonitor.Current.WaitAllInstancesDisposed();

//        //        client.Abort();
//        //    }
//        //}

//        //[Test, Category(TestCategories.UnitTest), Category(TestCategories.Unstable), Isolated]
//        //public void SampleService_Operation__should__throw_FaultException__when__InvalidOperationException_occurs_inside_service()
//        //{
//        //    InitializeServiceHost();

//        //    client.Connect();

//        //    try
//        //    {
//        //        client.Operation(OperationMode.InvalidOperationException);
//        //        throw new ApplicationException();
//        //    }
//        //    catch (FaultException)
//        //    {
//        //        Assert.AreEqual(CommunicationState.Faulted, client.State);

//        //        if (perSesseion)
//        //            DisposableMonitor.Current.WaitAllInstancesDisposed();

//        //        client.Abort();
//        //    }
//        //}

//        //[Test, Category(TestCategories.IntegrationTest)]
//        //public void SampleService_Operation__should__throw_TimeoutException()
//        //{
//        //    InitializeServiceHost();

//        //    client.Connect();

//        //    try
//        //    {
//        //        client.Operation(OperationMode.TimeoutException);
//        //        throw new ApplicationException();
//        //    }
//        //    catch (TimeoutException)
//        //    {
//        //        Assert.AreEqual(CommunicationState.Opened, client.State);

//        //        DisposableMonitor.Current.WaitAllInstancesDisposed();

//        //        client.Abort();
//        //    }
//        //}

//        //[Test, Category(TestCategories.IntegrationTest)]
//        //public void SampleService_Connect__should__throw_ServerTooBusyException__when_SampleService_has_not_been_hosted()
//        //{
//        //    try
//        //    {
//        //        client.Connect();
//        //        throw new ApplicationException();
//        //    }
//        //    catch (CommunicationException)
//        //    {
//        //        Assert.AreEqual(CommunicationState.Faulted, client.State);

//        //        client.Abort();
//        //    }
//        //}

//        //[Test, Category(TestCategories.IntegrationTest)]
//        //public void SampleService_Operation__should__throw_CommunicationObjectFaultedException__when__the_service_has_been_closed_du_to_inactivityTimeout()
//        //{
//        //    InitializeServiceHost();
//        //    host.Close();

//        //    client.Connect();

//        //    Thread.Sleep(6000);
//        //    DisposableMonitor.Current.WaitAllInstancesDisposed();

//        //    try
//        //    {
//        //        client.Operation(OperationMode.Valid);
//        //        throw new ApplicationException();
//        //    }
//        //    catch (CommunicationObjectFaultedException)
//        //    {
//        //        Assert.AreEqual(CommunicationState.Faulted, client.State);

//        //        client.Abort();
//        //    }
//        //}

//        protected abstract void InitializeServiceHost();

//        protected void ServiceHostFaulted(object sender, EventArgs e)
//        {
//            SetUnhandledException(new InvalidOperationException());
//        }

//        //public void Dispose()
//        //{
//        //    Dispose(true);
//        //    GC.SuppressFinalize(this);
//        //}

//        //protected virtual void Dispose(bool disposing)
//        //{
//        //    if (disposing && disposed != null)
//        //    {
//        //        //disposed.Dispose();
//        //        //disposed = null;
//        //    }
//        //}

//        protected SampleServiceClient client;

//        protected ServiceHost host;
//        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
//    }

//    [Log]
//    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
//    public class SampleServiceEapCallback: ISampleServiceCallback
//    {
//        public IAsyncResult BeginCallback(CompositeType compositeType, AsyncCallback callback, object asyncState)
//        {
//            throw new InvalidOperationException();
//        }

//        public IAsyncResult BeginOneWayCallback(CompositeType compositeType, AsyncCallback callback, object asyncState)
//        {
//            throw new InvalidOperationException();
//        }

//        public CompositeType Callback(CompositeType compositeType) => compositeType.Increment();

//        public CompositeType EndCallback(IAsyncResult result)
//        { throw new InvalidOperationException(); }

//        public void EndOneWayCallback(IAsyncResult result)
//        { throw new InvalidOperationException(); }

//        public void OneWayCallback(CompositeType compositeType)
//        { }
//    }
//}
