// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.Threading;
using SimControl.LogEx;
using SimControl.Samples.CSharp.Wcf.ServiceContract;

namespace SimControl.Samples.CSharp.Wcf.Service
{
    /// <summary>WCF sample service.</summary>
    [Log]
    public class SampleService : ISampleService, IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="SampleService"/> class.</summary>
        protected SampleService() => blockingCollection.Add(IncrementInstances());

        /// <inheritdoc/>
        public CompositeType ComplexOperation(CompositeType data) => State = data.Increment();

        /// <inheritdoc/>
        public void ComplexOperationOneWay(CompositeType data) => State = data.Increment();

        /// <inheritdoc/>
        public virtual void Connect() { }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public void Operation(OperationMode operationMode)
        {
            switch (operationMode)
            {
                case OperationMode.Valid:
                    break;
                case OperationMode.FaultException:
                    throw new FaultException("Fault");
                case OperationMode.FaultException_InvalidOperationException:
                    throw new FaultException<InvalidOperationException>(new InvalidOperationException("Invalid"));
                case OperationMode.InvalidOperationException:
                    throw new InvalidOperationException();
                case OperationMode.TimeoutException:
                    Thread.Sleep(5000);
                    break;
            }
        }

        /// <inheritdoc/>
        public void OperationOneWay(OperationMode operationMode) { }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged
        /// resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                blockingCollection.Add(DecrementInstances());

                disposed = true;
            }
        }

        private static int DecrementInstances()
        {
            lock (locker)
                return --instanceCounter;
        }

        private static int IncrementInstances()
        {
            lock (locker)
                return ++instanceCounter;
        }

        /// <inheritdoc/>
        public CompositeType CompositeType => State;

        /// <summary>The state</summary>
        protected CompositeType State { get; set; }

        private static readonly BlockingCollection<int> blockingCollection = new BlockingCollection<int>();
        private static readonly object locker = new object();

        private static int instanceCounter; //TODO instance count
        private bool disposed;
    }
}
