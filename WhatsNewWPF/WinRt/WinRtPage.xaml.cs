using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Windows.System.UserProfile;
using Windows.UI.Notifications;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;

namespace WhatsNewWPF.WinRt
{
    public partial class WinRtPage : Page
    {
        private bool _isRegistered;
        private const string APP_ID = "Whats New in WPF";

        public WinRtPage()
        {
            InitializeComponent();
        }

        private void SendToast_Click(object sender, RoutedEventArgs e)
        {
            // http://msdn.microsoft.com/en-us/library/windows/apps/hh802768.aspx
            var toast = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            toast.GetElementsByTagName("text").First().InnerText = "Toast Message from WPF";
            
            var notification = new ToastNotification(toast);
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(notification);
        }

        private void AccelerometerStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (!_isRegistered)
            {
                Windows.Devices.Sensors.Accelerometer.GetDefault().ReadingChanged += WinRtPage_ReadingChanged;
                _isRegistered = true;
            }
            else
            {
                Windows.Devices.Sensors.Accelerometer.GetDefault().ReadingChanged -= WinRtPage_ReadingChanged;
                _isRegistered = false;
                AccelerometerText.Text = "";
            }   
        }

        void WinRtPage_ReadingChanged(Windows.Devices.Sensors.Accelerometer sender, Windows.Devices.Sensors.AccelerometerReadingChangedEventArgs args)
        {
            var reading = args.Reading;
            App.Current.Dispatcher.Invoke(() =>
            {
                AccelerometerText.Text = string.Format("AccelerationX: {0:N2}\r\nAccelerationY: {1:N2}\r\nAccelerationZ: {2:N2}", reading.AccelerationX, reading.AccelerationY, reading.AccelerationZ);     
            });
        }

        #region Add Shortcut
        private bool IsAppLinkExists()
        {
            string defaultPath = string.Format(@"{0}\Microsoft\Windows\Start Menu\Programs\{1}.lnk",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                APP_ID);

            return File.Exists(defaultPath) == false ? CreateApplicationShortcut(defaultPath) : true;
        }

        private bool CreateApplicationShortcut(string defaultPath)
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            var newShortcut = (IShellLinkW)new CShellLink();

            // Create a shortcut to the exe
            ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
            ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));

            // Open the shortcut property store, set the AppUserModelId property
            var newShortcutProperties = (IPropertyStore)newShortcut;

            using (PropVariant appId = new PropVariant(APP_ID))
            {
                ErrorHelper.VerifySucceeded(newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appId));
                ErrorHelper.VerifySucceeded(newShortcutProperties.Commit());
            }

            // Commit the shortcut to disk
            var newShortcutSave = (IPersistFile)newShortcut;

            ErrorHelper.VerifySucceeded(newShortcutSave.Save(defaultPath, true));
            return true;
        }

        private void CreateShortcut_Click(object sender, RoutedEventArgs e)
        {
            IsAppLinkExists();
        }
        #endregion
    }
}
