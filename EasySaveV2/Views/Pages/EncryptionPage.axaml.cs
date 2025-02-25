using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using System.Linq;
using EasySaveV2.Services;
using EasySaveV2.Localization;

namespace EasySaveV2.Views
{
    public partial class EncryptionPage : UserControl
    {
        private readonly EasySaveApp manager;
        private readonly MessageService messageService;

        public EncryptionPage(EasySaveApp manager, MessageService messageService)
        {
            this.manager = manager;
            this.messageService = messageService;
            InitializeComponent();
            DataContext = TranslationManager.Translation; // Ajout du DataContext pour la traduction
            LoadExtensions();
        }

        private void LoadExtensions()
        {
            var extensionsListBox = this.FindControl<ListBox>("ExtensionsListBox");
            extensionsListBox.ItemsSource = manager.ExtensionsToEncrypt.ToList();
        }

        private async void OnAddExtensionClick(object sender, RoutedEventArgs e)
        {
            var extensionTextBox = this.FindControl<TextBox>("ExtensionTextBox");
            var extension = extensionTextBox.Text;
            if (string.IsNullOrEmpty(extension))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.ExtensionErrorEmpty);
                return;
            }
            else if (extension[0] != '.')
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.ExtensionErrorDot);
                return;
            }
            else if (manager.ExtensionsToEncrypt.Contains(extension))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.ExtensionErrorExists);
                return;
            }

            manager.AddExtension(extension);
            foreach (BackupJob job in manager.BackupJobs)
            {
                job.updateExtensionsToEncrypt(manager.ExtensionsToEncrypt);
            }
            LoadExtensions();
            extensionTextBox.Clear();
        }

        private async void OnRemoveExtensionClick(object sender, RoutedEventArgs e)
        {
            var extensionTextBox = this.FindControl<TextBox>("ExtensionTextBox");
            var extension = extensionTextBox.Text;
            if (string.IsNullOrEmpty(extension) || !manager.ExtensionsToEncrypt.Contains(extension))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, TranslationManager.Instance.ExtensionErrorNotFound);
                return;
            }

            manager.RemoveExtension(extension);
            foreach (BackupJob job in manager.BackupJobs)
            {
                job.updateExtensionsToEncrypt(manager.ExtensionsToEncrypt);
            }
            LoadExtensions();
            extensionTextBox.Clear();
        }

        private void ExtensionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var extensionsListBox = this.FindControl<ListBox>("ExtensionsListBox");
            var extensionTextBox = this.FindControl<TextBox>("ExtensionTextBox");

            if (extensionsListBox.SelectedItem != null)
            {
                extensionTextBox.Text = extensionsListBox.SelectedItem.ToString();
            }
        }
    }
}
