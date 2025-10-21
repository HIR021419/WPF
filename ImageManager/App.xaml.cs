using ImageManager.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ImageManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Views.MainWindow mainWindow = new Views.MainWindow();
            ViewModels.MainWindowViewModel viewModel = new ViewModels.MainWindowViewModel();
            mainWindow.DataContext = viewModel;
            mainWindow.InitializeComponent();
            mainWindow.Show();
        }
    }

}
