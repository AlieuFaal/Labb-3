using Labb_3.Commands;
using Labb_3.Dialogs;
using Labb_3.Model;
using Labb_3.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Labb_3.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

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

        private ObservableCollection<CategoryGroup> _categorizedPacks;
        public ObservableCollection<CategoryGroup> CategorizedPacks
        {
            get => _categorizedPacks;
            set
            {
                _categorizedPacks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
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
                    OnPropertyChanged(nameof(ActivePack));
                    OnActivePackChanged();
                }
            }
        }



        public ICommand OpenCreatePackDialogCommand => new DelegateCommand(_ => OpenCreatePackDialog());
        public ICommand OpenAddCategoryDialogCommand => new DelegateCommand(_ => OpenAddCategoryDialog());

        public ICommand OpenDeleteCategoryDialogCommand => new DelegateCommand(_ => OpenDeleteCategoryDialog());

        public ICommand OpenPackOptionsDialogCommand => new DelegateCommand(_ => OpenPackOptionsDialog());

        public ICommand SetActivePackCommand { get; }

        public ICommand DeleteActivePackCommand { get; }

        public ICommand ExitAppCommand => new DelegateCommand(_ => Application.Current.Shutdown());



        public void CategorizePacks()
        {
            var categorizedPacks = Packs
                .GroupBy(p => p.Model.Category)
                .Select(g => new CategoryGroup
                {
                    Category = g.Key,
                    QuestionPacks = new ObservableCollection<QuestionPackViewModel>(g)
                })
                .ToList();

            CategorizedPacks = new ObservableCollection<CategoryGroup>(categorizedPacks);
            OnPropertyChanged(nameof(CategorizedPacks));
        }

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
            var viewModel = new QuestionPackViewModel(new QuestionPack(), Packs, mainWindowViewModel, this);
            var dialog = new CreateNewPackDialog(viewModel);
            dialog.ShowDialog();
        }

        private void OpenAddCategoryDialog()
        {
            var dialog = new CreateCategoryDialog();
            if (dialog.ShowDialog() == true)
            {
                if (!Categories.Any(c => c.Name == dialog.CategoryName))
                {
                    Categories.Add(new Category(dialog.CategoryName));
                    SaveCategories();
                }
                else if (string.IsNullOrWhiteSpace(dialog.CategoryName))
                {
                    MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Category already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void OpenDeleteCategoryDialog()
        {
            var dialog = new RemoveCategoryDialog();
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                var categoryToRemove = Categories.FirstOrDefault(c => c.Name == dialog.CategoryName);
                if (categoryToRemove != null)
                {
                    Categories.Remove(categoryToRemove);
                    
                    var packsToRemove = Packs.Where(p => p.Model.Category == dialog.CategoryName).ToList();
                    foreach (var pack in packsToRemove)
                    {
                        await _questionPackService.RemoveQuestionPackAsync(pack);
                        Packs.Remove(pack);

                        if (ActivePack == pack)
                        {
                            ActivePack = null;
                        }

                        MessageBox.Show($"Category '{categoryToRemove.Name}' and associated question packs have been removed.", "Category Removed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        
                        SaveCategories();
                        CategorizePacks();
                    }
                }
            }
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
                    },
                Category = "General"
            }, Packs, mainWindowViewModel, this);

            Packs.Add(defaultPack);
            SetActivePack(defaultPack);
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
                    ActivePack = null;
                    OnPropertyChanged(nameof(ActivePack));
                }

                CommandManager.InvalidateRequerySuggested();
            }
            CategorizePacks();
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
            CategorizePacks();

            if (Packs.Count == 0)
            {
                InsertDefaultPack();
                await _questionPackService.SaveQuestionPacksAsync(Packs);
            }
            else
            {
                SetActivePack(Packs.First());
            }
            CategorizePacks();
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _questionPackService.LoadCategoriesAsync();
            Categories = new ObservableCollection<Category>(categories);

            QuestionPack.Categories.Clear();
            foreach (var category in categories)
            {
                QuestionPack.Categories.Add(category.Name);
            }
        }

        private async void SaveCategories()
        {
            await _questionPackService.SaveCategoriesAsync(new ObservableCollection<string>(Categories.Select(c => c.Name)));
        }

        private void OpenPackOptionsDialog()
        {
            if (ActivePack != null)
            {
                var originalName = ActivePack.Model.Name;
                var dialog = new PackOptionsDialog(ActivePack);
                dialog.Closed += async (s, e) =>
                {
                    await _questionPackService.UpdateQuestionPackAsync(ActivePack, originalName);
                };
                dialog.ShowDialog();
            }
            CategorizePacks();
        }



        public MenuViewModel(MainWindowViewModel? mainWindowViewModel, ObservableCollection<QuestionPackViewModel> packs)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            _questionPackService = new QuestionPackService();

            Packs = packs;
            Packs.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Packs));

            ActivePack = mainWindowViewModel?.ActivePack;

            Task.Run(async () => await LoadQuestionPacksAsync());
            Task.Run(async () => await LoadCategoriesAsync());

            SetActivePackCommand = new DelegateCommand(param => SetActivePack((QuestionPackViewModel)param));
            DeleteActivePackCommand = new DelegateCommand(_ => DeleteQuestionPack(_), _ => CanDeletePack());
        }
    }
}