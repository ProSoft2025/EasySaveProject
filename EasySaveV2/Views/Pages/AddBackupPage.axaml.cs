using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using System;
using System.Threading.Tasks;


namespace EasySaveV2
{
    public partial class AddBackupPage : UserControl
    {
        //private readonly IBackupJobFactory backupJobFactory;
        //private readonly IStateManager stateManager;
        //private readonly IBackupManager manager;IBackupJobFactory backupJobFactory, IStateManager stateManager, IBackupManager manager

        public AddBackupPage()
        {
            //this.backupJobFactory = backupJobFactory;
            //this.stateManager = stateManager;
            //this.manager = manager;
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
                    ShowMessage("Incorrect choice, please try again");
                    return;
            }

            AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
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
