// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

namespace SimControl.Samples.CSharp.Wcf.ServiceContract
{
    /// <summary>Operation mode enum.</summary>
    public enum OperationMode
    {
        /// <summary>Valid</summary>
        Valid,

        /// <summary>CodeContractException</summary>
        CodeContractException,

        /// <summary>FaultException</summary>
        FaultException,

        /// <summary>InvalidOperationException</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        FaultException_InvalidOperationException,

        /// <summary>InvalidOperationException</summary>
        InvalidOperationException,

        /// <summary>TimeoutException</summary>
        TimeoutException
    };
}
