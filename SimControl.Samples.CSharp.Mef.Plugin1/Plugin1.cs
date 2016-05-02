﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel.Composition;
using SimControl.Log;
using SimControl.Samples.CSharp.Mef.Contracts;

namespace SimControl.Samples.CSharp.Mef.Plugin
{
    /// <summary>Sample MEF plugin.</summary>
    [Export(typeof(IPlugin))]
    [Log]
    public class Plugin1 : IPlugin
    {
        /// <summary>Get the resource Name</summary>
        /// <returns>Resource name</returns>
        public string ResourceName() => Resource.ResourceName;

#pragma warning disable 0649

        [Import]
        private IResource Resource;

#pragma warning restore 0649
    }
}
