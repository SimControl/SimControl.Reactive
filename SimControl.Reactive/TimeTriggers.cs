// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.Contracts;

// TODO: CR

namespace SimControl.Reactive
{
    /// <summary>Compute a <see cref="DateTime"/> point in time.</summary>
    /// <returns><see cref="DateTime"/> point in time</returns>
    public delegate DateTime DateTimeExpression();

    /// <summary>Compute a <see cref="TimeSpan"/>
    /// </summary>
    /// <returns>Time span</returns>
    public delegate TimeSpan TimeSpanExpression();

    /// <summary>Trigger specifying a point in time.</summary>
    public sealed class DateTimeTrigger: TimeTrigger
    {
        /// <summary>Initializes a new instance of the <see cref="DateTimeTrigger"/> class.</summary>
        /// <param name="expression">Expression for computing the <see cref="DateTime"/> point in time.</param>
        public DateTimeTrigger(DateTimeExpression expression)
        {
            Contract.Requires(expression != null);

            this.expression = expression;

            throw new NotImplementedException();
        }

        internal override void Next()
        {
            Due = expression.Invoke();
        }

        private readonly DateTimeExpression expression;
    }

    /// <summary>Trigger specifying a time span.</summary>
    public sealed class TimeSpanTrigger: TimeTrigger
    {
        /// <summary>Initializes a new instance of the <see cref="TimeSpanTrigger"/> class.</summary>
        /// <param name="expression">Expression for computing the <see cref="TimeSpan"/>.</param>
        public TimeSpanTrigger(TimeSpanExpression expression)
        {
            Contract.Requires(expression != null);

            this.expression = expression;
        }

        internal override void Next()
        {
            Due = DateTime.Now + expression.Invoke();
        }

        private readonly TimeSpanExpression expression;
    }

    /// <summary>Base class for time triggers.</summary>
    public abstract class TimeTrigger: Trigger
    {
        internal abstract void Next();
        internal DateTime Due { get; set; }
    }
}
