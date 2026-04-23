// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

namespace SimControl.Samples.CSharp.Tests;

[ClassDataSource<DataClass>]
[ClassConstructor<DependencyInjectionClassConstructor>]
public class AndEvenMoreTests(DataClass dataClass)
{
    [Test]
    public void HaveFun()
    {
        Console.WriteLine(dataClass);
        Console.WriteLine("For more information, check out the documentation");
        Console.WriteLine("https://tunit.dev/");
    }
}
