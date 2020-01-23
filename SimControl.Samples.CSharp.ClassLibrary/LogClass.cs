// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using NLog;
using SimControl.Log;

namespace SimControl.Samples.CSharp.ClassLibrary
{
    [Log]
    public class LogClass
    {
        public void VoidMethod()
        {
            logger.Trace("VoidMethod");
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
