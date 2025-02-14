using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using System;
using System.Threading.Tasks;




namespace EasySaveV2
{
    public partial class AddBackupPage : UserControl
    {    

        private readonly BackupJobFactory backupJobFactory;
        private readonly StateManager stateManager;
        private readonly EasySaveApp manager;
        public AddBackupPage(BackupJobFactory backupJobFactory, StateManager stateManager, EasySaveApp manager)
        {
            this.backupJobFactory = backupJobFactory;
            this.stateManager = stateManager;
            this.manager = EasySaveApp.GetInstance();
            InitializeComponent();
        }

        private void OnAddBackupClick(object sender, RoutedEventArgs e)
        {
            string name = JobNameTextBox.Text;
            string sourceDirectory = SourceDirectoryTextBox.Text;
            string targetDirectory = TargetDirectoryTextBox.Text;
            string strategyChoice = ((ComboBoxItem)BackupTypeComboBox.SelectedItem).Content.ToString();

            IBackupStrategy strategy;
            switch (strategyChoice)
            {
                case "Complete Backup":
                    strategy = new CompleteBackup();
                    break;
                case "Differential Backup":
                    strategy = new DifferentialBackup();
                    break;
                default:
                    ShowMessage("Incorrect choice, please try again").Wait();
                    return;
            }

            EasySaveApp.GetInstance().AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
            ShowMessage("Backup added successfully");
        }

        private async Task ShowMessage(string message)
        {
            var dialog = new Window
            {
                Content = new TextBlock { Text = message },
                Width = 300,
                Height = 100,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            await dialog.ShowDialog((Window)this.VisualRoot);
        }
    }
}
