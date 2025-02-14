using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EasySaveV1;
using EasySaveV2.Services;

namespace EasySaveV2;

public partial class RestoreBackupPage : UserControl
{
    private readonly BackupJobFactory backupJobFactory;
    private readonly StateManager stateManager;
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
            await messageService.ShowMessage((Window)this.VisualRoot, "Please enter a valid backup name");
            return;
        }

        EasySaveApp appInstance = EasySaveApp.GetInstance();
        bool isRemoved = appInstance.BackupJobs.RemoveAll(job => job.Name == name) > 0;
        if (isRemoved)
        {
            await messageService.ShowMessage((Window)this.VisualRoot, "Backup restored successfully");
        }
        else
        {
            await messageService.ShowMessage((Window)this.VisualRoot, "No backup found with the specified name");
        }
    }
}


 

       
