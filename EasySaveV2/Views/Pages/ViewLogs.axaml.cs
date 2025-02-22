using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
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
            LoadTodayLog();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OpenFileManager_Click(object sender, RoutedEventArgs e)
        {
            string logDirectoryPath = @"C:\EasySave\logs"; 
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

        /// <summary>
        /// Add daily log when enter the page so textbox is already loaded with the today log (priority json)
        /// </summary>
        private async void LoadTodayLog()
        {
            string todayLogFileName = $"{DateTime.Now:yyyy-MM-dd}.json";
            string todayLogFilePath = Path.Combine(@"C:\EasySave\logs", todayLogFileName);

            if (!File.Exists(todayLogFilePath))
            {
                todayLogFileName = $"{DateTime.Now:yyyy-MM-dd}.xml";
                todayLogFilePath = Path.Combine(@"C:\EasySave\logs", todayLogFileName);
            }

            if (File.Exists(todayLogFilePath))
            {
                string fileContent = await ReadFileContentAsync(todayLogFilePath);
                var logsTextBox = this.FindControl<TextBox>("LogsContentTextBox");
                logsTextBox.Text = fileContent;
            }

            string stateFilePath = @"C:\EasySave\state.json";
            if (File.Exists(stateFilePath))
            {
                string content = await ReadFileContentAsync(stateFilePath);
                var stateTextBox = this.FindControl<TextBox>("StateContentTextBox");
                stateTextBox.Text = content;
            }
        }
    }
}