﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimControl.Samples.CSharp.ClassLibraryOld.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.1.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CSharpClassLibrary_AppSetting_Default")]
        public string CSharpClassLibrary_AppSetting {
            get {
                return ((string)(this["CSharpClassLibrary_AppSetting"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CSharpClassLibrary_UserSetting_Default")]
        public string CSharpClassLibrary_UserSetting {
            get {
                return ((string)(this["CSharpClassLibrary_UserSetting"]));
            }
            set {
                this["CSharpClassLibrary_UserSetting"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>ABC</string>\r\n  <string>DEF</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection CSharpClassLibrary_UserSetting_StringCollection {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["CSharpClassLibrary_UserSetting_StringCollection"]));
            }
            set {
                this["CSharpClassLibrary_UserSetting_StringCollection"] = value;
            }
        }
    }
}