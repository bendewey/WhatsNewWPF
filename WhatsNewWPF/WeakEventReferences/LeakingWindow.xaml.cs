using System;
using System.Windows;

namespace WhatsNewWPF.WeakEventReferences
{
    /// <summary>
    /// Interaction logic for LeakingWindows.xaml
    /// </summary>
    public partial class LeakingWindow : Window
    {

        //Create 10 Mo to be more visible in the process explorer
        public byte[] data = new byte[10 * 1024 * 1024];

        public LeakingWindow()
        {
            InitializeComponent();

            //Traditional event subscription: memory leak !
            App.Current.MainWindow.Activated += MainWindow_Activated;
        }

        void MainWindow_Activated(object sender, EventArgs e)
        {
            //Do something here
        }
    }
}
