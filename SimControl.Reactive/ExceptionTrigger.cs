// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;

// TODO CR

namespace SimControl.Reactive
{
    /// <summary>Exception trigger.</summary>
    public class ExceptionTrigger: Trigger
    {
        /// <summary>Constructor.</summary>
        /// <param name="exception">The exception.</param>
        public ExceptionTrigger(Exception exception)
        {
            // Contract.Requires(exception != null);

            exceptionType = exception.InnerException.GetType();
            this.exception = exception;
        }

        /// <summary>Constructor.</summary>
        /// <param name="exceptionType">Type of the exception.</param>
        protected ExceptionTrigger(Type exceptionType)
        {
            // Contract.Requires(exceptionType != null && (exceptionType == typeof(Exception) || exceptionType.IsSubclassOf(typeof(Exception))));

            this.exceptionType = exceptionType;
        }

        internal override bool Matches(Trigger trigger) => trigger is ExceptionTrigger other &&
            (exceptionType == other.exceptionType || other.exceptionType.IsSubclassOf(exceptionType));

        internal readonly Exception exception;
        internal readonly Type exceptionType;
    }

    /// <summary>Exception trigger.</summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Trigger"/>
    public sealed class ExceptionTrigger<T>: ExceptionTrigger, IGenericTrigger<T> where T : Exception
    {
        /// <summary>Default constructor.</summary>
        public ExceptionTrigger() : base(typeof(T)) { }
    }
}
