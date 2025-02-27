using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleDistante
{
    public partial class RemoteBackups : Window
    {
        public ObservableCollection<Dictionary<string, string>> BackupJobs { get; } = new ObservableCollection<Dictionary<string, string>>();

        public RemoteBackups()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void UpdateBackupJobList(List<Dictionary<string, string>> backupJobs)
        {
            BackupJobs.Clear();
            foreach (var job in backupJobs)
            {
                BackupJobs.Add(job);
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
