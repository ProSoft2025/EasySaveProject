using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BackupLogger;
using EasySaveV1;
using EasySaveV2.Services;

namespace EasySaveV2
{
    public partial class ExecuteBackupPage : UserControl
    {
        private EasySaveApp easySaveApp;

        public ExecuteBackupPage()
        {
            InitializeComponent();
            var languageManager = new LanguageManager(); // Assure que LanguageManager est bien instancié
            easySaveApp = EasySaveApp.GetInstance(languageManager);
        }

        private List<BackupJob> GetSelectedBackups(string input)
        {
            var jobs = easySaveApp.BackupJobs;
            List<BackupJob> selectedJobs = new List<BackupJob>();

            foreach (var part in input.Split(';'))
            {
                if (part.Contains("-"))
                {
                    var rangeParts = part.Split('-');
                    if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                    {
                        for (int i = start - 1; i < end; i++)
                        {
                            if (i >= 0 && i < jobs.Count)
                                selectedJobs.Add(jobs[i]);
                        }
                    }
                }
                else if (int.TryParse(part, out int index))
                {
                    if (index > 0 && index <= jobs.Count)
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
            {
                job.Execute(logger);
            }
        }

        private void OnExecuteBackupClick(object? sender, RoutedEventArgs e)
        {
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
