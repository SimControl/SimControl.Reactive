// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// REVIEW

using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace SimControl.Samples.CSharp.WpfApp;

/// <summary>Interaction logic for MainWindow.xaml</summary>
public partial class MainWindow : Window
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

        Countdown = timeout / 1000;
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
            _ = response.TrySetException(new TimeoutException());
        }
    }

    /// <inheritdoc/>
    public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;

    /// <summary>Gets or sets the countdown.</summary>
    /// <value>The countdown.</value>
    public int Countdown
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CountDown"));
        }
    }

    /// <summary>Gets or sets the text.</summary>
    /// <value>The text.</value>
    public string Text
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
        }
    } = "";

    private readonly DispatcherTimer timer = new();
    private TaskCompletionSource<bool>? response;
}
