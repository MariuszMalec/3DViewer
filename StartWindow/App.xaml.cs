using BladeMill.BLL.Services;
using StartWindow.Service;
using StartWindow.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace StartWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var view = new CustomView();

            ShiftWindowOntoScreenHelper.ShiftWindowOntoScreen(view);

            view.Show();
        }

        public App()
        {
            this.DispatcherUnhandledException += this.App_DispatcherUnhandledException;
            try
            {
                //App.Settings = new Settings();
                //App.Settings.Load(); // this creates default settings.json file if does not exist
            }
            catch (Exception exception)
            {
                this.SaveException(exception);
                App.Current.Shutdown();
            }
        }
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.SaveException(e.Exception);
            MessageBox.Show("Aplikacja zostanie zamknieta. Wystapil nie oczekiwany blad!", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SaveException(Exception exception)
        {
            string text = $"{exception.Message}{Environment.NewLine}{exception.StackTrace}";
            File.WriteAllText(@"C:/temp/StartWindow.log", text);
        }
    }
}
