// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NLog;


namespace SimControl.Log
{
    /// <summary>Custom enum, as <see cref="NLog.LogLevel"/> can not be used as an attribute parameter.</summary>
    public enum LogAttributeLevel
    {
        /// <summary>Trace level</summary>
        Trace = 0,

        /// <summary>Debug level</summary>
        Debug = 1,

        /// <summary>Info level</summary>
        Info = 2,

        /// <summary>Warn level</summary>
        Warn = 3,

        /// <summary>Error level</summary>
        Error = 4,

        /// <summary>Fatal level</summary>
        Fatal = 5,

        /// <summary>Off</summary>
        Off = 6,
    }

    /// <summary>Automatically log all method calls (except property getters) with NLOG.</summary>
    [Serializable]
    [AttributeUsage(
        AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor |
        AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface,
        AllowMultiple = true, Inherited = true)]
    public sealed class LogAttribute: Attribute
    {
        /// <summary>Default constructor.</summary>
        public LogAttribute() { }


        /// <summary>Log level used for exception log messages.</summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public LogAttributeLevel ExceptionLogLevel = LogAttributeLevel.Error;

        /// <summary>Log level used for entry and exit log messages.</summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public LogAttributeLevel LogLevel;

        [NonSerialized]
        private LogLevel exceptionLogLevel;

        private bool excluded;
        private bool hasReturnValue;

        [NonSerialized]
        private Logger logger;

        private bool logInstanceOnEntry = true;
        private bool logInstanceOnExit = true;

        [NonSerialized]
        private LogLevel logLevel;

        private MethodBase method;
    }
}
