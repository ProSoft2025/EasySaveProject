using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using System.Linq;
using EasySaveV2.Services;

namespace EasySaveV2
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
            LoadExtensions();
        }

        private void LoadExtensions()
        {
            var extensionsListBox = this.FindControl<ListBox>("ExtensionsListBox");
            extensionsListBox.ItemsSource = manager.ExtensionToEncrypt.ToList();
        }

        private async void OnAddExtensionClick(object sender, RoutedEventArgs e)
        {
            var extensionTextBox = this.FindControl<TextBox>("ExtensionTextBox");
            var extension = extensionTextBox.Text;
            if (string.IsNullOrEmpty(extension))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Extension invalide");
                return;
            }
            else if (extension[0] != '.')
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "L'extension doit commencer par un point");
                return;
            }
            else if (manager.ExtensionToEncrypt.Contains(extension))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Extension déjà présente");
                return;
            }

            manager.AddExtension(extension);
            foreach (BackupJob job in manager.BackupJobs)
            {
                job.updateExtensionsToEncrypt(manager.ExtensionToEncrypt);
            }
            LoadExtensions();
            await messageService.ShowMessage((Window)this.VisualRoot, "Extension ajoutée avec succès");
        }

        private async void OnRemoveExtensionClick(object sender, RoutedEventArgs e)
        {
            var extensionTextBox = this.FindControl<TextBox>("ExtensionTextBox");
            var extension = extensionTextBox.Text;
            if (string.IsNullOrEmpty(extension) || !manager.ExtensionToEncrypt.Contains(extension))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Extension invalide");
                return;
            }

            manager.RemoveExtension(extension);
            foreach (BackupJob job in manager.BackupJobs)
            {
                job.updateExtensionsToEncrypt(manager.ExtensionToEncrypt);
            }
            LoadExtensions();
            await messageService.ShowMessage((Window)this.VisualRoot, "Extension supprimée avec succès");
        }
    }
}
