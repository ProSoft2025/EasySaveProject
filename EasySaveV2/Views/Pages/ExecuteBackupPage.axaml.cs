using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BackupLogger;
using EasySaveV1;
using EasySaveV2.Services;
using EasySaveV2.Localization;

namespace EasySaveV2.Views
{
    public partial class ExecuteBackupPage : UserControl
    {
        private EasySaveApp easySaveApp;

        public ExecuteBackupPage()
        {
            InitializeComponent();
            DataContext = TranslationManager.Instance;
        }

        private List<BackupJob> GetSelectedBackups(string input)
        {
            var jobs = easySaveApp.BackupJobs;
            List<BackupJob> selectedJobs = new List<BackupJob>();

            if (!string.IsNullOrEmpty(input))
            {
                foreach (var part in input.Split(';'))
                {
                    if (int.TryParse(part, out int index) && index > 0 && index <= jobs.Count)
                        selectedJobs.Add(jobs[index - 1]);
                }
            }
            return selectedJobs;
        }

        private void ExecuteBackups(List<BackupJob> backupJobs)
        {
            var configManager = new ConfigManager();
            var logger = new JSONLog(configManager);

            foreach (var job in backupJobs)
                job.Execute(logger);
        }

        private void OnExecuteBackupClick(object? sender, RoutedEventArgs e)
        {
            if (BackupNumbersTextBox == null || ExecutionOutputTextBlock == null)
                return;

            string input = BackupNumbersTextBox.Text;
            var selectedJobs = GetSelectedBackups(input);

            if (selectedJobs.Count == 0)
            {
                ExecutionOutputTextBlock.Text = "No valid backups selected.";
                return;
            }

            ExecuteBackups(selectedJobs);
            ExecutionOutputTextBlock.Text = "Backup(s) executed successfully.";
        }
    }
}
