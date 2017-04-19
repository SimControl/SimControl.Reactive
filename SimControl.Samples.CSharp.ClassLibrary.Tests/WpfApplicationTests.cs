﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Samples.CSharp.WpfApplication;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    [Log]
    [TestFixture]
    public class WpfApplicationTestsWithInitializeAndCleanup: TestFrame
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SetUp]
        new public void SetUp()
        {
            context = RegisterTestAdapter(new DispatcherContextTestAdapter(this, "DispatcherContext", ApartmentState.STA));

            context.SendAssertTimeout(() => {
                window = new MainWindow();
                window.Show();
            });
        }

        [TearDown]
        new public void TearDown() => CatchTearDownExceptions(() => context.SendAssertTimeout(() => window.Close()));

        [Test, InteractiveTest, ExclusivelyUses(nameof(InteractiveTest))]
        public void WpfApplicationTests_DisplayWindow()
        {
            Task<bool> buttonPressed =
                context.PostAsync(() => window.DisplayTestMessageAsync("Press 'OK'\nJust some more text.",
                    DisableDebugTimeout(DefaultTestTimeout))).AssertTimeout();
            Assert.IsTrue(buttonPressed.AssertTimeout());

            buttonPressed = context.PostAssertTimeout(() => window.DisplayTestMessageAsync("Press 'Cancel'",
                DisableDebugTimeout(DefaultTestTimeout)));
            Assert.IsFalse(buttonPressed.AssertTimeout());
        }

        private DispatcherContextTestAdapter context;
        private MainWindow window;
    }

    [Log]
    [TestFixture]
    public class WpfApplicationTestsWithoutInitializeAndCleanup: TestFrame
    {
        [Test, InteractiveTest, ExclusivelyUses(nameof(InteractiveTest))]
        public void WpfApplicationTests_DisplayWindow()
        {
            using (var context = new DispatcherContextTestAdapter(this, "DispatcherContext", ApartmentState.STA))
            {
                MainWindow window = null;

                context.PostAssertTimeout(() => {
                    window = new MainWindow();
                    window.Show();
                });

                Task<bool> buttonPressed = context.PostAsync(() => window.DisplayTestMessageAsync("Press 'OK'\nJust some more text.",
                DisableDebugTimeout(DefaultTestTimeout))).AssertTimeout();
                Assert.IsTrue(buttonPressed.AssertTimeout());

                buttonPressed = context.PostAssertTimeout(() => window.DisplayTestMessageAsync("Press 'Cancel'",
                    DisableDebugTimeout(DefaultTestTimeout)));
                Assert.IsFalse(buttonPressed.AssertTimeout());

                context.SendAssertTimeout(() => window.Close());
            }
        }
    }
}
