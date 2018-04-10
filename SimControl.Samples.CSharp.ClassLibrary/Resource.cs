// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.ComponentModel.Composition;
using SimControl.Log;
using SimControl.Samples.CSharp.Mef.Contracts;

namespace SimControl.Samples.CSharp.ClassLibrary
{
    /// <summary>Sample MEF plugin.</summary>
    [Export(typeof(IResource))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Log]
    public class Resource : IResource
    {
        /// <summary>Get the resource name</summary>
        public string ResourceName => typeof(Resource).FullName;
    }
}
