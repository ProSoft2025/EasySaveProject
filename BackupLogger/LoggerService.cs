using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System;
using System.IO;
using Newtonsoft.Json;


/*"The Logger class records the save actions of the LogEntry class
 * in a JSON file with line breaks for easy reading */

namespace BackupLogger
{
    public class LoggerService
    {
        public Logger _backupLogger;

        public LoggerService(string logFilePath)
        {
            _backupLogger = new Logger(logFilePath);
        }
        public Logger GetBackupLogger()
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

