using Labb_3.Commands;
using Labb_3.Dialogs;
using Labb_3.Model;
using Labb_3.Services;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;

namespace Labb_3.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
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
                }
            }
        }

        public MenuViewModel MenuViewModel { get; }

        public ConfigurationViewModel ConfigurationViewModel { get; set; }

        public PlayerViewModel PlayerViewModel { get; }

        private bool _isConfigurationViewVisible;
        public bool IsConfigurationViewVisible
        {
            get => _isConfigurationViewVisible;
            set
            {
                if (_isConfigurationViewVisible != value)
                {
                    _isConfigurationViewVisible = value;
                    OnPropertyChanged(nameof(IsConfigurationViewVisible));
                }
            }
        }

        private bool _isPlayerViewVisible;
        public bool IsPlayerViewVisible
        {
            get => _isPlayerViewVisible;
            set
            {
                if (_isPlayerViewVisible != value)
                {
                    _isPlayerViewVisible = value;
                    OnPropertyChanged(nameof(IsPlayerViewVisible));
                }
            }
        }

        public ICommand ToggleConfigViewCommand => new DelegateCommand(_ =>
        {
            IsConfigurationViewVisible = true;
            IsPlayerViewVisible = false;
        });

        public ICommand TogglePlayerViewCommand => new DelegateCommand(_ =>
        {
            IsPlayerViewVisible = true;
            IsConfigurationViewVisible = false;
        });

        //public ICommand SetActivePackCommand { get; }

        public async Task OnApplicationExitAsync()
        {
            await _questionPackService.SaveQuestionPacksAsync(Packs);
        }

        //private void SetActivePack(QuestionPackViewModel pack)
        //{
        //    ActivePack = pack;
        //}
        public MainWindowViewModel()
        {
            _questionPackService = new QuestionPackService();

            MenuViewModel = new MenuViewModel(this);

            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayerViewModel = new PlayerViewModel(this);

            ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));

            IsConfigurationViewVisible = true;

            IsPlayerViewVisible = false;

            Packs = new ObservableCollection<QuestionPackViewModel>();

            //SetActivePackCommand = new RelayCommand<QuestionPackViewModel>(SetActivePack);
        }
    }
}