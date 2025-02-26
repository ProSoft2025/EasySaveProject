using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BackupLogger;
using EasySaveV1;
using EasySaveV2.Services;
using EasySaveV2.Localization;
using System.Threading.Tasks;
using EasySaveV2.ViewModels;

namespace EasySaveV2.Views
{
    public partial class ExecuteBackupPage : UserControl
    {
        public ExecuteBackupPage()
        {
            InitializeComponent();
            DataContext = new ExecuteBackupPageViewModel();
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
            if (BackupNumbersTextBox == null)
                return;

            var viewModel = DataContext as ExecuteBackupPageViewModel; // Récupère le ViewModel
            if (viewModel == null) return;

            // Vérification : aucun backup existant
            if (EasySaveApp.GetInstance().BackupJobs.Count == 0)
            {
                viewModel.Backup.Status = TranslationManager.Instance.BackupError; // Utilisation de la traduction
                return;
            }

            string input = BackupNumbersTextBox.Text;
            var selectedJobs = GetSelectedBackups(input);

            // Vérification : entrée utilisateur invalide
            if (selectedJobs.Count == 0 || input.Contains("0"))
            {
                viewModel.Backup.Status = TranslationManager.Instance.BackupError; // Traduction de l'erreur
                return;
            }

            // Lancement des backups
            await ExecuteBackups(selectedJobs);
            viewModel.Backup.Status = TranslationManager.Instance.BackupCompleted; // Traduction du message de succès
        }
    }
}
