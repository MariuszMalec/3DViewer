using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace _3DViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupLogs();
            SetupExceptionHandling();
        }

        private void SetupLogs()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(@"C:\temp\3DView-logs.txt")
                .CreateLogger();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                MessageBox.Show(((Exception)e.ExceptionObject).Message, "Error");
                Log.Error((Exception)e.ExceptionObject, "App Error");
            };


            DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show(e.Exception.Message, "Error");
                e.Handled = true;
                Log.Error(e.Exception, "App Error");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                MessageBox.Show(e.Exception.Message, "Error");
                Log.Error(e.Exception, "App Error");
                e.SetObserved();
            };
        }
    }
}
