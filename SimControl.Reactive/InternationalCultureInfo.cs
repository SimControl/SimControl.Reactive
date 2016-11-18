// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace SimControl.Reactive
{
    /// <summary>
    /// Generic <see cref="CultureInfo"/> for logging exceptions in English with dates formatted to "yyyy-MM-dd HH:mm:ss".
    /// </summary>
    public static class InternationalCultureInfo
    {
        /// <summary>Default constructor.</summary>
        static InternationalCultureInfo()
        {
            Instance = new CultureInfo("en-US");
            Instance.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Instance.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Instance.DateTimeFormat.ShortTimePattern = "HH:mm";
        }

        /// <summary>Sets the current thread culture to InternationalCultureInfo.</summary>
        public static void SetCurrentThreadCulture()
        {
            Thread.CurrentThread.CurrentCulture = Instance;
            Thread.CurrentThread.CurrentUICulture = Instance;
        }

        /// <summary>Sets the current thread culture.</summary>
        /// <param name="currentCulture">The current culture.</param>
        /// <param name="currentUICulture">The current user interface culture.</param>
        public static void SetCurrentThreadCulture(CultureInfo currentCulture, CultureInfo currentUICulture)
        {
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
        }

        /// <summary>The instance.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly CultureInfo Instance;
    }
}
