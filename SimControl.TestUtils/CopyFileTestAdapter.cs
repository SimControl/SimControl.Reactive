﻿// (C) KEBA AG Linz, Austria. All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Threading;
using SimControl.Log;
using NUnit.Framework;

namespace SimControl.TestUtils
{
    /// <summary>Test adapter for copying files.</summary>
    public class CopyFileTestAdapter : TestAdapter
    {
        /// <summary>Initializes a new instance of the <see cref="CopyFileTestAdapter"/> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public CopyFileTestAdapter( string source, string target )
        {
            Contract.Requires( !string.IsNullOrEmpty( source ) );
            Contract.Requires( !string.IsNullOrEmpty( target ) );

            destination = TestContext.CurrentContext.TestDirectory + "\\" + target;

            File.Copy( TestContext.CurrentContext.TestDirectory + "\\" + source, destination, true );
        }

        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            if( disposing && File.Exists( destination ) )
                File.Delete( destination );
        }

        private readonly string destination;
    }
}
