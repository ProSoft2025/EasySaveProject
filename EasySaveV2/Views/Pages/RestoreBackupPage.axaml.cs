using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BackupLogger;
using EasySaveV1;
using EasySaveV2.Services;

namespace EasySaveV2;

public partial class RestoreBackupPage : UserControl
{
    private readonly BackupJobFactory backupJobFactory;
    private readonly StateManager stateManager;
    private readonly EasySaveApp manager;
    private readonly MessageService messageService;
    private readonly LanguageManager languageManager;


    public RestoreBackupPage()
    {
        this.manager = EasySaveApp.GetInstance(languageManager);
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

        EasySaveApp appInstance = EasySaveApp.GetInstance(languageManager);

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
            await messageService.ShowMessage((Window)this.VisualRoot, "Backup restored successfully");
        }
        else
        {
            await messageService.ShowMessage((Window)this.VisualRoot, "No backup found with the specified name");
        }
    }
}


 

       
