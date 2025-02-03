using System.Text.Json;
using EasySave;

namespace EasySave
{
    public class Logger
    {
        // Déclaration des variables
        public string nom;
        public DateTime timestamp { get; set; } = DateTime.Now;


        // Déclaration du Constructeur
        public Logger (string nom, DateTime timestamp)
        {
            this.nom = nom;
            this.timestamp = timestamp;
        }

        public void writeLog(string name, string file_src, string file_dst, double fileSize, double FileTransferTime, TimeSpan time)
        {

            string json = "{" + "name" + "" + "" + "" + "}";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // string jsonString = JsonSerializer.Serialize(backupInfo, options);
            // Console.WriteLine(jsonString);
        }
        public void DisplayLogFileContent(string logFilePath="C:\\Users\\Tom\\Documents\\CESI\\A3\\PROJET TEST\\Beta-Projet 3\\Logs.json")
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
