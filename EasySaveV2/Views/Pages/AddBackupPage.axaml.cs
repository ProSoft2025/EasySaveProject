using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using EasySaveV2.Services;
using EasySaveV2.Localization;
using System.IO;
using System.Linq;

namespace EasySaveV2.Views
{
    public partial class AddBackupPage : UserControl
    {
        private readonly BackupJobFactory backupJobFactory;
        private readonly StateManager stateManager;
        private readonly MessageService messageService;
        private readonly LanguageManager languageManager;
        private readonly EasySaveApp manager;

        public AddBackupPage() { }

        public AddBackupPage(BackupJobFactory backupJobFactory, StateManager stateManager, EasySaveApp manager)
        {
            InitializeComponent();
            DataContext = TranslationManager.Instance;
            this.backupJobFactory = backupJobFactory;
            this.stateManager = stateManager;
            this.manager = EasySaveApp.GetInstance();
            messageService = new MessageService();
        }

        private async void OnAddBackupClick(object sender, RoutedEventArgs e)
        {

            string name = JobNameTextBox.Text;

            string sourceDirectory = SourceDirectoryTextBox.Text;


            if (string.IsNullOrWhiteSpace(name) || BackupTypeComboBox.SelectedItem == null || TargetDirectoryTextBox == null)
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Incorrect choice, please try again");
                return;
            }
            
            string targetDirectory = Path.Combine(TargetDirectoryTextBox.Text, name);
            string lastFullBackupDir = LastFullBackupPathTextBox.Text;
            string strategyChoice = ((ComboBoxItem)BackupTypeComboBox.SelectedItem).Content.ToString();

            
            IBackupStrategy strategy;
            switch (strategyChoice)
            {
                case "Complete":
                    strategy = new CompleteBackup(languageManager, stateManager);
                    break;
                case "Differential":
                    strategy = new DifferentialBackup(languageManager, stateManager, lastFullBackupDir);
                    break;
                default:
                    await messageService.ShowMessage((Window)this.VisualRoot, "Incorrect choice, please try again");
                    return;
            }

            if (strategyChoice == "Differential")
            {
                if (string.IsNullOrWhiteSpace(lastFullBackupDir))
                {
                    await messageService.ShowMessage((Window)this.VisualRoot, "Please select a last full backup directory");
                    return;
                }

                if (!Directory.Exists(lastFullBackupDir))
                {
                    await messageService.ShowMessage((Window)this.VisualRoot, "The last full backup directory does not exist");
                    return;
                }

                if (!Directory.EnumerateFileSystemEntries(lastFullBackupDir).Any())
                {
                    await messageService.ShowMessage((Window)this.VisualRoot, "The last full backup directory is empty");
                    return;
                }
            }

            EasySaveApp.GetInstance().AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
            JobNameTextBox.Text = "";
            SourceDirectoryTextBox.Text = "";
            TargetDirectoryTextBox.Text = "";
            LastFullBackupPathTextBox.Text = "";
            BackupTypeComboBox.SelectedItem = null;
        }

        private async void OnBrowseSourceClick(object sender, RoutedEventArgs e)
        {
            if (SourceDirectoryTextBox == null) return;
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync((Window)this.VisualRoot);
            if (!string.IsNullOrEmpty(result))
                SourceDirectoryTextBox.Text = result;
        }

        private async void OnBrowseTargetClick(object sender, RoutedEventArgs e)
        {
            if (TargetDirectoryTextBox == null) return;
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync((Window)this.VisualRoot);
            if (!string.IsNullOrEmpty(result))
                TargetDirectoryTextBox.Text = result;
        }
        private async void OnBrowseLastFullBackupClick(object sender, RoutedEventArgs e)
        {
            if (LastFullBackupPathTextBox == null) return;
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync((Window)this.VisualRoot);
            if (!string.IsNullOrEmpty(result))
                LastFullBackupPathTextBox.Text = result;
        }

        private void OnBackupTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BackupTypeComboBox.SelectedItem != null)
            {
                string strategyChoice = ((ComboBoxItem)BackupTypeComboBox.SelectedItem).Content.ToString();
                var BrowseDiff = this.FindControl<Button>("BrowseDiff");
                var LastText = this.FindControl<TextBlock>("LastText");
                var LastFullBackupPathTextBox = this.FindControl<TextBox>("LastFullBackupPathTextBox");

                if (strategyChoice == "Differential")
                {
                    LastText.IsVisible = true;
                    LastFullBackupPathTextBox.IsVisible = true;
                    BrowseDiff.IsVisible = true;
                }
                else
                {
                    LastText.IsVisible = false;
                    LastFullBackupPathTextBox.IsVisible = false;
                    BrowseDiff.IsVisible = false;
                }

            }
        }

    }
}
