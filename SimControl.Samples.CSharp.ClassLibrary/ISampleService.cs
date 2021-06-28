// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO

/*
using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace SimControl.Samples.CSharp.Wcf.ServiceContract
{
    /// <summary>Sample WCF service contract.</summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    [DeliveryRequirements(RequireOrderedDelivery = true, QueuedDeliveryRequirements = QueuedDeliveryRequirementsMode.NotAllowed)]
    [ContractClass(typeof(SampleServiceContract))]
    public interface ISampleService
    {
        /// <summary>Passes some composite data.</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [OperationContract(IsInitiating = false)]
        CompositeType ComplexOperation(CompositeType data);

        /// <summary>Passes some composite data as a one-way call.</summary>
        /// <param name="data">The data.</param>
        [OperationContract(IsInitiating = false)]
        void ComplexOperationOneWay(CompositeType data);

        /// <summary>Connect to the service.</summary>
        [OperationContract(IsInitiating = true)]
        void Connect();

        /// <summary>Execute the operation specified by OperationMode.</summary>
        /// <param name="operationMode">The operation mode.</param>
        [OperationContract(IsInitiating = false), FaultContract(typeof(InvalidOperationException))]
        void Operation(OperationMode operationMode);

        /// <summary>Execute the operation specified by OperationMode as a one-way call.</summary>
        /// <param name="operationMode">The operation mode.</param>
        [OperationContract(IsInitiating = false), FaultContract(typeof(InvalidOperationException))]
        void OperationOneWay(OperationMode operationMode);

        /// <summary>Get some composite data.</summary>
        CompositeType CompositeType
        {
            [OperationContract(IsInitiating = false)]
            get;
        }
    }

    [ContractClassFor(typeof(ISampleService))]
    internal abstract class SampleServiceContract : ISampleService
    {
        public CompositeType ComplexOperation(CompositeType data)
        {
            Contract.Requires(data != null);

            Contract.Ensures(Contract.Result<CompositeType>() != null);

            return null;
        }

        public void ComplexOperationOneWay(CompositeType data) => Contract.Requires(data != null);

        /// <inheritdoc/>
        public abstract void Connect();

        /// <inheritdoc/>
        public abstract void Operation(OperationMode operationMode);

        /// <inheritdoc/>
        public abstract void OperationOneWay(OperationMode operationMode);

        public CompositeType CompositeType
        {
            get
            {
                Contract.Ensures(Contract.Result<CompositeType>() != null);

                return null;
            }
        }
    }
}
*/
