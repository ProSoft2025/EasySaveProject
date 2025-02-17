using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BackupLogger;
using System;
using System.IO;

namespace EasySaveV2
{
    public partial class DailyLogsPage : UserControl
    {
        public DailyLogsPage()
        {
            InitializeComponent();
            OpenFileAndDisplayContent();
        }

        private void OpenFileAndDisplayContent()
        {
            string filePath = Path.Combine("C:\\EasySave\\logs", $"{DateTime.Now:yyyy-MM-dd}.json");

            // Check if TextBox with name "FileContentTextBox" exists
            var textBox = this.FindControl<TextBox>("FileContentTextBox");
            if (textBox == null)
            {
                System.Console.WriteLine("TextBox 'FileContentTextBox' not found.");
                return;
            }

            // Read file content
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                textBox.Text = fileContent;
            }
            else
            {
                textBox.Text = "No logs found";
            }
        }
    }
}
