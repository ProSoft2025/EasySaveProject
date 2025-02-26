using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using EasySaveV2.Services;
using EasySaveV2.Localization;
using BackupLogger;

namespace EasySaveV2.Views
{
    public partial class RestoreBackupPage : UserControl
    {
        private readonly EasySaveApp manager;
        private readonly MessageService messageService;

        public RestoreBackupPage()
        {
            this.manager = EasySaveApp.GetInstance();
            this.messageService = new MessageService();
            InitializeComponent();
        }

        private async void OnRestoreBackupClick(object sender, RoutedEventArgs e)
        {
            string name = JobNameTextBox.Text;

            if (string.IsNullOrEmpty(name))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.BackupNameInvalid);
                return;
            }

            EasySaveApp appInstance = EasySaveApp.GetInstance();
            var configManager = new ConfigManager();
            var logger = new JSONLog(configManager);

            bool isRestore = false;

            foreach (var job in appInstance.BackupJobs)
            {
                if (job.Name == name)
                {
                    job.Restore(logger);
                    isRestore = true;
                }
            }

            if (isRestore)
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.BackupRestored);
            }
            else
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.BackupNotFound);
            }
        }
    }
}
