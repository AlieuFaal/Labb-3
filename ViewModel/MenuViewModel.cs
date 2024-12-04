using Labb_3.Commands;
using Labb_3.Dialogs;
using Labb_3.Model;
using Labb_3.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Labb_3.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private readonly QuestionPackService _questionPackService;
        public ObservableCollection<QuestionPackViewModel> Packs { get; }

        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                if (_activePack != value)
                {
                    _activePack = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ActivePack));
                    OnActivePackChanged();
                }
            }
        }
        


        public ICommand OpenCreatePackDialogCommand => new DelegateCommand(_ => OpenCreatePackDialog());
       
        public ICommand OpenPackOptionsDialogCommand => new DelegateCommand(_ => OpenPackOptionsDialog());

        public ICommand SetActivePackCommand { get; }

        public ICommand DeleteActivePackCommand { get; }

        public ICommand ExitAppCommand => new DelegateCommand(_ => Application.Current.Shutdown());



        private void SetActivePack(QuestionPackViewModel pack)
        {
            ActivePack = pack;
            if (mainWindowViewModel != null)
            {
                mainWindowViewModel.ActivePack = pack;
                mainWindowViewModel.ConfigurationViewModel.ActivePack = pack;
            }
            OnPropertyChanged(nameof(ActivePack));
        }

        private void OpenCreatePackDialog()
        {
            var viewModel = new QuestionPackViewModel(new QuestionPack());
            var dialog = new CreateNewPackDialog(viewModel);
            dialog.ShowDialog();
        }

        private void InsertDefaultPack()
        {
            var defaultPack = new QuestionPackViewModel(new QuestionPack
            {
                Name = "Default Pack",
                Difficulty = Difficulty.Medium,
                TimeLimit = 120,
                Questions = new List<Question>
                {
                    new Question("What is the capital of France?", "Paris", "London", "Berlin", "Madrid")
                }
            });

            Packs.Add(defaultPack);
        }

        public async void DeleteQuestionPack(object obj)
        {
            if (ActivePack != null)
            {
                MessageBoxResult result;
                var DeleteMessage = $"Are you sure you want to delete {ActivePack.PackName}?";
                result = MessageBox.Show(DeleteMessage, "Delete Question Pack.", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _questionPackService.RemoveQuestionPackAsync(ActivePack);
                    Packs.Remove(ActivePack);
                    ActivePack = Packs.FirstOrDefault();
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }
        
        public bool IsActivePackSelected => ActivePack != null;

        private bool CanDeletePack() => ActivePack != null && Packs.Count > 1;

        private void OnActivePackChanged()
        {
            OnPropertyChanged(nameof(IsActivePackSelected));
            if (DeleteActivePackCommand != null)
            {
                ((DelegateCommand)DeleteActivePackCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadQuestionPacksAsync()
        {
            Packs.Clear();
            var packs = await _questionPackService.LoadQuestionPacksAsync();
            foreach (var pack in packs)
            {
                Packs.Add(pack);
            }

            if (Packs.Count == 0)
            {
                InsertDefaultPack();
                await _questionPackService.SaveQuestionPacksAsync(Packs);
            }
        }

        private void OpenPackOptionsDialog()
        {
            if (ActivePack != null)
            {
                var originalName = ActivePack.Model.Name;
                var dialog = new PackOptionsDialog(ActivePack); 
                dialog.Closed += async (s, e) =>
                {
                    //MessageBox.Show($"Updating Pack: {ActivePack.Model.Name}");
                    await _questionPackService.UpdateQuestionPackAsync(ActivePack, originalName);
                };
                dialog.ShowDialog();
            }
        }



        public MenuViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            _questionPackService = new QuestionPackService();

            Packs = new ObservableCollection<QuestionPackViewModel>();
            Packs.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Packs));

            ActivePack = mainWindowViewModel?.ActivePack;

            Task.Run(async () => await LoadQuestionPacksAsync());

            //SetActivePackCommand = new DelegateCommand<QuestionPackViewModel>(SetActivePack);
            SetActivePackCommand = new DelegateCommand(param => SetActivePack((QuestionPackViewModel)param));
            DeleteActivePackCommand = new DelegateCommand(_ => DeleteQuestionPack(_), _ => CanDeletePack());
        }
    }
}