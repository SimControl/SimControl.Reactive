// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using ArxOne.MrAdvice.Advice;
using ArxOne.MrAdvice.Annotation;
using NUnit.Framework;

namespace SimControl.Samples.CSharp.Test
{
    [AttributeUsage(AttributeTargets.Method), Priority(Priority)]
    public class Advice1Attribute: Attribute, IMethodInfoAdvice
    {
        /// <inheritdoc/>
        public void Advise(MethodInfoAdviceContext _)
        {
            if (MethodInfoAdviceTests.Expected != Priority)
                throw new InvalidOperationException();

            MethodInfoAdviceTests.Expected = Priority-1;
        }

        public const int Priority = 1;
    }

    [AttributeUsage(AttributeTargets.Method), Priority(Priority)]
    public class Advice2Attribute: Attribute, IMethodInfoAdvice
    {
        /// <inheritdoc/>
        public void Advise(MethodInfoAdviceContext _)
        {
            if (MethodInfoAdviceTests.Expected != Priority)
                throw new InvalidOperationException();

            MethodInfoAdviceTests.Expected = Priority-1;
        }

        public const int Priority = 2;
    }

    internal class ClassA
    {
        static ClassA() { }

        public ClassA() { }

        [Advice1, Advice2]
        public static void Method() { }
    }

    internal class ClassB
    {
        static ClassB() { }

        public ClassB() { }

        [Advice2, Advice1]
        public static void Method() { }
    }

    [TestFixture]
    public class MethodInfoAdviceTests
    {
        [Test]
        public void MethodInfoAdviceClassA_MethodA()
        {
            Expected = 2;
            ClassA.Method();
            Assert.That(Expected, Is.Zero);
        }

        [Test]
        public void MethodInfoAdviceClassB_MethodB()
        {
            Expected = 2;
            ClassB.Method(); // fails
            Assert.That(Expected, Is.Zero);
        }

        public static int Expected;
    }
}
