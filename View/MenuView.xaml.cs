using Labb_3.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Labb_3.View
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnCloseCommandExecuted));
        }

        private void OnCloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
