using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }
        
        private readonly MainWindowViewModel? mainWindowViewModel;
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
    }
}
