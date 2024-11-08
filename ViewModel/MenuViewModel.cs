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
                    OnPropertyChanged(nameof(ConfigurationViewModel.ActivePack));
                    OnActivePackChanged();
                }
            }
        }

        public ICommand OpenCreatePackDialogCommand => new DelegateCommand(_ => OpenCreatePackDialog());

        public ICommand SetActivePackCommand { get; }

        public ICommand DeleteActivePackCommand { get; }

        private void SetActivePack(QuestionPackViewModel pack)
        {
            ActivePack = pack;
        }

        private void OpenCreatePackDialog()
        {
            var viewModel = new QuestionPackViewModel(new QuestionPack());
            var dialog = new CreateNewPackDialog(viewModel);
            dialog.ShowDialog();
        }

        private void LoadQuestionPacks()
        {
            Packs.Add(new QuestionPackViewModel(new QuestionPack("My Question Pack")));
        }

        public void DeleteQuestionPack(object obj)
        {
            if (ActivePack != null)
            {
                MessageBoxResult result;
                var DeleteMessage = $"Are you sure you want to delete {ActivePack.PackName}?";
                result = MessageBox.Show(DeleteMessage, "Delete Question Pack.", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Packs.Remove(ActivePack);
                    ActivePack = Packs.FirstOrDefault();
                    _ = _questionPackService.SaveQuestionPacksAsync(Packs);
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }
        
        private bool CanDeletePack()
        {
            return ActivePack != null;
        }

        private void OnActivePackChanged()
        {
            if(DeleteActivePackCommand != null)
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
                LoadQuestionPacks();
                await _questionPackService.SaveQuestionPacksAsync(Packs);
            }
        }

        public MenuViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            _questionPackService = new QuestionPackService();

            Packs = new ObservableCollection<QuestionPackViewModel>();

            ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));

            Task.Run(async () => await LoadQuestionPacksAsync());

            SetActivePackCommand = new RelayCommand<QuestionPackViewModel>(SetActivePack);

            DeleteActivePackCommand = new DelegateCommand(_ => DeleteQuestionPack(_), _ => CanDeletePack());
        }
    }
}