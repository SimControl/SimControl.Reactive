// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Samples.CSharp.ClassLibrary;
using SimControl.Samples.CSharp.ClassLibrary.Component;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class ComponentLoaderTest : TestFrame
    {
        [Test]
        public void Test1()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new Counter()).As<Counter>().SingleInstance();
            builder.Register(c => new Element(c.Resolve<Counter>(), "A")).As<IElement>();
            builder.Register(c => new Component1(c.Resolve<IElement>())).As<IComponent>();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
                Assert.That(scope.Resolve<IComponent>().ElementName, Is.EqualTo("Element.0.A"));
        }

        [Test]
        public void Test2()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new Counter()).As<Counter>().SingleInstance();
            builder.RegisterInstance("Text").Named<string>("Text").ExternallyOwned();
            builder.Register(c => new Element(c.Resolve<Counter>(), "B")).As<IElement>();
            builder.Register(c => new Component2(c.Resolve<IElement>(), c.ResolveNamed<string>("Text"))).As<IComponent>();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
                Assert.That(scope.Resolve<IComponent>().ElementName, Is.EqualTo("Element.0.B.Text"));
        }

        [Test]
        public void Test3()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new Counter()).As<Counter>().SingleInstance();
            builder.RegisterInstance("Text").Named<string>("Text").ExternallyOwned();
            builder.Register(c => new Element(c.Resolve<Counter>(), "C")).As<IElement>();
            builder.Register(c => new Component1(c.Resolve<IElement>())).As<Component1>();
            builder.Register(c => new Component2(c.Resolve<IElement>(), c.ResolveNamed<string>("Text"))).As<Component2>();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                Assert.That(scope.Resolve<Component1>().ElementName, Is.EqualTo("Element.0.C"));
                Assert.That(scope.Resolve<Component2>().ElementName, Is.EqualTo("Element.1.C.Text"));
            }
        }

        [Test]
        public void Test4()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new Counter()).As<Counter>().SingleInstance();
            builder.RegisterInstance("Text").Named<string>("Text").ExternallyOwned();
            builder.Register(c => new Element(c.Resolve<Counter>(), "B")).As<IElement>();
            builder.Register(c => new Component1(c.Resolve<IElement>())).As<IComponent>();
            builder.Register(c => new Component2(c.Resolve<IElement>(), c.ResolveNamed<string>("Text"))).As<IComponent>();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                IEnumerable<IComponent> components = scope.Resolve<IEnumerable<IComponent>>();
                Assert.That(components.Count, Is.EqualTo(2));
                Assert.That(components.Where(t => t is Component1).Count, Is.EqualTo(1));
                Assert.That(components.Where(t => t is Component2).Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Test5()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Counter>().SingleInstance();
            builder.Register(c => new Element(c.Resolve<Counter>(), "A")).As<IElement>();
            builder.RegisterType<Component1>().As<IComponent>();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
                Assert.That(scope.Resolve<IComponent>().ElementName, Is.EqualTo("Element.0.A"));
        }
    }
}
