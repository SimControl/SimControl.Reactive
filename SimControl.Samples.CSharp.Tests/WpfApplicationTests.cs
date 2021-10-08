// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO implement

using System.Threading;
using System.Threading.Tasks;
using NCrunch.Framework;
using NUnit.Framework;
using SimControl.Log;
using SimControl.Samples.CSharp.WpfApp;
using SimControl.TestUtils;
using SimControl.Reactive;

namespace SimControl.Samples.CSharp.ClassLibrary.Tests
{
    //[Log]
    //[TestFixture, Apartment(ApartmentState.MTA)]
    //public class WpfApplicationTestsWithInitializeAndCleanup: TestFrame
    //{
    //    [SetUp]
    //    public new void SetUp()
    //    {
    //        SynchronizationContext.Current.Send(() => {
    //            window = new MainWindow();
    //            window.Show();
    //        });
    //    }

    // [TearDown] public new void TearDown() => CatchTearDownExceptions(() => context.SendAssertTimeout(window.Close));

    // [Test, InteractiveTest, ExclusivelyUses(nameof(InteractiveTestAttribute))] public void
    // WpfApplicationTests_DisplayWindow() { Task<bool> buttonPressed = context.PostAsync(() =>
    // window.DisplayTestMessageAsync("Press 'OK'\nJust some more text.",
    // DisableDebugTimeout(InteractiveTimeout))).ResultAssertTimeout();
    // Assert.IsTrue(buttonPressed.ResultAssertTimeout());

    // buttonPressed = context.PostAssertTimeout(() => window.DisplayTestMessageAsync("Press 'Cancel'",
    // DisableDebugTimeout(InteractiveTimeout))); Assert.IsFalse(buttonPressed.ResultAssertTimeout()); }

    //    private MainWindow window;
    //}

    //[Log]
    [TestFixture, Apartment(ApartmentState.STA)]
    public class WpfApplicationTestsWithoutInitializeAndCleanup: TestFrame
    {
        [Test, InteractiveTest, ExclusivelyUses(nameof(InteractiveTestAttribute))]
        public async Task WpfApplicationTests_DisplayWindow()
        {
            SynchronizationContext context = SynchronizationContext.Current;
            MainWindow? window = null;

            await context.SendAsync(() => {
                window = new MainWindow();
                window.Show();
            })/*.AssertTimeoutAsync()*/;
            ;
            Task<bool> buttonPressed = await context.SendAsync(() =>
                window.DisplayTestMessageAsync("Press 'OK'\nJust some more text.", InteractiveTimeout))/*.AssertTimeoutAsync()*/;
            ;
            await buttonPressed;
            //            Assert.That(buttonPressed/*.AssertTimeoutAsync(InteractiveTimeout + Timeout)*/, Is.True);
            ;
            Task<bool> buttonPressed2 = await context.SendAsync(() =>
                window.DisplayTestMessageAsync("Press 'Cancel'", InteractiveTimeout))/*.AssertTimeoutAsync()*/;

            await buttonPressed2;
            //Assert.That(buttonPressed2/*.AssertTimeoutAsync(InteractiveTimeout + Timeout)*/, Is.True);

            await context.SendAsync(() => window.Close())/*.AssertTimeoutAsync()*/;
        }
    }
}
