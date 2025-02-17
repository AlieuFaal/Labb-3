﻿using Labb_3.Commands;
using Labb_3.Dialogs;
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

        private readonly MainWindowViewModel _mainWindowViewModel;

        private readonly MenuViewModel _menuViewModel;

        private ObservableCollection<QuestionPackViewModel> _packs;
        public ObservableCollection<QuestionPackViewModel> Packs
        {
            get => _packs;
            set
            {
                _packs = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Question> Questions { get; }

        private readonly QuestionPack model;
        public QuestionPack Model => model;



        public ICommand CreatePackCommand { get; }

        public ICommand CancelCommand { get; }



        public string PackName
        {
            get => model.Name;
            set
            {
                if (model.Name != value)
                {
                    model.Name = value;
                    OnPropertyChanged();
                }
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
                if (model.TimeLimit != value)
                {
                    model.TimeLimit = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PackCategory
        {
            get => model.Category;
            set
            {
                if (model.Category != value)
                {
                    model.Category = value;
                    OnPropertyChanged();
                }
            }
        }



        private async void CreatePack()
        {
            var existingPacks = await _questionPackService.LoadQuestionPacksAsync();
            if (existingPacks.Any(p => p.Model.Name == this.PackName))
            {
                MessageBox.Show("A pack with that name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newPack = new QuestionPackViewModel(new QuestionPack(PackName, PackDifficulty, PackTimeLimit, PackCategory), Packs, _mainWindowViewModel, _menuViewModel);
            Packs.Add(newPack);

            await _questionPackService.SaveQuestionPacksAsync(Packs);

            OnPropertyChanged(nameof(Packs));
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            _menuViewModel.CategorizePacks();
        }

        private void Cancel()
        {
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
        }



        public QuestionPackViewModel(QuestionPack pack, ObservableCollection<QuestionPackViewModel> packs)
        {
            _questionPackService = new QuestionPackService();

            this.model = pack;
            this.Questions = new ObservableCollection<Question>(pack.Questions);
            Packs = packs;

            CancelCommand = new DelegateCommand(_ => Cancel());
            CreatePackCommand = new DelegateCommand(_ => CreatePack());
        }

        public QuestionPackViewModel(QuestionPack pack, ObservableCollection<QuestionPackViewModel> packs, MainWindowViewModel mainWindowViewModel, MenuViewModel menuViewModel)
        {
            this.model = pack;
            this.Questions = new ObservableCollection<Question>(pack.Questions);
            Packs = packs;

            _mainWindowViewModel = mainWindowViewModel;
            _menuViewModel = menuViewModel;
            _questionPackService = new QuestionPackService();

            CancelCommand = new DelegateCommand(_ => Cancel());
            CreatePackCommand = new DelegateCommand(_ => CreatePack());
        }
    }
}