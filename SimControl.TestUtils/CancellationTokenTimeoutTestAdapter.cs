// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using NLog;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for creating cancellation tokens with a specified timeout.</summary>
    public class CancellationTokenTimeoutTestAdapter: TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="CancellationTokenTimeoutTestAdapter"/> class.</summary>
        /// <remarks>The timeout is set to <see cref="TestFrame.DefaultTestTimeout"/></remarks>
        public CancellationTokenTimeoutTestAdapter()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TestFrame.DisableDebugTimeout(TestFrame.DefaultTestTimeout));
        }

        /// <summary>Initializes a new instance of the <see cref="CancellationTokenTimeoutTestAdapter"/> class.</summary>
        /// <param name="timeout">The timeout in ms.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public CancellationTokenTimeoutTestAdapter(int timeout)
        {
            cancellationTokenSource.CancelAfter(TestFrame.DisableDebugTimeout(timeout));
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void Dispose(bool disposing)
        {
            if (disposing && cancellationTokenSource != null)
                try { cancellationTokenSource.Cancel(); }
                catch (Exception e) { logger.Warn(e, MethodBase.GetCurrentMethod().ToString()); }
                finally
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                }
        }

        /// <summary>Gets the token.</summary>
        /// <value>The token.</value>
        public CancellationToken Token => cancellationTokenSource.Token;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private CancellationTokenSource cancellationTokenSource;
    }
}
