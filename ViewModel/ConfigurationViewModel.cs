using Labb_3.Commands;
using Labb_3.Dialogs;
using Labb_3.Model;
using Labb_3.Services;
using Labb_3.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Labb_3.ViewModel
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private readonly QuestionPackService _questionPackService;

        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                OnPropertyChanged(nameof(ActivePack));
            }
        }

        public ObservableCollection<Question> Questions { get; set; }
      
        public QuestionPack? Model => ActivePack?.Model;



        public string NewQuestionQuery { get; set; }
        public string NewCorrectAnswer { get; set; }
        public string NewIncorrectAnswer1 { get; set; }
        public string NewIncorrectAnswer2 { get; set; }
        public string NewIncorrectAnswer3 { get; set; }

        public bool IsQuestionSelected => SelectedQuestion != null;

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                if (_selectedQuestion != value)
                {
                    _selectedQuestion = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedQuestionQuery));
                    OnPropertyChanged(nameof(SelectedCorrectAnswer));
                    OnPropertyChanged(nameof(SelectedIncorrectAnswer1));
                    OnPropertyChanged(nameof(SelectedIncorrectAnswer2));
                    OnPropertyChanged(nameof(SelectedIncorrectAnswer3));
                    OnPropertyChanged(nameof(IsQuestionSelected));
                }
            }
        }

        public string? SelectedQuestionQuery
        {
            get => SelectedQuestion?.Query;
            set
            {
                if (SelectedQuestion != null && value != null && SelectedQuestion.Query != value)
                {
                    SelectedQuestion.Query = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? SelectedCorrectAnswer
        {
            get => SelectedQuestion?.CorrectAnswer;
            set
            {
                if (SelectedQuestion != null && value != null && SelectedQuestion.CorrectAnswer != value)
                {
                    SelectedQuestion.CorrectAnswer = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? SelectedIncorrectAnswer1
        {
            get => SelectedQuestion?.IncorrectAnswers?[0];
            set
            {
                if (SelectedQuestion != null && SelectedQuestion.IncorrectAnswers != null && SelectedQuestion.IncorrectAnswers[0] != value)
                {
                    SelectedQuestion.IncorrectAnswers[0] = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? SelectedIncorrectAnswer2
        {
            get => SelectedQuestion?.IncorrectAnswers?[1];
            set
            {
                if (SelectedQuestion != null && SelectedQuestion.IncorrectAnswers != null && SelectedQuestion.IncorrectAnswers[1] != value)
                {
                    SelectedQuestion.IncorrectAnswers[1] = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? SelectedIncorrectAnswer3
        {
            get => SelectedQuestion?.IncorrectAnswers?[2];
            set
            {
                if (SelectedQuestion != null && SelectedQuestion.IncorrectAnswers != null && SelectedQuestion.IncorrectAnswers[2] != value)
                {
                    SelectedQuestion.IncorrectAnswers[2] = value;
                    OnPropertyChanged();
                }
            }
        }




        private readonly ICommand _addQuestionCommand;
        public ICommand AddQuestionCommand => _addQuestionCommand;

        private readonly ICommand _removeQuestionCommand;
        public ICommand RemoveQuestionCommand => _removeQuestionCommand;



        private void AddQuestion()
        {
            if (ActivePack != null)
            {
                var newQuestion = new Question(NewQuestionQuery, NewCorrectAnswer, NewIncorrectAnswer1, NewIncorrectAnswer2, NewIncorrectAnswer3);
                ActivePack.Questions.Add(newQuestion);
                //await _questionPackService.SaveQuestionsAsync(ActivePack.Questions.ToList(), ActivePack);
                OnPropertyChanged(nameof(Questions));
                ClearNewQuestionFields();
            }
        }

        private bool CanRemoveQuestion(object? parameter)
        {
            return SelectedQuestion != null;
        }

        private async void RemoveQuestion(object? parameter)
        {
            if (ActivePack != null && SelectedQuestion != null)
            {
                await _questionPackService.RemoveQuestionAsync(ActivePack, SelectedQuestion);
                ActivePack.Questions.Remove(SelectedQuestion);
                OnPropertyChanged(nameof(ActivePack.Questions));
            }
        }

        private void ClearNewQuestionFields()
        {
            NewQuestionQuery = string.Empty;
            NewCorrectAnswer = string.Empty;
            NewIncorrectAnswer1 = string.Empty;
            NewIncorrectAnswer2 = string.Empty;
            NewIncorrectAnswer3 = string.Empty;
            OnPropertyChanged(nameof(NewQuestionQuery));
            OnPropertyChanged(nameof(NewCorrectAnswer));
            OnPropertyChanged(nameof(NewIncorrectAnswer1));
            OnPropertyChanged(nameof(NewIncorrectAnswer2));
            OnPropertyChanged(nameof(NewIncorrectAnswer3));
        }



        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            _questionPackService = new QuestionPackService();

            _addQuestionCommand = new DelegateCommand(_ => AddQuestion());
            _removeQuestionCommand = new DelegateCommand(RemoveQuestion);

            ActivePack = mainWindowViewModel?.ActivePack;

            Questions = new ObservableCollection<Question>();
            NewQuestionQuery = string.Empty;
            NewCorrectAnswer = string.Empty;
            NewIncorrectAnswer1 = string.Empty;
            NewIncorrectAnswer2 = string.Empty;
            NewIncorrectAnswer3 = string.Empty;
            _selectedQuestion = null;
        }
    }
}
