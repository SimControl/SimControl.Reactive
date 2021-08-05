// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO implement

/*
using System.ServiceModel;
using SimControl.LogEx;
using SimControl.Samples.CSharp.Wcf.ServiceContract;

namespace SimControl.Samples.CSharp.Wcf.Service
{
    /// <summary>WCF sample service.</summary>
    [Log]
    public abstract class DuplexSampleService : SampleService, IDuplexSampleService
    {
        /// <inheritdoc/>
        public override void Connect() => callback = OperationContext.Current.GetCallbackChannel<IDuplexSampleServiceCallback>();

        /// <inheritdoc/>
        public CompositeType InvokeCallback(CompositeType compositeType) => State = callback.Callback(State = compositeType.Increment());

        /// <inheritdoc/>
        public void InvokeCallbackOneWay(CompositeType data) => callback.OneWayCallback(State = data.Increment());

        private IDuplexSampleServiceCallback callback;
    }
}
*/
