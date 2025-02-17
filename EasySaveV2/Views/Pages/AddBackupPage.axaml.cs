using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using System;
using System.Threading.Tasks;
using EasySaveV2.Services;



namespace EasySaveV2
{
    public partial class AddBackupPage : UserControl
    {    

        private readonly BackupJobFactory backupJobFactory;
        private readonly StateManager stateManager;
        private readonly EasySaveApp manager;
        private readonly MessageService messageService;
        private readonly LanguageManager languageManager;
        public AddBackupPage(BackupJobFactory backupJobFactory, StateManager stateManager, EasySaveApp manager)
        {
            this.backupJobFactory = backupJobFactory;
            this.stateManager = stateManager;
            this.manager = EasySaveApp.GetInstance(languageManager);
            this.messageService = new MessageService();
            InitializeComponent();
        }

        private async void OnAddBackupClick(object sender, RoutedEventArgs e)
        {
            string name = JobNameTextBox.Text;
            string sourceDirectory = SourceDirectoryTextBox.Text;
            string targetDirectory = TargetDirectoryTextBox.Text;
            string strategyChoice = ((ComboBoxItem)BackupTypeComboBox.SelectedItem).Content.ToString();

            IBackupStrategy strategy;
            switch (strategyChoice)
            {
                case "Complete Backup":
                    strategy = new CompleteBackup(languageManager);
                    break;
                case "Differential Backup":
                    strategy = new DifferentialBackup(languageManager);
                    break;
                default:
                    await messageService.ShowMessage((Window)this.VisualRoot, "Incorrect choice, please try again");
                    return;
            }

            EasySaveApp.GetInstance(languageManager).AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
            await messageService.ShowMessage((Window)this.VisualRoot, "Backup added successfully");
        }
    }
}
