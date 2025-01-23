using Labb_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Labb_3.Dialogs
{
    /// <summary>
    /// Interaction logic for RemoveCategoryDialog.xaml
    /// </summary>
    public partial class RemoveCategoryDialog : Window
    {
        public string CategoryName { get; private set; }

        public RemoveCategoryDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            CategoryName = (string)ComboBox.SelectedItem;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
