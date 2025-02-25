using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using EasySaveV2.Services;
using EasySaveV2.Localization;
using System.IO;

namespace EasySaveV2.Views
{
    public partial class AddBackupPage : UserControl
    {
        private readonly BackupJobFactory backupJobFactory;
        private readonly StateManager stateManager;
        private readonly MessageService messageService;
        private readonly LanguageManager languageManager;
        private readonly EasySaveApp manager;

        public AddBackupPage()
        {
            InitializeComponent();
            DataContext = TranslationManager.Instance;
            messageService = new MessageService();
        }

        public AddBackupPage(BackupJobFactory backupJobFactory, StateManager stateManager, EasySaveApp manager)
        {
            InitializeComponent();
            DataContext = TranslationManager.Instance;
            this.backupJobFactory = backupJobFactory;
            this.stateManager = stateManager;
            this.manager = EasySaveApp.GetInstance(languageManager);
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
            string strategyChoice = ((ComboBoxItem)BackupTypeComboBox.SelectedItem).Content.ToString();

            IBackupStrategy strategy;
            switch (strategyChoice)
            {
                case "Complete":
                    strategy = new CompleteBackup(languageManager, stateManager);
                    break;
                case "Differential":
                    strategy = new DifferentialBackup(languageManager, stateManager);
                    break;
                default:
                    await messageService.ShowMessage((Window)this.VisualRoot, "Incorrect choice, please try again");
                    return;
            }

            EasySaveApp.GetInstance(languageManager).AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
            JobNameTextBox.Text = "";
            SourceDirectoryTextBox.Text = "";
            TargetDirectoryTextBox.Text = "";
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
    }
}
