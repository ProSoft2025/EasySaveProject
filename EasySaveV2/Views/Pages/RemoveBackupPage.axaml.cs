using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EasySaveV2
{
    public partial class RemoveBackupPage : UserControl
    {
        public RemoveBackupPage()
        {
            InitializeComponent();
        }

        private void OnRemoveBackupClick(object sender, RoutedEventArgs e)
        {
            string name = JobNameTextBox.Text;
            string sourceDirectory = SourceDirectoryTextBox.Text;
            string targetDirectory = TargetDirectoryTextBox.Text;
            string strategyChoice = ((ComboBoxItem)BackupTypeComboBox.SelectedItem).Content.ToString();

            //IBackupStrategy strategy;
            //switch (strategyChoice)
            //{
            //    case "Complete Backup":
            //        strategy = new CompleteBackup();
            //        break;
            //    case "Differential Backup":
            //        strategy = new DifferentialBackup();
            //        break;
            //    default:
            //        MessageBox.Show("Incorrect choice, please try again");
            //        return;
            //}

            //manager.AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
            //MessageBox.Show("Backup added successfully");
        }
    }
}
