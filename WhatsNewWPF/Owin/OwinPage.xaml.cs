using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Owin.Hosting;
using Nancy;
using Owin;

namespace WhatsNewWPF.Owin
{
    public partial class OwinPage : Page
    {
        private IDisposable _app = null;

        public OwinPage()
        {
            InitializeComponent();
            UpdateLabels();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _app = WebApp.Start<Startup>("http://localhost:12345");
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            var running = _app != null;
            StoppedLabel.Visibility = running ? Visibility.Collapsed : Visibility.Visible;
            RunningLabel.Visibility = running ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_app != null)
            {
                _app.Dispose();
                _app = null;
                UpdateLabels();
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://localhost:12345");
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }

    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = _ =>
            {
                var model = new { title = "Open Web Interface for .NET (OWIN)" };
                return View["home", model];
            };
        }
    }

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private byte[] favicon;

        protected override byte[] FavIcon
        {
            get { return this.favicon ?? (this.favicon = LoadFavIcon()); }
        }

        private byte[] LoadFavIcon()
        {
            //TODO: remember to replace 'AssemblyName' with the prefix of the resource
            using (var resourceStream = GetType().Assembly.GetManifestResourceStream("WhatsNewWPF.views.favicon.ico"))
            {
                var tempFavicon = new byte[resourceStream.Length];
                resourceStream.Read(tempFavicon, 0, (int)resourceStream.Length);
                return tempFavicon;
            }
        }
    }
}
