////
////  Include this file in your project if your project uses
////  ContractArgumentValidator or ContractAbbreviator methods
////

//using System.Diagnostics.CodeAnalysis;

//// TODO: CR

//namespace System.Diagnostics.Contracts
//{
//    /// <summary>Enables writing abbreviations for contracts that get copied to other methods</summary>
//    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
//    [Conditional("CONTRACTS_FULL")]
//    internal sealed class ContractAbbreviatorAttribute: Attribute {}

//    /// <summary>Enables factoring legacy if-then-throw into separate methods for reuse and full control over thrown exception
//    ///     and arguments</summary>
//    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
//    [Conditional("CONTRACTS_FULL")]
//    internal sealed class ContractArgumentValidatorAttribute: Attribute {}

//    /// <summary>Allows setting contract and tool options at assembly, type, or method granularity.</summary>
//    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
//    [Conditional("CONTRACTS_FULL")]
//    internal sealed class ContractOptionAttribute: Attribute
//    {
//            Justification = "Build-time only attribute")]
//            Justification = "Build-time only attribute")]
//            Justification = "Build-time only attribute")]
//        public ContractOptionAttribute(string category, string setting, bool toggle) {}

//            Justification = "Build-time only attribute")]
//            Justification = "Build-time only attribute")]
//            Justification = "Build-time only attribute")]
//        public ContractOptionAttribute(string category, string setting, string value) {}
//    }
//}
