// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using SimControl.Samples.CSharp.Mef.Contracts;

namespace SimControl.Samples.CSharp.ClassLibrary
{
    /// <summary>
    ///
    /// </summary>
    public class PluginLoader
    {
        /// <summary>Load plugins</summary>
        /// <returns></returns>
        public void Load()
        {
            using (AggregateCatalog aggregateCatalog = new AggregateCatalog())
            using (AssemblyCatalog assemblyCatalog = new AssemblyCatalog(typeof(Resource).Assembly))
            using (DirectoryCatalog directoryCatalog = new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.Plugin*.dll"))
            {
                aggregateCatalog.Catalogs.Add(assemblyCatalog);
                aggregateCatalog.Catalogs.Add(directoryCatalog);

                using (CompositionContainer container = new CompositionContainer(aggregateCatalog))
                    container.ComposeParts(this);
            }
        }

        /// <summary>List of plugins</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ImportMany]
        public List<IPlugin> Plugins { get; set; }

        /// <summary>Gets or sets the resource.</summary>
        /// <value>The resource.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), Import]
        public IResource Resource { get; set; }
    }
}
