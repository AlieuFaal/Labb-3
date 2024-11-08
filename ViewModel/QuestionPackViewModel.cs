using Labb_3.Commands;
using Labb_3.Model;
using Labb_3.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Labb_3.ViewModel
{
    public class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPackService _questionPackService;

        public ObservableCollection<QuestionPackViewModel> Packs { get; set; } = new ObservableCollection<QuestionPackViewModel>();
        
        public ObservableCollection<Question> Questions { get; }
        
        public QuestionPack Model => model;
        private readonly QuestionPack model;
        
        private QuestionPack? selectedQuestionPack;
        public QuestionPack? SelectedQuestionPack
        {
            get => selectedQuestionPack;
            set
            {
                if (selectedQuestionPack != value)
                {
                    selectedQuestionPack = value;
                    OnPropertyChanged();
                    OnSelectedQuestionPackChanged();
                }
            }
        }

        public ICommand CreatePackCommand { get; }

        public ICommand CancelCommand { get; }

        public string PackName
        {
            get => model.Name;
            set
            {
                model.Name = value;
                OnPropertyChanged();
            }
        }

        public Difficulty PackDifficulty
        {
            get => model.Difficulty;
            set
            {
                if (model.Difficulty != value)
                {
                    model.Difficulty = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PackTimeLimit
        {
            get => model.TimeLimit;
            set
            {
                model.TimeLimit = value;
                OnPropertyChanged();
            }
        }

        private void OnSelectedQuestionPackChanged()
        {
            if (selectedQuestionPack != null)
            {
               OnPropertyChanged(nameof(PackName));
               OnPropertyChanged(nameof(PackDifficulty));
               OnPropertyChanged(nameof(PackTimeLimit));
               Questions.Clear();
                
                foreach (var question in selectedQuestionPack.Questions)
                {
                    Questions.Add(question);
                }
            }
        }

        private async void CreatePack()
        {
            if (string.IsNullOrWhiteSpace(PackName) || PackTimeLimit <= 0)
            {
                MessageBox.Show("Please fill in all fields with valid values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newPack = new QuestionPack(PackName, PackDifficulty, PackTimeLimit);
            var newPackViewModel = new QuestionPackViewModel(newPack);
            
            Packs.Add(newPackViewModel);

            if (_questionPackService == null)
            {
                MessageBox.Show("QuestionPackService is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            await _questionPackService.SaveQuestionPacksAsync(Packs);

            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
        }

        private void Cancel()
        {
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
        }
       
        //public QuestionPackViewModel()
        //{
        //    this.model = new QuestionPack();
        //    this.Questions = new ObservableCollection<Question>();

        //    CancelCommand = new DelegateCommand(_ => Cancel());
        //    CreatePackCommand = new DelegateCommand(_ => CreatePack());
        //    _questionPackService = new QuestionPackService();
        //}

        public QuestionPackViewModel(QuestionPack pack)
        {
            this.model = pack;
            this.Questions = new ObservableCollection<Question>(pack.Questions);

            CancelCommand = new DelegateCommand(_ => Cancel());
            CreatePackCommand = new DelegateCommand(_ => CreatePack());
            _questionPackService = new QuestionPackService();
        }
    }
}