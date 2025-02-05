using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using EasySave;
using System;
using System.IO;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


/*"The Logger class records the save actions of the LogEntry class
 * in a JSON file with line breaks for easy reading */

namespace EasySave
{
    public class Logger
    {
        // Déclaration des variables
        public string LogFilePath { get; set; }
        public DateTime timestamp { get; set; } = DateTime.Now;
        // Déclaration du Constructeur
        public Logger (string LogFilePath)
        public Logger (string nom, DateTime timestamp)
            this.LogFilePath = LogFilePath;
            this.timestamp = timestamp;
        }
        public void LogAction(LogEntry entry)
        public void writeLog(string name, string file_src, string file_dst, double fileSize, double FileTransferTime, TimeSpan time)

        }

        public void OpenLogFile()
        {
            this.LogFilePath = logFilePath;
        }      
        public void LogAction(LogEntry entry)
        { 
                var logEntries = new List<LogEntry>();
                if (File.Exists(LogFilePath))
                {
                    var json = File.ReadAllText(LogFilePath);
                    logEntries = JsonSerializer.Deserialize<List<LogEntry>>(json);
                }
                logEntries.Add(entry);
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(LogFilePath, JsonSerializer.Serialize(logEntries, options));
            
        }
        public void writeLog(string name, string file_src, string file_dst, double fileSize, double FileTransferTime, TimeSpan time) { }

        public void OpenLogFile() { }

        public void DisplayLogFileContent()
        public void DisplayLogFileContent(string logFilePath="C:\\Users\\Tom\\Documents\\CESI\\A3\\PROJET TEST\\Beta-Projet 3\\Logs.json")
        {
            if (File.Exists(LogFilePath))
                var logContent = File.ReadAllText(LogFilePath);
                Console.WriteLine(LogFilePath);
                Console.WriteLine(logContent);
                Console.WriteLine(logContent);
            }
            else
            {
                Console.WriteLine("Log file not found.");
            }
        
    }
}