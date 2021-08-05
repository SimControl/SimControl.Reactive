// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

namespace SimControl.Samples.CSharp.Wcf.ServiceContract
{
    /// <summary>WCF service operation mode enumeration.</summary>
    public enum OperationMode
    {
        /// <summary>Valid</summary>
        Valid,

        /// <summary>FaultException</summary>
        FaultException,

        /// <summary>InvalidOperationException</summary>
        InvalidOperationException,

        /// <summary>TimeoutException</summary>
        TimeoutException
    }
}
