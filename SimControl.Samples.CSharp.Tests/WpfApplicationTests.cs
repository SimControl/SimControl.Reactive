﻿// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO CR

/*
using System.Threading;
using System.Threading.Tasks;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.LogEx;
using SimControl.Samples.CSharp.WpfApplication;
using SimControl.TestUtils;

namespace SimControl.Samples.CSharp.ClassLibraryEx.Tests
{
    [Log]
    [TestFixture]
    public class WpfApplicationTestsWithInitializeAndCleanup : TestFrame
    {
        [SetUp]
        public new void SetUp()
        {
            context = RegisterTestAdapter(new DispatcherContextTestAdapter(this, "DispatcherContext", ApartmentState.STA));

            context.SendAssertTimeout(() => {
                window = new MainWindow();
                window.Show();
            });
        }

        [TearDown]
        public new void TearDown() => CatchTearDownExceptions(() => context.SendAssertTimeout(window.Close));

        [Test, InteractiveTest, ExclusivelyUses(nameof(InteractiveTest))]
        public void WpfApplicationTests_DisplayWindow()
        {
            Task<bool> buttonPressed =
                context.PostAsync(() => window.DisplayTestMessageAsync("Press 'OK'\nJust some more text.",
                    DisableDebugTimeout(DefaultTestTimeout))).ResultAssertTimeout();
            Assert.IsTrue(buttonPressed.ResultAssertTimeout());

            buttonPressed = context.PostAssertTimeout(() => window.DisplayTestMessageAsync("Press 'Cancel'",
                DisableDebugTimeout(DefaultTestTimeout)));
            Assert.IsFalse(buttonPressed.ResultAssertTimeout());
        }

        private DispatcherContextTestAdapter context;
        private MainWindow window;
    }

    [Log]
    [TestFixture]
    public class WpfApplicationTestsWithoutInitializeAndCleanup : TestFrame
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
                DisableDebugTimeout(DefaultTestTimeout))).ResultAssertTimeout();
                Assert.IsTrue(buttonPressed.ResultAssertTimeout());

                buttonPressed = context.PostAssertTimeout(() => window.DisplayTestMessageAsync("Press 'Cancel'",
                    DisableDebugTimeout(DefaultTestTimeout)));
                Assert.IsFalse(buttonPressed.ResultAssertTimeout());

                context.SendAssertTimeout(window.Close);
            }
        }
    }
}
*/
