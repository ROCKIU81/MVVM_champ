using System.Windows;
using WorldCupMVVM.Services;

namespace WorldCupMVVM.Views
{
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private async void TestDataButton_Click(object sender, RoutedEventArgs e)
        {
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();
            
            var mainWindow = new MainWindow();
            mainWindow.Show();
            
            this.Close();
        }

        private void DatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            
            this.Close();
        }
    }
}
