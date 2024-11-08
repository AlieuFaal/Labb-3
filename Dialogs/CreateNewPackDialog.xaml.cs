using Labb_3.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Labb_3.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewPackDialog.xaml
    /// </summary>
    public partial class CreateNewPackDialog : Window
    {
        public CreateNewPackDialog(QuestionPackViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
