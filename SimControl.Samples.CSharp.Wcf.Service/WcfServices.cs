// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.ServiceModel;
using SimControl.Log;

namespace SimControl.Samples.CSharp.Wcf.Service
{
    /// <summary>WCF duplex sample service InstanceContextMode.PerSession.</summary>
    [Log]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class DuplexSampleServicePerSessionInstance : DuplexSampleService { }

    /// <summary>WCF duplex sample service with InstanceContextMode.Single.</summary>
    [Log]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class DuplexSampleServiceSingleInstance : DuplexSampleService { }

    /// <summary>WCF sample service InstanceContextMode.PerSession.</summary>
    [Log]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Single)]
    public class SampleServicePerSessionInstance : SampleService { }

    /// <summary>WCF sample service with InstanceContextMode.Single.</summary>
    [Log]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class SampleServiceSingleInstance : SampleService { }
}
