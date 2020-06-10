// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Threading;
//using System.Threading.Tasks;

//// TODO: CR

//namespace SimControl.Reactive
//{
//    /// <summary>Monitors the number of disposable object that have been created and disposed. Can be uesed to assure that
//    ///     async services have been shut down after test runs.</summary>
//    public static class DisposableMonitor
//    {
//        /// <summary>Remove an instance</summary>
//        /// <param name="instance">The instance.</param>
//        public static void DisposeInstance(object instance)
//        {
//#if DEBUG
//            Contract.Requires(instance != null);

//            lock (instances)
//            {
//                if (!instances.Contains(instance)) throw new InvalidOperationException("DisposableMonitor does not contain instance");

//                instances.Remove(instance);

//                if (instances.Count == 0) ready.Set();
//            }
//#endif
//        }

//        /// <summary>Add a new instance to be monitored.</summary>
//        /// <param name="instance">The instance.</param>
//        public static void NewInstance(object instance)
//        {
//#if DEBUG
//            Contract.Requires(instance != null);

//            lock (instances)
//            {
//                if (instances.Contains(instance)) throw new InvalidOperationException("DisposableMonitor already contains instance");

//                if (instances.Count == 0)
//                    ready.Reset();

//                instances.Add(instance);
//            }
//#endif
//        }

//        /// <summary>Reinitializes the singletons.</summary>
//        public static void ReinitializeSingletons()
//        {
//#if DEBUG
//            instances.Clear();
//            ready.Set();
//#endif
//        }

//        /// <summary>Wait until all instances are disposed.</summary>
//        public static void WaitAllInstancesDisposed(int timeout)
//        {
//#if DEBUG
//            if (!ready.WaitOne(timeout))
//                throw new TimeoutException();
//#endif
//        }

//#if DEBUG
//        private static readonly HashSet<object> instances = new HashSet<object>();
//        private static readonly ManualResetEvent ready = new ManualResetEvent(true);
//#endif
//    }
//}
