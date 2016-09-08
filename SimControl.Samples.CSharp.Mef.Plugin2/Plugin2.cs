// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.ComponentModel.Composition;
using SimControl.Log;
using SimControl.Samples.CSharp.Mef.Contracts;

namespace SimControl.Samples.CSharp.Mef.Plugin
{
    /// <summary>Sample MEF plugin.</summary>
    [Export(typeof(IPlugin))]
    [Log]
    public class Plugin2 : IPlugin
    {
        /// <summary>Initializes a new instance of the <see cref="Plugin2"/> class.</summary>
        /// <param name="resource">The resource.</param>
        [ImportingConstructor]
        public Plugin2(IResource resource) { this.resource = resource; }

        /// <inheritdoc/>
        public string ResourceName() => resource.ResourceName;

        private readonly IResource resource;
    }
}
