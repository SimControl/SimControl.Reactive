// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

namespace SimControl.Samples.CSharp.ClassLibrary
{
    /// <summary>Log class.</summary>
    [Log]
    public class LogClass
    {
        /// <summary>Void method.</summary>
        public void VoidMethod() => logger.Trace("VoidMethod");

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
