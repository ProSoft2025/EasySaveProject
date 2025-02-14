using BackupLogger;
using System.Diagnostics;

namespace EasySave
{
    public class DifferentialBackup : IBackupStrategy
    {
        private LanguageManager languageManager;

        public DifferentialBackup(LanguageManager languageManager)
        {
            this.languageManager = languageManager;
        }
        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            Console.WriteLine(languageManager.GetTranslation("start_diff_backup"));
            Console.WriteLine(languageManager.GetTranslation("input_complete_backup_path"));
            string lastFullBackupDir = Console.ReadLine();

            if (!Directory.Exists(jobBackup.TargetDirectory))
            {
                Directory.CreateDirectory(jobBackup.TargetDirectory);
            }

            try
            {
                var lastFullBackupFiles = Directory.GetFiles(lastFullBackupDir, "*", SearchOption.AllDirectories)
                                                    .Select(f => f.Substring(lastFullBackupDir.Length + 1))
                                                    .ToList();

                var currentFiles = Directory.GetFiles(jobBackup.SourceDirectory, "*", SearchOption.AllDirectories)
                                            .Select(f => f.Substring(jobBackup.SourceDirectory.Length + 1))
                                            .ToList();

                foreach (var file in currentFiles)
                {
                    var sourceFilePath = Path.Combine(jobBackup.SourceDirectory, file);
                    var differentialBackupFilePath = Path.Combine(jobBackup.TargetDirectory, file);
                    var lastFullBackupFilePath = Path.Combine(lastFullBackupDir, file);

                    if (!File.Exists(lastFullBackupFilePath) || File.GetLastWriteTime(sourceFilePath) > File.GetLastWriteTime(lastFullBackupFilePath))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(differentialBackupFilePath));
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        File.Copy(sourceFilePath, differentialBackupFilePath, true);
                        stopwatch.Stop();
                        var fileManager = new CryptoSoft.FileManager(differentialBackupFilePath, "EasySave");
                        int ElapsedTime = fileManager.TransformFile();

                        loggerStrategy.Update(jobBackup.Name, sourceFilePath, differentialBackupFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, ElapsedTime);
                        loggerStrategy.DisplayLogFileContent();

                        Console.WriteLine((languageManager.GetTranslation("copied")) + $" : {sourceFilePath}" + (languageManager.GetTranslation("to")) + $"{differentialBackupFilePath}");
                    }
                }
                Console.WriteLine(languageManager.GetTranslation("diff_backup_finished"));
            }
            catch (Exception ex)
            {
                Console.WriteLine((languageManager.GetTranslation("error_diff_backup")) + $"{ex.Message}");
            }
        }

        public void Restore(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            Console.WriteLine(languageManager.GetTranslation("start_backup_restore"));
            Console.WriteLine(languageManager.GetTranslation("input_complete_backup_path"));
            string lastFullBackupDir = Console.ReadLine();

            var tempBackupJob = new BackupJob(jobBackup.Name, lastFullBackupDir, jobBackup.SourceDirectory, jobBackup.BackupStrategy, jobBackup.StateManager);

            try
            {
                var completeBackup = new CompleteBackup(this.languageManager);
                // Restaurer d'abord la dernière sauvegarde totale en utilisant la stratégie complète
                completeBackup.Restore(tempBackupJob, loggerStrategy);

                // Utiliser la méthode utilitaire pour copier les fichiers et sous-répertoires de la sauvegarde différentielle
                FileManager.CopyDirectory(jobBackup.TargetDirectory, jobBackup.SourceDirectory);

                loggerStrategy.Update(jobBackup.Name, jobBackup.TargetDirectory, jobBackup.SourceDirectory, new FileInfo(jobBackup.TargetDirectory).Length, 10, 10);

                Console.WriteLine(languageManager.GetTranslation("restore_success"));
            }
            catch (Exception ex)
            {
                Console.WriteLine((languageManager.GetTranslation("restore_error")) + $"{ex.Message}");
            }
        }
    }
}

