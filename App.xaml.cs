using Labb_3.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Labb_3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnExit(ExitEventArgs e)
        {
            if (Current.MainWindow?.DataContext is MainWindowViewModel viewModel)
            {
                await viewModel.OnApplicationExitAsync();
            }
            base.OnExit(e);
        }
    }
}
