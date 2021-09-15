// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SimControl.Samples.CSharp.WpfApp
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow: Window
    {
        /// <summary>Default constructor.</summary>
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromSeconds(1);
        }

        /// <summary>Displays a test message.</summary>
        /// <param name="message">The message.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A Task&lt;bool&gt;</returns>
        public Task<bool> DisplayTestMessageAsync(string message, int timeout)
        {
            Text = message;

            Countdown = timeout/1000;
            timer.Start();

            response = new();
            return response.Task;
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Stop();
            response.SetResult(false);
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            Stop();
            response.SetResult(true);
        }

        private void Stop()
        {
            timer.Stop();
            Countdown = 0;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (--Countdown <= 0)
            {
                Stop();
                response.TrySetException(new TimeoutException());
            }
        }

        /// <inheritdoc/>
        public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;

        /// <summary>Gets or sets the countdown.</summary>
        /// <value>The countdown.</value>
        public int Countdown
        {
            get => countdown;
            set
            {
                countdown = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CountDown"));
            }
        }

        /// <summary>Gets or sets the text.</summary>
        /// <value>The text.</value>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        private readonly DispatcherTimer timer = new();
        private int countdown;
        private TaskCompletionSource<bool>? response;
        private string text = "";
    }
}
