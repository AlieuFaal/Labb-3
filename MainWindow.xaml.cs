using Labb_3.ViewModel;
using Labb_3.Model;
using System.Windows;
using System.ComponentModel;
using Labb_3.Services;

namespace Labb_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.LoadPacks();
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            _mainWindowViewModel.SaveQuestion();
        }
    }
}