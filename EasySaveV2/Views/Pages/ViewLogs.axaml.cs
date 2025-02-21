using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace EasySaveV2
{
    public partial class ViewLogsPage : UserControl
    {
        public ViewLogsPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OpenFileManager_Click(object sender, RoutedEventArgs e)
        {
            string logDirectoryPath = @"C:\EasySave\logs"; // Remplacez par le chemin de votre répertoire de logs
            Process.Start("explorer.exe", logDirectoryPath);
        }

        private async void SelectLogFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "JSON and XML files", Extensions = new List<string> { "json", "xml" } }
                }
            };

            var result = await openFileDialog.ShowAsync((Window)this.VisualRoot);
            if (result != null && result.Length > 0)
            {
                string selectedFilePath = result[0];
                string fileContent = await ReadFileContentAsync(selectedFilePath);
                var logsTextBox = this.FindControl<TextBox>("LogsContentTextBox");
                logsTextBox.Text = fileContent;
            }
        }

        private async void SelectStateFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "JSON and XML files", Extensions = new List<string> { "json", "xml" } }
                }
            };

            var result = await openFileDialog.ShowAsync((Window)this.VisualRoot);
            if (result != null && result.Length > 0)
            {
                string selectedFilePath = result[0];
                string fileContent = await ReadFileContentAsync(selectedFilePath);
                var stateTextBox = this.FindControl<TextBox>("StateContentTextBox");
                stateTextBox.Text = fileContent;
            }
        }

        private async Task<string> ReadFileContentAsync(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}