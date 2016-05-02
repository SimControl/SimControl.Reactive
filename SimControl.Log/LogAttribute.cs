// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using NLog;
using PostSharp.Aspects;

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
    public sealed class LogAttribute: OnMethodBoundaryAspect
    {
        /// <summary>Default constructor.</summary>
        public LogAttribute()
        { ApplyToStateMachine = false; }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            Contract.Requires(method != null);

            this.method = method;
            var methodInfo = method as MethodInfo;
            hasReturnValue = methodInfo != null && methodInfo.ReturnType != typeof(void);

            excluded |= (this.method.Name == nameof(System.Object) || this.method.Name == nameof(ToString) ||
                this.method.Name.StartsWith("get_", StringComparison.Ordinal) ||
                method.Name == "Dispose" && method.DeclaringType.GetInterfaces().Contains(typeof(IDisposable)) &&
                method.GetParameters().Length == 1);
            logInstanceOnEntry &=!method.IsConstructor;
            logInstanceOnExit &=(method.Name !="Dispose" ||
                                 !method.DeclaringType.GetInterfaces().Contains(typeof(IDisposable)) ||
                                 method.GetParameters().Length !=0);
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (logLevel != NLog.LogLevel.Off && logger.IsEnabled(logLevel) && !excluded)
                LogMethod.LogEntryFromLogAttribute(logger,
                    logLevel,
                    method,
                    logInstanceOnEntry ? args.Instance : null,
                    args.Arguments);
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override void OnException(MethodExecutionArgs args)
        {
            if (exceptionLogLevel != NLog.LogLevel.Off && logger.IsEnabled(exceptionLogLevel) && !excluded)
                logger.Exception(exceptionLogLevel, method, args.Instance, args.Exception);
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (logLevel != NLog.LogLevel.Off && logger.IsEnabled(logLevel) && !excluded)
                logger.Exit(logLevel,
                    method,
                    logInstanceOnExit ? args.Instance : null,
                    hasReturnValue ? args.ReturnValue : null);
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override void RuntimeInitialize(MethodBase method)
        {
            Contract.Requires(method != null);

            logger = LogManager.GetLogger(method.DeclaringType.FullName);
            logLevel = NLog.LogLevel.FromOrdinal((int) LogLevel);
            exceptionLogLevel = NLog.LogLevel.FromOrdinal((int) ExceptionLogLevel);
        }

        /// <summary>Log level used for exception log messages.</summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public LogAttributeLevel ExceptionLogLevel = LogAttributeLevel.Error;

        /// <summary>Log level used for entry and exit log messages.</summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public LogAttributeLevel LogLevel = LogAttributeLevel.Trace;

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
