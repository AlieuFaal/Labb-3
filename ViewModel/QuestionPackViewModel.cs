using Labb_3.Model;
using System.Collections.ObjectModel;

namespace Labb_3.ViewModel
{
    internal class QuestionPackViewModel : ViewModelBase
    {

        private readonly QuestionPack model;

        public string Name
        {
            get => model.Name;
            set
            {
                model.Name = value;
                OnPropertyChanged();
            }
        }

        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                OnPropertyChanged();

            }
        }

        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                OnPropertyChanged();
            }
        }

        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            this.Questions = new ObservableCollection<Question>(model.Questions);
        }
        public ObservableCollection<Question> Questions { get; }
    }
}
