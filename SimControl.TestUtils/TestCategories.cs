// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;

namespace SimControl.TestUtils
{
    /// <summary>An integration tests tests the integration of several disjoint components.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class IntegrationTestAttribute: CategoryAttribute { }

    /// <summary>An interactive test requires some user interaction during test execution.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class InteractiveTestAttribute: CategoryAttribute { }

    /// <summary>A performance test tests the performance of specific operations.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class PerformanceTestAttribute: CategoryAttribute { }

    /// <summary>A stability test tests the stability of specific operations/components.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class StabilityTestAttribute: CategoryAttribute { }
}
