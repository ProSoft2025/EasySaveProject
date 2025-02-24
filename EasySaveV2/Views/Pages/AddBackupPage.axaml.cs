using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using EasySaveV2.Services;
using EasySaveV2.Localization;

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
            if (JobNameTextBox == null || SourceDirectoryTextBox == null || TargetDirectoryTextBox == null || BackupTypeComboBox == null)
                return;

            string name = JobNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
                return;

            string sourceDirectory = SourceDirectoryTextBox.Text;
            string targetDirectory = TargetDirectoryTextBox.Text;
            string strategyChoice = ((ComboBoxItem)BackupTypeComboBox.SelectedItem)?.Content?.ToString();

            if (strategyChoice == null)
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Incorrect choice, please try again");
                return;
            }

            IBackupStrategy strategy = strategyChoice switch
            {
                "Complete Backup" => new CompleteBackup(languageManager, stateManager),
                "Differential Backup" => new DifferentialBackup(languageManager, stateManager),
                _ => null
            };

            if (strategy == null)
                return;

            EasySaveApp.GetInstance(languageManager).AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
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
