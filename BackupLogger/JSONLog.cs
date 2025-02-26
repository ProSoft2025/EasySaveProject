/// <summary>
/// Represent a log strategy that writes logs in JSON format
/// </summary>
namespace BackupLogger
{
    public class JSONLog : ILoggerStrategy
    {
        private IConfigManager configManager;

        public JSONLog(IConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public override void Update(string taskName, string sourcePath, string targetPath, long fileSize, long transferTime, int EncryptionTime)
        {
            string logFileName = Path.Combine(configManager.LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.json");

            string logEntry = "{\n" +
                              $"  \"Timestamp\": \"{DateTime.Now:HH:mm:ss}\",\n" +
                              $"  \"TaskName\": \"{taskName}\",\n" +
                              $"  \"SourcePath\": \"{sourcePath}\",\n" +
                              $"  \"TargetPath\": \"{targetPath}\",\n" +
                              $"  \"FileSize\": {fileSize},\n" +
                              $"  \"TransferTime\": {transferTime},\n" +
                              $"  \"EncryptionTime\": {EncryptionTime}\n" +
                              "}\n";

            using (StreamWriter writer = new StreamWriter(logFileName, append: true))
            {
                writer.WriteLine(logEntry);
            }

            Console.WriteLine($"[LOG UPDATED] {logEntry}");
        }

        public override void DisplayLogFileContent()
        {
            string logFileName = Path.Combine(configManager.LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.json");
            if (File.Exists(logFileName))
            {
                Console.WriteLine($"Contenu du log ({logFileName}):");
                Console.WriteLine(File.ReadAllText(logFileName));
            }
            else
            {
                Console.WriteLine("Log file not found.");
            }
        }
    }
}