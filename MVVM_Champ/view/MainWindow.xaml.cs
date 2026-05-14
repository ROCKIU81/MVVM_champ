using System.Configuration;
using System.Diagnostics;
using System.Windows;
using WorldCupMVVM;
using WorldCupMVVM.ViewModels;

namespace MVVM_Champ
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                string connectionString = BuildConnectionString();
                var serviceContainer = new ServiceContainer(connectionString);
                var mainViewModel = serviceContainer.GetService<MainViewModel>();

                DataContext = mainViewModel;
            }
            catch (Exception ex)
            {
                var details = ex.ToString();
                Debug.WriteLine(details);
                MessageBox.Show($"Ошибка при инициализации:\n{details}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private static string BuildConnectionString()
        {
            var configuredConnection = ConfigurationManager.ConnectionStrings["WorldCupDb"]?.ConnectionString;
            if (!string.IsNullOrWhiteSpace(configuredConnection))
            {
                return configuredConnection;
            }

            var host = Environment.GetEnvironmentVariable("WC_DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("WC_DB_PORT") ?? "5432";
            var database = Environment.GetEnvironmentVariable("WC_DB_NAME") ?? "world_cup";
            var username = Environment.GetEnvironmentVariable("WC_DB_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("WC_DB_PASSWORD") ?? "postgres";

            return $"Host={host};Port={port};Database={database};Username={username};Password={password};Include Error Detail=true";
        }
    }
}
