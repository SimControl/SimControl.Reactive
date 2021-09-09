// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using NLog;
using SimControl.Log;

// UNDONE implement

namespace SimControl.Samples.CSharp.ClassLibrary
{
    /// <summary>SampleClass implementation.</summary>
    [Log]
    public class SampleClass
    {
        static SampleClass() => logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(),
            typeof(SampleClass).AssemblyQualifiedName);

        /// <summary>Increment the static counter</summary>
        public static void IncrementStaticCounter() => staticCounter++;

        /// <summary>Does something</summary>
        /// <returns></returns>
        public bool DoSomething()
        {
            logger.Message(LogLevel.Info, LogMethod.GetCurrentMethodName(), nameof(DoSomething));

            counter++;

            return true;
        }

        /// <inheritdoc/>
        public override string ToString() => LogFormat.FormatObject(typeof(SampleClass), staticCounter, counter);

        /// <summary>Get the static counter</summary>
        public static int StaticCounter => staticCounter;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static int staticCounter;
        private int counter;
    }
}
