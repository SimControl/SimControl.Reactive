// (C) KEBA Linz, Austria. All rights reserved.

// TODO CR

using System.Globalization;
using System.Reflection;
using System.Threading;

namespace SimControl.Log
{
    /// <summary>
    /// Represents the culture used for diagnostics, providing identical English output in English language and
    /// YYYY-MM-DD date time format.
    /// </summary>
    public static class InternationalCultureInfo
    {
        static InternationalCultureInfo()
        {
            Instance = new CultureInfo("en-US", false);
            Instance.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Instance.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Instance.DateTimeFormat.ShortTimePattern = "HH:mm";
        }

        /// <summary>Sets the current thread culture to the default diagnostic culture.</summary>
        public static void SetCurrentThreadCulture() =>
            SetCurrentThreadCulture(Instance, Instance);

        /// <summary>Sets the current thread culture to the culture provided.</summary>
        /// <param name="cultureInfo">The default <see cref="CultureInfo"/>.</param>
        /// <param name="cultureInfoUI">The default user interface <see cref="CultureInfo"/>.</param>
        public static void SetCurrentThreadCulture(CultureInfo cultureInfo, CultureInfo cultureInfoUI)
        {
            // Contract.Requires( cultureInfo != null ); Contract.Requires( cultureInfoUI != null );

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfoUI;
        }

        /// <summary>
        /// Sets the default culture for threads in the current application domain. <remarks>When using .NET 4.5 use
        /// CultureInfo.DefaultThreadCurrentCulture.</remarks>
        /// </summary>
        /// <param name="cultureInfo">The default <see cref="CultureInfo"/>.</param>
        /// <param name="cultureInfoUI">The default user interface <see cref="CultureInfo"/>.</param>
        public static void SetDefaultThreadCulture(CultureInfo cultureInfo, CultureInfo cultureInfoUI)
        {
            // Contract.Requires( cultureInfo != null ); Contract.Requires( cultureInfoUI != null );

            // sets the default culture for threads in the current application domain.
            // note: when using .NET 4.5 use CultureInfo.DefaultThreadCurrentCulture.
            typeof(CultureInfo).InvokeMember("s_userDefaultCulture",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.SetField, null, null,
                new object[] { cultureInfo }, CultureInfo.InvariantCulture);

            typeof(CultureInfo).InvokeMember("s_userDefaultUICulture",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.SetField, null, null,
                new object[] { cultureInfoUI }, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Sets the default diagnostic culture for threads in the current application domain. <remarks>When using .NET
        /// 4.5 use CultureInfo.DefaultThreadCurrentCulture.</remarks>
        /// </summary>
        public static void SetDefaultThreadCulture() =>
            SetDefaultThreadCulture(Instance, Instance);

        /// <summary>Gets the culture used for diagnostics.</summary>
        public static CultureInfo Instance { get; private set; }
    }
}
