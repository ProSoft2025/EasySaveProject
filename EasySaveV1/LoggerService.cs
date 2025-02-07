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
        public void LogBackupCreation(BackupJob backupJob)
        {
            if (!Directory.Exists(backupJob.SourceDirectory))
            {
                Console.WriteLine($"Erreur : Le dossier source '{backupJob.SourceDirectory}' n'existe pas.");
                return; // Sortie pour éviter l'erreur
            }
            string[] files = Directory.GetFiles(backupJob.SourceDirectory);
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int totalFiles = files.Length;

            var logEntry = new LogEntry(
                backupJob.Name,
                backupJob.SourceDirectory,
                backupJob.TargetDirectory,
                (int)totalSize,
                0 // Temps fictif
            );

            _backupLogger.LogAction(logEntry);
        }
    }
}

