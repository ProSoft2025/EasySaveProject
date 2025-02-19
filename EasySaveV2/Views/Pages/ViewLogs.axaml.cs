using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BackupLogger;
using System;
using System.IO;

namespace EasySaveV2
{
    public partial class ViewLogsPage : UserControl
    {
        public ViewLogsPage()
        {
            InitializeComponent();
            OpenFileAndDisplayContent();
        }

        private void OpenFileAndDisplayContent()
        {
            string LogsFilePath = Path.Combine("C:\\EasySave\\logs", $"{DateTime.Now:yyyy-MM-dd}.json");
            string StateFilePath = Path.Combine("C:\\EasySave\\state", $"{DateTime.Now:yyyy-MM-dd}.json");

            // Check if TextBox with name "...TextBox" exists
            var LogsTextBox = this.FindControl<TextBox>("LogsContentTextBox");
            var StateTextBox = this.FindControl<TextBox>("StateContentTextBox");

            if (LogsTextBox == null)
            {
                System.Console.WriteLine("TextBox 'LogsContentTextBox' not found.");
                return;
            }

            // Read file content
            if (File.Exists(LogsFilePath))
            {
                string fileContent = File.ReadAllText(LogsFilePath);
                LogsTextBox.Text = fileContent;
            }
            else
            {
                LogsTextBox.Text = "No logs found";
            }

            if (File.Exists(StateFilePath))
            {
                string fileContent = File.ReadAllText(StateFilePath);
                StateTextBox.Text = fileContent;
            }
            else
            {
                StateTextBox.Text = "No State found";
            }
        }
    }
}
