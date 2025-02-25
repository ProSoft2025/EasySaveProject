using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BackupLogger;
using EasySaveV1;
using EasySaveV2.Services;
using EasySaveV2.Localization;
using System.Threading.Tasks;

namespace EasySaveV2.Views
{
    public partial class ExecuteBackupPage : UserControl
    {
        public ExecuteBackupPage()
        {
            InitializeComponent();
            DataContext = TranslationManager.Instance;
        }

        private List<BackupJob> GetSelectedBackups(string input)
        {
            var jobs = EasySaveApp.GetInstance().BackupJobs;
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

        private async Task ExecuteBackups(List<BackupJob> backupJobs)
        {
            var configManager = new ConfigManager();
            var logger = new JSONLog(configManager);

            var tasks = new List<Task>();

            foreach (var job in backupJobs)
            {
                tasks.Add(Task.Run(async () => await job.Execute(logger)));
            }

            await Task.WhenAll(tasks);
        }

        private async void OnExecuteBackupClick(object? sender, RoutedEventArgs e)
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

            await ExecuteBackups(selectedJobs);
            ExecutionOutputTextBlock.Text = "Backup(s) executed successfully.";
        }
    }
}
