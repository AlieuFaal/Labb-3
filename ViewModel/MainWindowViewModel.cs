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

        public MenuViewModel MenuViewModel { get; set; }

        public ConfigurationViewModel ConfigurationViewModel { get; set; }

        public PlayerViewModel PlayerViewModel { get; set; }



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


        public ICommand ToggleFullScreenCommand { get; }

        public ICommand ToggleConfigViewCommand => new DelegateCommand(_ =>
        {
            StopQuiz();
        });

        public ICommand TogglePlayerViewCommand => new DelegateCommand(_ =>
        {
            StartQuiz();
        });

        public ICommand OpenPackOptionsDialogCommand => new DelegateCommand(_ => OpenPackOptionsDialog());



        public async Task OnApplicationExitAsync()
        {
            await _questionPackService.SaveQuestionPacksAsync(Packs);
        }

        private void OpenPackOptionsDialog()
        {
            if (ActivePack != null)
            {
                var dialog = new PackOptionsDialog(ActivePack);
                dialog.ShowDialog();
            }
        }

        public void StartQuiz()
        {
            IsConfigurationViewVisible = false;
            IsPlayerViewVisible = true;
            PlayerViewModel.StartQuiz();
        }

        public void StopQuiz()
        {
            IsConfigurationViewVisible = true;
            IsPlayerViewVisible = false;
            PlayerViewModel.StopQuiz2();
        }

        private async void LoadPacks()
        {
           Packs = await _questionPackService.LoadQuestionPacksAsync();
        }

        private void ToggleFullScreen()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                if(mainWindow.WindowState == WindowState.Maximized && mainWindow.WindowStyle == WindowStyle.None)
                {
                    mainWindow.WindowState = WindowState.Normal;
                    mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                }
                else
                {
                    mainWindow.WindowState = WindowState.Maximized;
                    mainWindow.WindowStyle = WindowStyle.None;
                }
            }
        }



        public MainWindowViewModel()
        {
            _questionPackService = new QuestionPackService();

            MenuViewModel = new MenuViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);

            Packs = new ObservableCollection<QuestionPackViewModel>();

            IsConfigurationViewVisible = true;
            IsPlayerViewVisible = false;

            LoadPacks();

            MenuViewModel.ActivePack = Packs.FirstOrDefault();
            ConfigurationViewModel.ActivePack = MenuViewModel.ActivePack;

            ToggleFullScreenCommand = new DelegateCommand(_ => ToggleFullScreen());
        }
    }
}