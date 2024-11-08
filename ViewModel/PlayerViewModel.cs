using Labb_3.Commands;
using System.Windows.Threading;

namespace Labb_3.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        
        public DelegateCommand UpdateButtonCommand { get; }
        
        private DispatcherTimer timer;
        
        private int testData = 60;

        public int TestData 
        { 
            get => testData;
            private set
            {
                testData = value;
                OnPropertyChanged();
            }
        }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateButtonCommand = new DelegateCommand(UpdateButton, CanUpdateButton);
        }

        private bool CanUpdateButton(object? arg) // Namnet är deceiving, ett bättre namn / exempel hade varit AddQuestion, Remove Question osv. It just so happened to be a button example.
        {
            return TestData !> 0;
        }

        private void UpdateButton(object obj)
        {
            TestData -= 10;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TestData -= 1;
        }
    }
}
