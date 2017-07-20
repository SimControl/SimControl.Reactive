// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using NLog;
using SimControl.Log.Properties;
using SimControl.Reactive;

#pragma warning disable S3242 // Method parameters should be declared with base types

namespace SimControl.Log
{
    /// <summary>Utility class to format log messages.</summary>
    public static class LogMethod
    {
        static LogMethod() => LogFormat.LogFormatMaxCollectionElements = Settings.Default.LogFormatMaxCollectionElements;

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
            Contract.Requires(args != null);

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

            var resultEnumerable = result as IEnumerable;

            logger.Log(logLevel,
                "} " + method.Name + LogFormat.FormatToString(instance) +
                (resultEnumerable != null && !(result is string)
                     ? LogFormat.FormatIEnumerable(resultEnumerable) : LogFormat.FormatToString(result)));
        }

        /// <summary>Format an arbitrary log message.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="args">The args.</param>
        public static void Message(this Logger logger, LogLevel logLevel, params object[] args)
        {
            Contract.Requires(logger != null);
            Contract.Requires(args != null);

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
            Contract.Requires(method != null);
            Contract.Requires(args != null);

            logger.Log(logLevel, ": " + method.Name + (args.Length > 0 ? LogFormat.FormatArgsList(args) : ""));
        }

        /// <summary>Sets the default thread culture to InternationalCultureInfo.</summary>
        public static void SetDefaultThreadCulture()
        {
            typeof(CultureInfo).InvokeMember("s_userDefaultCulture", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.SetField, null, null, new object[] { InternationalCultureInfo.Instance }, CultureInfo.InvariantCulture);
            typeof(CultureInfo).InvokeMember("s_userDefaultUICulture", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.SetField, null, null, new object[] { InternationalCultureInfo.Instance }, CultureInfo.InvariantCulture);
            // CultureInfo.DefaultThreadCurrentCulture = new InternationalCultureInfo(); Requires .Net Framework 4.5
        }

        /// <summary>Sets the default thread culture.</summary>
        /// <param name="currentCulture">The current culture.</param>
        /// <param name="currentUICulture">The current user interface culture.</param>
        public static void SetDefaultThreadCulture(CultureInfo currentCulture, CultureInfo currentUICulture)
        {
            typeof(CultureInfo).InvokeMember("s_userDefaultCulture", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.SetField, null, null, new object[] { currentCulture }, CultureInfo.InvariantCulture);
            typeof(CultureInfo).InvokeMember("s_userDefaultUICulture", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.SetField, null, null, new object[] { currentUICulture }, CultureInfo.InvariantCulture);
            // CultureInfo.DefaultThreadCurrentCulture = new InternationalCultureInfo(); Requires .Net Framework 4.5
        }

        internal static void LogEntryFromLogAttribute(Logger logger, LogLevel logLevel, MethodBase method,
                                                      object instance, ICollection<object> args) =>
            logger.Log(logLevel, "{ " + method.Name + LogFormat.FormatToString(instance) +
                (args.Count > 0 ? LogFormat.FormatArgsList(args) : ""));
    }
}

#pragma warning restore S3242
