using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace WhatsNewWPF.Binding
{
    public partial class BindingPage : Page
    {
        public BindingPage()
        {
            InitializeComponent();

            BindingExpressionPanel.DataContext =
            DelayBindingPanel.DataContext = new CustomerViewModel();
        }

        void BindingPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var binding = NameTextBox.GetBindingExpression(TextBox.TextProperty);
            var textbox = binding.Target;
            var vm = binding.ResolvedSource;
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }

    public class AppSettings
    {
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        private static bool _enableNotifications;
        public static bool EnableNotifications
        {
            get { return _enableNotifications; }
            set
            {
                _enableNotifications = value;
                OnStaticPropertyChanged("EnableNotifications");
            }
        }

        public static void OnStaticPropertyChanged(string propertyName)
        {
            var handler = StaticPropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class CustomerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
