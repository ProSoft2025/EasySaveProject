using System.Text.Json;

namespace BackupLogger
{
    public class JSONLogger : ILogger
    {
        public string LogFilePath { get; set; }

        public JSONLogger(string LogFilePath)
        {
            this.LogFilePath = LogFilePath;
        }

        public void LogAction(LogEntry entry)
        {
            var logEntries = new List<LogEntry>();
            if (File.Exists(LogFilePath))
            {
                var existingLogs = File.ReadAllText(LogFilePath);
                logEntries = JsonSerializer.Deserialize<List<LogEntry>>(existingLogs) ?? new List<LogEntry>();
            }
            logEntries.Add(entry);

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(LogFilePath, JsonSerializer.Serialize(logEntries, options));
        }

        public void DisplayLogFileContent()
        {
            if (File.Exists(LogFilePath))
            {
                Console.WriteLine($"Contenu du log ({LogFilePath}):");
                Console.WriteLine(File.ReadAllText(LogFilePath));
            }
            else
            {
                // Changer par exception
                Console.WriteLine("Log file not found.");
            }
        }
    }
}
