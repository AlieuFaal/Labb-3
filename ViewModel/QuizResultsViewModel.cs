using Labb_3.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Labb_3.ViewModel
{
    public class QuizResultsViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly PlayerViewModel playerViewModel;

        public string ResultText
        {
            get
            {
                if (playerViewModel?.CorrectAnswers == null || mainWindowViewModel?.ActivePack?.Questions == null)
                {
                    return "Result not available.";
                }
                return $"You got {playerViewModel.CorrectAnswers} out of {mainWindowViewModel.ActivePack.Questions.Count} correct!";
            }
        }


        public ICommand RestartCommand { get; }

        private void RestartQuiz(object parameter)
        {
            mainWindowViewModel.StartQuiz();
        }

        public QuizResultsViewModel(MainWindowViewModel mainWindowViewModel, PlayerViewModel playerViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.playerViewModel = playerViewModel;

            playerViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(playerViewModel.CorrectAnswers))
                {
                    OnPropertyChanged(nameof(ResultText));
                }
            };

            mainWindowViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(mainWindowViewModel.ActivePack))
                {
                    OnPropertyChanged(nameof(ResultText));
                }
            };

            RestartCommand = new DelegateCommand(RestartQuiz);
        }
    }
}