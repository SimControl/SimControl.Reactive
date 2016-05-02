using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SimControl.Samples.CSharp.WpfApplication
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow: Window, INotifyPropertyChanged
    {
        /// <summary>Default constructor.</summary>
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
        }

        /// <summary>Displays a test message.</summary>
        /// <param name="message">The message.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A Task&lt;bool&gt;</returns>
        public Task<bool> DisplayTestMessageAsync(string message, int timeout)
        {
            Text = message;

            Countdown = timeout/1000;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();

            testResponse = new TaskCompletionSource<bool>();
            return testResponse.Task;
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            testResponse.SetResult(false);
            timer.Stop();
            Countdown = 0;
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            testResponse.SetResult(true);
            timer.Stop();
            Countdown = 0;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Countdown--;
            if (Countdown == 0)
            {
                timer.Stop();
                testResponse.TrySetException(new TimeoutException());
            }
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Gets or sets the countdown.</summary>
        /// <value>The countdown.</value>
        public int Countdown
        {
            get { return countdown; }
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
            get { return text; }
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        private int countdown = 99;
        private TaskCompletionSource<bool> testResponse;
        private string text = "";
        private readonly DispatcherTimer timer = new DispatcherTimer();
    }
}
