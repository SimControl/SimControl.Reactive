// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using ArxOne.MrAdvice.Advice;
using NLog;

namespace SimControl.Log
{
    /// <summary>Custom enum, as <see cref="LogLevel"/> can not be used as an attribute parameter.</summary>
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
    //[Serializable]
    [AttributeUsage(
        AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor |
        AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface,
        AllowMultiple = true, Inherited = true)]
    public sealed class LogAttribute: Attribute, IMethodAdvice, IMethodAsyncAdvice, IPropertyAdvice
    {
        /// <summary>Log level used for exception log messages.</summary>
        public LogAttributeLevel ExceptionLogLevel { get; set; } = LogAttributeLevel.Error;

        /// <summary>Log level used for entry and exit log messages.</summary>
        public LogAttributeLevel LogLevel { get; set; } = LogAttributeLevel.Info;

        private LogLevel exceptionLogLevel;

        private bool hasReturnValue;

        private readonly bool logInstanceOnEntry = true;
        private readonly bool logInstanceOnExit = true;

        [NonSerialized]
        private LogLevel logLevel;

        //private MethodBase method;

        public void Advise(MethodAdviceContext context)
        {
            //this.method = method;
            //var methodInfo = method as MethodInfo;
            //hasReturnValue = methodInfo != null && methodInfo.ReturnType != typeof(void);

            //excluded |= this.method.Name == nameof(Object) || this.method.Name == nameof(ToString) ||
            //    this.method.Name.StartsWith("get_", StringComparison.Ordinal) ||
            //    (method.Name == "Dispose" && method.DeclaringType.GetInterfaces().Contains(typeof(IDisposable)) &&
            //    method.GetParameters().Length == 1);
            //logInstanceOnEntry &= !method.IsConstructor;
            //logInstanceOnExit &= method.Name != "Dispose" ||
            //                     !method.DeclaringType.GetInterfaces().Contains(typeof(IDisposable)) ||
            //                     method.GetParameters().Length != 0;




            logger = LogManager.GetLogger(context.TargetType.FullName);
            logLevel = NLog.LogLevel.FromOrdinal((int) LogLevel);
            exceptionLogLevel = NLog.LogLevel.FromOrdinal((int) ExceptionLogLevel);

            MethodInfo res = context.TargetMethod is MethodInfo ? (MethodInfo) context.TargetMethod : null ;
            hasReturnValue = res != null && res.ReturnType != typeof(void);

            if (logLevel != NLog.LogLevel.Off && logger.IsEnabled(logLevel))
                LogMethod.LogEntryFromLogAttribute(logger,
                    logLevel,
                    context.TargetMethod,
                    logInstanceOnEntry ? context.Target : null,
                    context.Arguments);

            try
            { context.Proceed(); }
            catch (Exception e)
            {
                if (exceptionLogLevel != NLog.LogLevel.Off && logger.IsEnabled(exceptionLogLevel))
                    logger.Exception(exceptionLogLevel, context.TargetMethod, context.Target, e);
                throw;
            }

            if (logLevel != NLog.LogLevel.Off && logger.IsEnabled(logLevel))
                logger.Exit(logLevel, context.TargetMethod, logInstanceOnExit ? context.Target : null,
                    hasReturnValue ? context.ReturnValue : null);
        }

        public Task Advise(MethodAsyncAdviceContext context) => context.ProceedAsync();

        public void Advise(PropertyAdviceContext context) => context.Proceed();

        private Logger logger;
    }

    //[Serializable]
    [AttributeUsage(
        AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor |
        AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface,
        AllowMultiple = true, Inherited = true)]
    public sealed class LogExcludeAttribute: Attribute
    { }
}
