// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NUnit.Framework;

namespace SimControl.TestUtils
{
    /// <summary>An device tests requires specific hardware components.</summary>
    /// <seealso cref="CategoryAttribute"/>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false )]
    public sealed class DeviceTestAttribute : CategoryAttribute { }

    /// <summary>An integration tests the integration of several disjoint components.</summary>
    /// <seealso cref="CategoryAttribute"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class IntegrationTestAttribute: CategoryAttribute { }

    /// <summary>An interactive test requires some user interaction during test execution.</summary>
    /// <seealso cref="CategoryAttribute"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class InteractiveTestAttribute: CategoryAttribute { }

    /// <summary>A performance test tests the performance of specific operations.</summary>
    /// <seealso cref="CategoryAttribute"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class PerformanceTestAttribute: CategoryAttribute { }

    /// <summary>Provides API usages samples.</summary>
    /// <seealso cref="CategoryAttribute"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SamplesTestAttribute: CategoryAttribute { }

    /// <summary>A stability test tests the stability of specific operations/components.</summary>
    /// <seealso cref="CategoryAttribute"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class StabilityTestAttribute: CategoryAttribute { }
}
