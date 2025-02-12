/*"The Logger class records the save actions of the LogEntry class
 * in a JSON file with line breaks for easy reading */

namespace BackupLogger
{
    public class LoggerService
    {
        private readonly ILogger _backupLogger;

        public LoggerService(string logFilePath, ILogger logFormat)
        {
            _backupLogger = new ILogger(logFilePath, logFormat);
        }
        public ILogger GetBackupLogger()
        {
            return _backupLogger;
        }
        public void LogBackupCreation(string filelog)
        {
            if (!File.Exists(filelog))
            {
                // Créer un fichier vide
                File.WriteAllText(filelog, "[]");
            }
        }
    }
}

