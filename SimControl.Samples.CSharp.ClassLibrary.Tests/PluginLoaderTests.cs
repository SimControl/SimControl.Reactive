// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mef;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Samples.CSharp.ClassLibrary;
using SimControl.Samples.CSharp.ClassLibrary.Tests;
using SimControl.Samples.CSharp.Mef.Contracts;
using SimControl.Samples.CSharp.Mef.Plugin;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class PluginLoaderTest: TestFrame
    {
        [Test]
        public void PluginLoaderTests_MEFCompositionContainer_GetExportedValue_InstantiatesPlugin1Object()
        {
            IPlugin plugin1;
            IResource resource;

            using (var catalog = new TypeCatalog(typeof(TestResource), typeof(Plugin1)))
            using (var container = new CompositionContainer(catalog))
            {
                plugin1 = container.GetExportedValue<IPlugin>();
                resource = container.GetExportedValue<IResource>();
            }

            Assert.IsNotNull(plugin1);
            Assert.IsNotNull(resource);

            Assert.AreEqual(resource.ResourceName, plugin1.ResourceName());
        }

        [Test]
        public void PluginLoaderTests_Plugin_ResourceName_ReturnsInstantiatedResource()
        {
            using (var aggregateCatalog = new AggregateCatalog())
            using (var assemblyCatalog = new AssemblyCatalog(typeof(Resource).Assembly))
            using (var directoryCatalog = new DirectoryCatalog(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location), "*.Plugin*.dll"))
            {
                aggregateCatalog.Catalogs.Add(assemblyCatalog);
                aggregateCatalog.Catalogs.Add(directoryCatalog);

                var builder = new ContainerBuilder();
                builder.RegisterComposablePartCatalog(aggregateCatalog);

                using (IContainer container = builder.Build())
                {
                    var plugins = container.Resolve<IEnumerable<IPlugin>>().ToArray();
                    var resource = container.Resolve<IResource>();

                    Assert.AreEqual(2, plugins.Length);
                    Assert.AreEqual(resource.ResourceName, plugins[0].ResourceName());
                    Assert.AreEqual(resource.ResourceName, plugins[1].ResourceName());
                }
            }
        }

        [Test]
        public void PluginLoaderTests_TestDirectory_Contains2Plugins() => Assert.AreEqual(2, Directory.GetFiles(TestContext.CurrentContext.TestDirectory, "SimControl.Samples.CSharp.Mef.Plugin?.dll").Length);
    }
}
