// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ServiceModel;
using System.Threading;
using SimControl.Log;
using SimControl.Reactive;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public CompositeType InvokeCallback(CompositeType compositeType) => State = callback.Callback(State = compositeType.Increment());

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void InvokeCallbackOneWay(CompositeType data) => callback.OneWayCallback(State = data.Increment());

        private IDuplexSampleServiceCallback callback;
    }
}
