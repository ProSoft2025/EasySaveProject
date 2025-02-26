using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using EasySaveV2.Services;
using EasySaveV2.Localization;

namespace EasySaveV2.Views
{
    public partial class RemoveBackupPage : UserControl
    {
        private readonly EasySaveApp manager;
        private readonly MessageService messageService;

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
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.BackupNameInvalid);
                return;
            }

            bool isRemoved = manager.BackupJobs.RemoveAll(job => job.Name == name) > 0;
            if (isRemoved)
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.BackupDeleted);
            }
            else
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.BackupNotFound);
            }
        }
    }
}
