using Labb_3.Commands;
using Labb_3.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Labb_3.ViewModel
{
    public class ConfigurationViewModel : ViewModelBase
    {
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }
        
        private readonly MainWindowViewModel? mainWindowViewModel;

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            
        }
    }
}
