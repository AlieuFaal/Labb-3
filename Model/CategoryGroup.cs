using Labb_3.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3.Model
{
    public class CategoryGroup
    {
        public string Category { get; set; }
        public ObservableCollection<QuestionPackViewModel> QuestionPacks { get; set; }
    }
}
