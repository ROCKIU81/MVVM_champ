using System.Windows;
using WorldCupMVVM.Services;
using MVVM_Champ;

namespace WorldCupMVVM.Views
{
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void TestDataButton_Click(object sender, RoutedEventArgs e)
        {
            var testDataService = new TestDataService();
            testDataService.FillTestDataAsync().Wait();
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            
            this.Close();
        }

        private void DatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            
            this.Close();
        }
    }
}
