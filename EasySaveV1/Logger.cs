using System;
using System.IO;
using Newtonsoft.Json;

namespace EasySave
{
    public class Logger
    {
        public string LogFilePath { get; set; }
        public DateTime timestamp { get; set; } = DateTime.Now;

        public Logger(string LogFilePath)
        {
            this.LogFilePath = LogFilePath;
        }

        public void LogAction(LogEntry entry)
        {
            try
            {
                string json = JsonConvert.SerializeObject(entry, Formatting.Indented);
                File.AppendAllText(LogFilePath, json + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'\u00e9criture du log : {ex.Message}");
            }
        }

        public void OpenLogFile()
        {
            // Impl\u00e9mentation pour ouvrir le fichier log si besoin
        }

        public void DisplayLogFileContent()
        {
            if (File.Exists(LogFilePath))
            {
                string logContent = File.ReadAllText(LogFilePath);
                Console.WriteLine(LogFilePath);
                Console.WriteLine(logContent);
            }
            else
            {
                Console.WriteLine("Log file not found.");
            }
        }
    }
}
