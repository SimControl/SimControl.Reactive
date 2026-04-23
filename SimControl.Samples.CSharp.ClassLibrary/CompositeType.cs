// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

using SimControl.Logging;
using System.Runtime.Serialization;

namespace SimControl.Samples.CSharp.Wcf.ServiceContract;

/// <summary>WCF service sample composite data.</summary>
[DataContract]
public class CompositeType
{
    /// <summary>Increments the int value.</summary>
    /// <returns>The incremented object.</returns>
    public CompositeType Increment()
    {
        IntValue++;
        return this;
    }

    /// <inheritdoc/>
    public override string ToString() => LogFormat.FormatObject(typeof(CompositeType), IntValue);

    /// <summary>Integer value.</summary>
    /// <value>The int value.</value>
    public int IntValue { get; set; }

    /// <summary>String value.</summary>
    [DataMember]
    public string StringValue { get; set; } = "";
}
