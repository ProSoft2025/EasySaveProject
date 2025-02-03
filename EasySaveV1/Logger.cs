using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using EasySave;

namespace EasySave
{
    public class Logger
    {
        // Déclaration des variables
        public string LogFilePath { get; set; }

        // Déclaration du Constructeur
        public Logger (string LogFilePath)
        {
            this.LogFilePath = LogFilePath;
        }

        public void LogAction(LogEntry entry)
        {

        }

        public void OpenLogFile()
        {

        }

        public void DisplayLogFileContent(string logFilePath="./Logs.json")
        {
            if (File.Exists(logFilePath))
            {
                var logContent = File.ReadAllText(logFilePath);
                Console.WriteLine(logContent);
            }
            else
            {
                Console.WriteLine("Log file not found.");
            }
        }
    }

}
