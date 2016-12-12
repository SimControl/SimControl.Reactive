// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NLog;
using SimControl.Log;
using System;
using System.Reflection;

namespace SimControl.Samples.CSharp.ConsoleApplication
{
    /// <summary>
    /// SampleClass implementation.
    /// </summary>
    public static class VerifyJitOptimization
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Run
        /// </summary>
        public static void Run()
        {
            const int a = 2*2;
            int count = 0;

            for (int i = 0; i < 3; i++)
                count += a*i;

            logger.Message(LogLevel.Debug, MethodBase.GetCurrentMethod(), "Count", count);

            try { MethodA(); }
            catch (InvalidOperationException e) { logger.Exception(LogLevel.Debug, MethodBase.GetCurrentMethod(), null, e); }
        }

        private static void BadMethod() { throw new InvalidOperationException("generic bad thing"); }

        private static void MethodA() => MethodB();

        private static void MethodB() => MethodC();

        private static void MethodC() => BadMethod();
    }
}