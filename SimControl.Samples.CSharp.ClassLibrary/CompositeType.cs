// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Runtime.Serialization;
using SimControl.Log;

namespace SimControl.Samples.CSharp.Wcf.ServiceContract
{
    /// <summary>Sample composite data // Contract.</summary>
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
}
