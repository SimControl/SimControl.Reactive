// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace SimControl.Log
{
    /// <summary>Utility class to format log messages.</summary>
    public static class LogFormat
    {
        /// <summary>Formats the args.</summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string FormatArgs(params object[] args)
        {
            // Contract.Ensures(// Contract.Result<string>() != null);

            return FormatIEnumerable(args, "(", " )");
        }

        /// <summary>Formats the args.</summary>
        /// <param name="argsList">The args.</param>
        /// <returns></returns>
        public static string FormatArgsList(IEnumerable argsList)
        {
            // Contract.Requires(argsList != null);
            // Contract.Ensures(// Contract.Result<string>() != null);

            return FormatIEnumerable(argsList, " (", " )");
        }

        /// <summary>Formats an IEnumerable.</summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static string FormatIEnumerable(IEnumerable enumerable)
        {
            // Contract.Requires(enumerable != null);
            // Contract.Ensures(// Contract.Result<string>() != null);

            return FormatIEnumerable(enumerable, " [", " ]");
        }

        /// <summary>Utility method that can be used for implementing <see cref="object.ToString()"/> in a structured way.</summary>
        /// <param name="type">The type.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string FormatObject(Type type, params object[] args)
        {
            // Contract.Requires(type != null);
            // Contract.Ensures(// Contract.Result<string>() != null);

            return args.Length == 0 ? type.FullName : type.FullName + FormatIEnumerable(args, "{", " }");
        }

        /// <summary>Returns target.toString() with handling possible exceptions and null objects.</summary>
        /// <param name="target">The target.</param>
        /// <returns>target.ToString()</returns>
        public static string FormatToString(object target)
        {
            // Contract.Ensures(// Contract.Result<string>() != null);

            try
            {
                return target == null
                           ? " null"
                           : " " +
                             (!(target is IFormattable formatable)
                                  ? target.ToString() : formatable.ToString(null, CultureInfo.InvariantCulture /*InternationalCultureInfo.Instance*/));
            }
            catch (Exception e)
            {
                return target.GetType().FullName + "{ " + e + " }";
            }
        }

        private static string FormatIEnumerable(IEnumerable enumerable, string open, string close)
        {
            // Contract.Requires(enumerable != null);
            // Contract.Ensures(// Contract.Result<string>() != null);

            var sb = new StringBuilder(open);

            //int i;

            foreach (object o in enumerable)
            {
                //if (i++ >= LogFormatMaxCollectionElements)
                //    return sb.Append(" ...").Append(close).ToString();

                sb.Append(o is IEnumerable c && !(o is string) ?
                    FormatIEnumerable(c, " [", " ]") : FormatToString(o));
            }

            return sb.Append(close).ToString();
        }

        /// <summary>The log format maximum collection elements.</summary>
        public static int LogFormatMaxCollectionElements { get; set; } = 100;
    }
}
