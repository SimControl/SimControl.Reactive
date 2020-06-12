// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using NLog;

namespace SimControl.Log
{
    /// <summary>Utility class to format log messages.</summary>
    public static class LogMethod
    {
        static LogMethod() => LogFormat.LogFormatMaxCollectionElements = 0;

        /// <summary>Format a log message for a method entry.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="method">The method.</param>
        /// <param name="instance">(Optional) The instance.</param>
        /// <param name="args">The args.</param>
        public static void Entry(this Logger logger, LogLevel logLevel, MethodBase method, object instance = null,
                                 params object[] args)
        {
            Contract.Requires(logger != null);
            Contract.Requires(method != null);

            logger.Log(logLevel,
                "{ " + method.Name + LogFormat.FormatToString(instance) +
                (args.Length > 0 ? LogFormat.FormatArgsList(args) : ""));
        }

        /// <summary>Format a log message for a method for an exception.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="method">The method.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="logException">The exception.</param>
        public static void Exception(this Logger logger, LogLevel logLevel, MethodBase method, object instance,
                                     Exception logException)
        {
            Contract.Requires(logger != null);
            Contract.Requires(method != null);
            Contract.Requires(logException != null);

            logger.Log(logLevel, logException, "! " + method.Name + LogFormat.FormatToString(instance));
        }

        /// <summary>Format a log message for a method exit with a method result.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="method">The method.</param>
        /// <param name="instance">(Optional) The instance.</param>
        /// <param name="result">(Optional) The result.</param>
        public static void Exit(this Logger logger, LogLevel logLevel, MethodBase method, object instance = null,
                                object result = null)
        {
            Contract.Requires(logger != null);
            Contract.Requires(method != null);

            logger.Log(logLevel,
                "} " + method.Name + LogFormat.FormatToString(instance) +
                (result is IEnumerable resultEnumerable && !(result is string)
                     ? LogFormat.FormatIEnumerable(resultEnumerable) : LogFormat.FormatToString(result)));
        }

        /// <summary>Format an arbitrary log message.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="args">The args.</param>
        public static void Message(this Logger logger, LogLevel logLevel, params object[] args)
        {
            Contract.Requires(logger != null);

            logger.Log(logLevel, ":" + (args.Length > 0 ? LogFormat.FormatArgsList(args) : ""));
        }

        /// <summary>Format an arbitrary log message.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="method">The method.</param>
        /// <param name="args">The args.</param>
        public static void Message(this Logger logger, LogLevel logLevel, MethodBase method, params object[] args)
        {
            Contract.Requires(logger != null);

            logger.Log(logLevel, ": " + method.Name + (args.Length > 0 ? LogFormat.FormatArgsList(args) : ""));
        }

        internal static void LogEntryFromLogAttribute(Logger logger, LogLevel logLevel, MethodBase method,
                                                      object instance, ICollection<object> args) =>
            logger.Log(logLevel, "{ " + method.Name + LogFormat.FormatToString(instance) +
                (args.Count > 0 ? LogFormat.FormatArgsList(args) : ""));
    }
}
