using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EasySaveV1;

namespace EasySaveV2
{

    public partial class ViewBackupPage : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<BackupJob> _backupJobs;
        private readonly LanguageManager languageManager;

        public ObservableCollection<BackupJob> BackupJobs
        {
            get => _backupJobs;
            set
            {
                if (_backupJobs != value)
                {
                    _backupJobs = value;
                    OnPropertyChanged(nameof(BackupJobs));
                }
            }
        }

        public ViewBackupPage()
        {
            InitializeComponent();
            DataContext = this;
            BackupJobs = new ObservableCollection<BackupJob>(EasySaveApp.GetInstance(languageManager).BackupJobs);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}