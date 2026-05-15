using System;
using System.Diagnostics;
using System.Windows;
using WorldCupMVVM.Views;

namespace MVVM_Champ
{
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                
                var startupWindow = new StartupWindow();
                startupWindow.Show();
            }
            catch (Exception ex)
            {
                var message = $"Ошибка при запуске приложения:\n\n{ex}";
                Debug.WriteLine(message);
                MessageBox.Show(message, "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            var message = $"Необработанное исключение:\n\n{ex?.ToString()}";
            Debug.WriteLine(message);
            MessageBox.Show(message, "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var message = $"Ошибка диспетчера:\n\n{e.Exception}";
            Debug.WriteLine(message);
            MessageBox.Show(message, "Ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}


