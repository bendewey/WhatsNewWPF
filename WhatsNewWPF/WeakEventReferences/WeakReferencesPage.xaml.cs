using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WhatsNewWPF.WeakEventReferences
{
    public partial class WeakReferencesPage : Page
    {
        public WeakReferencesPage()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(300);
            timer.Tick += TimerUpdate;
            timer.Start();
        }

        void TimerUpdate(object sender, EventArgs e)
        {
            MemoryUsageTextBlock.Text = string.Format("{0:N2}MB of memory used.", Process.GetCurrentProcess().PrivateMemorySize64 / 1024d / 1024d);
        }

        private void Leak_Click(object sender, RoutedEventArgs e)
        {
            LeakingWindow leakingWindow = new LeakingWindow();
            leakingWindow.Show();
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            WeakWindow weakWindow = new WeakWindow();
            weakWindow.Show();
        }

        private void GCCollect_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
