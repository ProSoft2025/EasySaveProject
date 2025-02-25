using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using EasySaveV2.Services;

namespace EasySaveV2
{
    public partial class RemoveBackupPage : UserControl
    {
        private readonly BackupJobFactory backupJobFactory;
        private readonly StateManager stateManager;
        private readonly EasySaveApp manager;
        private readonly MessageService messageService;
        private readonly LanguageManager languageManager;

        public RemoveBackupPage()
        {
            this.manager = EasySaveApp.GetInstance();
            this.messageService = new MessageService();
            InitializeComponent();
        }

        private async void OnRemoveBackupClick(object sender, RoutedEventArgs e)
        {
            string name = JobNameTextBox.Text;

            if (string.IsNullOrEmpty(name))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Please enter a valid backup name");
                return;
            }

            EasySaveApp appInstance = EasySaveApp.GetInstance();
            bool isRemoved = appInstance.BackupJobs.RemoveAll(job => job.Name == name) > 0;
            if (isRemoved)
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Backup deleted successfully");
            }
            else
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "No backup found with the specified name");
            }
        }
    }
}
