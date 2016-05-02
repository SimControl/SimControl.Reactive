// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Runtime.Serialization;
using SimControl.Reactive;

namespace SimControl.Samples.CSharp.Wcf.ServiceContract
{
    /// <summary>Sample composite data contract.</summary>
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int"), DataMember]
        public int IntValue { get; set; } = 0;

        /// <summary>String value.</summary>
        [DataMember]
        public string StringValue { get; set; } = "";
    }
}
