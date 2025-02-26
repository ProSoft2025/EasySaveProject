using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV1;
using System.Linq;
using EasySaveV2.Services;

namespace EasySaveV2.Views.Pages
{
    public partial class PriorityExtensionsPage : UserControl
    {
        private readonly EasySaveApp manager;
        private readonly MessageService messageService;

        public PriorityExtensionsPage(EasySaveApp manager, MessageService messageService)
        {
            this.manager = manager;
            this.messageService = messageService;
            InitializeComponent();
            LoadExtensions();
        }

        private void LoadExtensions()
        {
            var extensionsListBox = this.FindControl<ListBox>("ExtensionsListBox");
            extensionsListBox.ItemsSource = manager.PriorityExtensions.ToList();
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
            else if (manager.PriorityExtensions.Contains(extension))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Extension déjà présente");
                return;
            }

            manager.AddPriorityExtension(extension);
            foreach (BackupJob job in manager.BackupJobs)
            {
                job.UpdatePriorityExtensions(manager.PriorityExtensions);
            }
            LoadExtensions();
            await messageService.ShowMessage((Window)this.VisualRoot, "Extension ajoutée avec succès");
            extensionTextBox.Clear();
        }

        private async void OnRemoveExtensionClick(object sender, RoutedEventArgs e)
        {
            var extensionTextBox = this.FindControl<TextBox>("ExtensionTextBox");
            var extension = extensionTextBox.Text;
            if (string.IsNullOrEmpty(extension) || !manager.PriorityExtensions.Contains(extension))
            {
                await messageService.ShowMessage((Window)this.VisualRoot, "Extension invalide");
                return;
            }

            manager.RemovePriorityExtension(extension);
            foreach (BackupJob job in manager.BackupJobs)
            {
                job.UpdatePriorityExtensions(manager.PriorityExtensions);
            }
            LoadExtensions();
            await messageService.ShowMessage((Window)this.VisualRoot, "Extension supprimée avec succès");
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