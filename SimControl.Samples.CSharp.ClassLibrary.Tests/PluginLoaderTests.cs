// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
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
            var loader = new PluginLoader();
            loader.Load();

            Assert.AreEqual(loader.Resource.ResourceName, loader.Plugins[0].ResourceName());
            Assert.AreEqual(loader.Resource.ResourceName, loader.Plugins[1].ResourceName());
        }

        [Test]
        public void PluginLoaderTests_PluginLoader_Load_Loads2Plugins()
        {
            var loader = new PluginLoader();
            loader.Load();

            Assert.AreEqual(2, loader.Plugins.Count);
        }

        [Test]
        public void PluginLoaderTests_TestDirectory_Cotains2Plugins()
        {
            Assert.AreEqual(2, Directory.GetFiles(TestContext.CurrentContext.TestDirectory, "SimControl.Samples.CSharp.Mef.Plugin?.dll").Length);
        }
    }
}
