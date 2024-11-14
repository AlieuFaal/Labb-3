using Labb_3.Commands;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Labb_3.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        
        private readonly DispatcherTimer timer;
        private int timeRemaining;

        private int CurrentQuestionIndex;
       
        private int correctAnswers;
        public int CorrectAnswers
        {
            get => correctAnswers;
            private set
            {
                correctAnswers = value;
                OnPropertyChanged();
            }
        }

        private string currentQuestionText;
        public string CurrentQuestionText
        {
            get => currentQuestionText;
            private set
            {
                currentQuestionText = value;
                OnPropertyChanged();
            }
        }

        public string TimerText => $"{timeRemaining}";
        private string answer1;
        private string answer2;
        private string answer3;
        private string answer4;

        private string answer1State;
        private string answer2State;
        private string answer3State;
        private string answer4State;


        public string Answer1
        {
            get => answer1;
            private set
            {
                answer1 = value;
                OnPropertyChanged();
            }
        }

        public string Answer2
        {
            get => answer2;
            private set
            {
                answer2 = value;
                OnPropertyChanged();
            }
        }

        public string Answer3
        {
            get => answer3;
            private set
            {
                answer3 = value;
                OnPropertyChanged();
            }
        }

        public string Answer4
        {
            get => answer4;
            private set
            {
                answer4 = value;
                OnPropertyChanged();
            }
        }


        public string Answer1State
        {
            get => answer1State;
            private set
            {
                answer1State = value;
                OnPropertyChanged();
            }
        }

        public string Answer2State
        {
            get => answer2State;
            private set
            {
                answer2State = value;
                OnPropertyChanged();
            }
        }

        public string Answer3State
        {
            get => answer3State;
            private set
            {
                answer3State = value;
                OnPropertyChanged();
            }
        }

        public string Answer4State
        {
            get => answer4State;
            private set
            {
                answer4State = value;
                OnPropertyChanged();
            }
        }



        public ICommand AnswerCommand { get; }



        public void StartQuiz()
        {
            timeRemaining = mainWindowViewModel.ActivePack.Model.TimeLimit;
            CurrentQuestionIndex = 0;
            CorrectAnswers = 0;
            timer.Start();
            LoadNextQuestion();
        }

        public void StopQuiz()
        {
            timer.Stop();
            mainWindowViewModel.StopQuiz();
            //MessageBox.Show($"Quiz Over! You got {CorrectAnswers} out of {mainWindowViewModel.ActivePack.Questions.Count} correct.");
        }
        public void StopQuiz2()
        {
            timer.Stop();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timeRemaining--;
            OnPropertyChanged(nameof(TimerText));
            if (timeRemaining <= 0)
            {
                StopQuiz();
            }
        }

        private void LoadNextQuestion()
        {
            if (CurrentQuestionIndex >= mainWindowViewModel.ActivePack.Questions.Count)
            {
                StopQuiz();
                return;
            }

            var question = mainWindowViewModel.ActivePack.Questions[CurrentQuestionIndex];
            var shuffledAnswers = question.ShuffleAnswers();
            var answers = new[] { question.CorrectAnswer, question.IncorrectAnswer1, question.IncorrectAnswer2, question.IncorrectAnswer3 };
        
            if (question != null)
            {
                CurrentQuestionText = question.Query;
                Answer1 = shuffledAnswers[0];
                Answer2 = shuffledAnswers[1];
                Answer3 = shuffledAnswers[2];
                Answer4 = shuffledAnswers[3];
                OnPropertyChanged(nameof(Answer1));
                OnPropertyChanged(nameof(Answer2));
                OnPropertyChanged(nameof(Answer3));
                OnPropertyChanged(nameof(Answer4));
            }
        }

        private async void CheckAnswer(int answerIndex)
        {
            //if (answerIndex < 0 || answerIndex > 3)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(answerIndex), "Answer index must be between 0 and 3.");
            //}

            var question = mainWindowViewModel.ActivePack.Questions[CurrentQuestionIndex];
            var selectedAnswer = answerIndex switch
            {
                1 => Answer1,
                2 => Answer2,
                3 => Answer3,
                4 => Answer4,
                _ => throw new ArgumentOutOfRangeException(nameof(answerIndex))
            };

            if (selectedAnswer == question.CorrectAnswer)
            {
                CorrectAnswers++;
                SetAnswerState(answerIndex, "Correct");
            }
            else
            {
                SetAnswerState(answerIndex, "Incorrect");
            }

            await Task.Delay(1000);

            ResetAnswerStates();

            CurrentQuestionIndex++;
            LoadNextQuestion();
        }

        private void SetAnswerState(int answerIndex, string state)
        {
            switch (answerIndex)
            {
                case 1:
                    Answer1State = state;
                    break;
                case 2:
                    Answer2State = state;
                    break;
                case 3:
                    Answer3State = state;
                    break;
                case 4:
                    Answer4State = state;
                    break;
            }
        }

        private void ResetAnswerStates()
        {
            Answer1State = string.Empty;
            Answer2State = string.Empty;
            Answer3State = string.Empty;
            Answer4State = string.Empty;
        }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;
            AnswerCommand = new RelayCommand<int>(CheckAnswer);
        }
    }
}
