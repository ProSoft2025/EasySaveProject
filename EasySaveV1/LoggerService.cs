using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using EasySave;
using System;
using System.IO;
using Newtonsoft.Json;
using BackupLogger;

/*"The Logger class records the save actions of the LogEntry class
 * in a JSON file with line breaks for easy reading */

namespace EasySave
{
    public class LoggerService
    {
        private Logger _backupLogger;

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

