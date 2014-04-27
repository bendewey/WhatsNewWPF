using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WhatsNewWPF.Validation
{
    public partial class ValidationPage : Page
    {
        public ValidationPage()
        {
            InitializeComponent();

            DataContext = new LoginViewModel();
        }

        public class LoginViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

            public ConcurrentDictionary<string, List<string>> Errors { get; set; }

            public bool HasErrors
            {
                get { return Errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
            }

            private string _username;
            public string Username
            {
                get { return _username; }
                set
                {
                    _username = value;
                    ValidateProperty("Username");
                    OnPropertyChanged("Username");
                }
            }

            public LoginViewModel()
            {
                Errors = new ConcurrentDictionary<string, List<string>>();
            }

            public void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            public void OnErrorsChanged(string propertyName)
            {
                var handler = ErrorsChanged;
                if (handler != null)
                {
                    handler(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }

            public IEnumerable GetErrors(string propertyName)
            {
                if (!Errors.ContainsKey(propertyName))
                {
                    return Enumerable.Empty<string>();
                }
                return Errors[propertyName];
            }

            private async void ValidateProperty(string propertyName)
            {
                if (propertyName == "Username")
                {
                    ClearError("Username");
                    await Task.Delay(2000);
                    if (Username == "user1")
                    {
                        AddOrCreateError("Username", "Username already exists");    
                    }
                    OnErrorsChanged("Username");
                }
            }

            private void AddOrCreateError(string propertyName, string errorMessage)
            {
                if (!Errors.ContainsKey(propertyName))
                {
                    Errors.TryAdd(propertyName, new List<string>());
                }
                Errors[propertyName].Add(errorMessage);
            }

            private void ClearError(string propertyName)
            {
                if (Errors.ContainsKey(propertyName))
                {
                    List<string> list;
                    Errors.TryRemove(propertyName, out list);
                }
            }
        }
    }
}
