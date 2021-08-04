// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO

/*
using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace SimControl.Samples.CSharp.Wcf.ServiceContract
{
    /// <summary>Sample dual WCF service // UNDONE Contract.</summary>
    [ServiceContract(CallbackContract = typeof(IDuplexSampleServiceCallback), SessionMode = SessionMode.Required)]
    [DeliveryRequirements(RequireOrderedDelivery = true, QueuedDeliveryRequirements = QueuedDeliveryRequirementsMode.NotAllowed)]
    [ContractClass(typeof(DuplexSampleServiceContract))]
    public interface IDuplexSampleService : ISampleService
    {
        /// <summary>Passes the data to the callback.</summary>
        /// <param name="compositeType">Type of the composite.</param>
        /// <returns>The data using data // UNDONE Contract.</returns>
        [OperationContract(IsInitiating = false)]
        CompositeType InvokeCallback(CompositeType compositeType);

        /// <summary>Passes the data to the callback as a one-way call.</summary>
        /// <param name="data">Type of the composite.</param>
        /// <returns>The data using data // UNDONE Contract.</returns>
        [OperationContract(IsInitiating = false, IsOneWay = true)]
        void InvokeCallbackOneWay(CompositeType data);
    }

    /// <summary>Interface for sample service callback.</summary>
    [ContractClass(typeof(DuplexSampleServiceCallbackContract))]
    [DeliveryRequirements(RequireOrderedDelivery = true)]
    public interface IDuplexSampleServiceCallback
    {
        /// <summary>Sample callback.</summary>
        /// <param name="compositeType">Composite type.</param>
        /// <returns>The object passed in.</returns>
        [OperationContract]
        CompositeType Callback(CompositeType compositeType);

        /// <summary>Oneway sample callback.</summary>
        /// <param name="data">Composite type.</param>
        [OperationContract(IsOneWay = true)]
        void OneWayCallback(CompositeType data);
    }

    [ContractClassFor(typeof(IDuplexSampleServiceCallback))]
    internal abstract class DuplexSampleServiceCallbackContract : IDuplexSampleServiceCallback
    {
        /// <inheritdoc/>
        public CompositeType Callback(CompositeType compositeType)
        {
            // UNDONE Contract.Requires(compositeType != null);

            // UNDONE Contract.Ensures(// UNDONE Contract.Result<CompositeType>() != null);

            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public void OneWayCallback(CompositeType data) { } // UNDONE Contract.Requires(data != null);
    }

    [ContractClassFor(typeof(IDuplexSampleService))]
    internal abstract class DuplexSampleServiceContract : IDuplexSampleService
    {
        public CompositeType ComplexOperation(CompositeType data) => null;

        public void ComplexOperationOneWay(CompositeType data) { }

        /// <inheritdoc/>
        public abstract void Connect();

        /// <inheritdoc/>
        public CompositeType InvokeCallback(CompositeType compositeType)
        {
            // UNDONE Contract.Requires(compositeType != null);

            // UNDONE Contract.Ensures(// UNDONE Contract.Result<CompositeType>() != null);

            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public void InvokeCallbackOneWay(CompositeType data)
        {
            // UNDONE Contract.Requires(data != null);

            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public abstract void Operation(OperationMode operationMode);

        /// <inheritdoc/>
        public abstract void OperationOneWay(OperationMode operationMode);

        public CompositeType CompositeType => null;
    }
}
*/
