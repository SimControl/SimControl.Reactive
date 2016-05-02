// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading.Tasks;
using NLog;
using SimControl.Log;
using SimControl.Reactive;
using SimControl.Samples.CSharp.ClassLibrary.Properties;

namespace SimControl.Samples.CSharp.ClassLibrary
{
    /// <summary>SampleClass implementation.</summary>
    [Log]
    public class SampleClass
    {
        static SampleClass()
        {
            logger.Message(LogLevel.Info,
                MethodBase.GetCurrentMethod(),
                "SimControl.Samples.CSharp.ConsoleApplication",
                typeof(SampleClass).AssemblyQualifiedName);
        }

        /// <summary>Change user settings.</summary>
        /// <param name="newSettings">The new settings.</param>
        public static void ChangeUserSettings(string newSettings)
        {
            Contract.Requires(!string.IsNullOrEmpty(newSettings));

            Settings.Default.CSharpClassLibrary_UserSetting = newSettings;
        }

        /// <summary>Increment the static counter</summary>
        public static void IncrementStaticCounter()
        {
            counter++;
        }

        /// <summary>Saves the user settings.</summary>
        public static void SaveUserSettings()
        {
            Settings.Default.Save();
        }

        /// <summary>Does something</summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool DoSomething()
        {
            logger.Message(LogLevel.Info, MethodBase.GetCurrentMethod(), nameof(DoSomething));

            return true;
        }

        /// <summary>Writes the settings.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void LogSettings()
        {
            logger.Message(LogLevel.Debug,
                MethodBase.GetCurrentMethod(),
                "CSharpClassLibrary_AppSetting",
                Settings.Default.CSharpClassLibrary_AppSetting);
            logger.Message(LogLevel.Debug,
                MethodBase.GetCurrentMethod(),
                "CSharpClassLibrary_UserSetting",
                Settings.Default.CSharpClassLibrary_UserSetting);
            logger.Message(LogLevel.Debug,
                MethodBase.GetCurrentMethod(),
                "CSharpClassLibrary_UserSetting_StringCollection",
                Settings.Default.CSharpClassLibrary_UserSetting_StringCollection);
        }

        /// <inheritdoc/>
        public override string ToString() => LogFormat.FormatObject(typeof(SampleClass));

        /// <summary>Validate the settings.</summary>
        /// <param name="valid">if set to <c>true</c> [valid].</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void ValidateCodeContract(bool valid)
        {
            Contract.Requires(valid);
        }

        /// <summary>Validates the settings.</summary>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        public void ValidateSettings()
        {
            logger.Message(LogLevel.Debug,
                MethodBase.GetCurrentMethod(),
                "CSharpClassLibrary_AppSetting",
                Settings.Default.CSharpClassLibrary_AppSetting);
            logger.Message(LogLevel.Debug,
                MethodBase.GetCurrentMethod(),
                "CSharpClassLibrary_UserSetting",
                Settings.Default.CSharpClassLibrary_UserSetting);

            if (Settings.Default.CSharpClassLibrary_AppSetting != "CSharpClassLibrary_AppSetting_Test")
                throw new InvalidOperationException("Invalid CSharpClassLibrary_AppSetting: " +
                                                    Settings.Default.CSharpClassLibrary_AppSetting);

            if (Settings.Default.CSharpClassLibrary_UserSetting != "CSharpClassLibrary_UserSetting_Test")
                throw new InvalidOperationException("Invalid CSharpClassLibrary_UserSetting: " +
                                                    Settings.Default.CSharpClassLibrary_UserSetting);

            if (Settings.Default.CSharpClassLibrary_UserSetting_StringCollection.Count != 2)
                throw new InvalidOperationException("CSharpClassLibrary_UserSetting_StringCollection.Count: " +
                                                    Settings.Default.CSharpClassLibrary_UserSetting_StringCollection
                                                            .Count);

            if (!Settings.Default.CSharpClassLibrary_UserSetting_StringCollection.Contains("GHI"))
                throw new InvalidOperationException(
                    "CSharpClassLibrary_UserSetting_StringCollection.Contains(\"GHI\"): " +
                    Settings.Default.CSharpClassLibrary_UserSetting_StringCollection);

            if (!Settings.Default.CSharpClassLibrary_UserSetting_StringCollection.Contains("JKL"))
                throw new InvalidOperationException(
                    "CSharpClassLibrary_UserSetting_StringCollection.Contains(\"JKL\"): " +
                    Settings.Default.CSharpClassLibrary_UserSetting_StringCollection);
        }

        /// <summary>Get the static counter</summary>
        public static int StaticCounter => counter;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static int counter;
    }
}
