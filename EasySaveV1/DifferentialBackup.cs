using BackupLogger;
using System.Diagnostics;

namespace EasySaveV1
{
    public class DifferentialBackup : IBackupStrategy
    {
        private readonly LanguageManager languageManager;
        private readonly StateManager stateManager;

        public DifferentialBackup(LanguageManager languageManager, StateManager stateManager)
        {
            this.languageManager = languageManager;
            this.stateManager = stateManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobBackup"></param>
        /// <param name="loggerStrategy"></param>
        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            Console.WriteLine(languageManager.GetTranslation("start_diff_backup"));
            Console.WriteLine(languageManager.GetTranslation("input_complete_backup_path"));
            string lastFullBackupDir = Console.ReadLine();

            if (!Directory.Exists(jobBackup.TargetDirectory))
            {
                Directory.CreateDirectory(jobBackup.TargetDirectory);
            }

            jobBackup.Status = BackupStatus.Running;
            stateManager.UpdateState(new StateEntry { TaskName = jobBackup.Name, Timestamp = DateTime.Now, Status = "Running" });

            try
            {
                var lastFullBackupFiles = Directory.GetFiles(lastFullBackupDir, "*", SearchOption.AllDirectories)
                                                   .Select(f => f.Substring(lastFullBackupDir.Length + 1))
                                                   .ToList();

                var currentFiles = Directory.GetFiles(jobBackup.SourceDirectory, "*", SearchOption.AllDirectories)
                                            .Select(f => f.Substring(jobBackup.SourceDirectory.Length + 1))
                                            .ToList();

                int totalFiles = currentFiles.Count;
                long totalSize = currentFiles.Sum(file => new FileInfo(Path.Combine(jobBackup.SourceDirectory, file)).Length);
                int filesProcessed = 0;
                long sizeProcessed = 0;

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

                        filesProcessed++;
                        sizeProcessed += new FileInfo(sourceFilePath).Length;

                        // 🔹 Mise à jour de l'état après chaque fichier copié
                        StateEntry state = new StateEntry
                        {
                            TaskName = jobBackup.Name,
                            Timestamp = DateTime.Now,
                            Status = "Running",
                            TotalFiles = totalFiles,
                            TotalSize = totalSize,
                            Progress = (int)((double)filesProcessed / totalFiles * 100),
                            RemainingFiles = totalFiles - filesProcessed,
                            RemainingSize = totalSize - sizeProcessed,
                            CurrentSource = sourceFilePath,
                            CurrentTarget = differentialBackupFilePath
                        };
                        stateManager.UpdateState(state);

                        var fileExtension = Path.GetExtension(sourceFilePath);
                        if (jobBackup.ExtensionsToEncrypt.Contains(fileExtension))
                        {
                            var fileManager = new CryptoSoft.FileManager(differentialBackupFilePath, "EasySave");
                            int elapsedTime = fileManager.TransformFile();
                            loggerStrategy.Update(jobBackup.Name, sourceFilePath, differentialBackupFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, elapsedTime);
                            Console.WriteLine(differentialBackupFilePath + languageManager.GetTranslation("encrypted"));
                        }
                        else
                        {
                            loggerStrategy.Update(jobBackup.Name, sourceFilePath, differentialBackupFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, 0);
                            Console.WriteLine(languageManager.GetTranslation("copied") + $" : {sourceFilePath}" + languageManager.GetTranslation("to") + $"{differentialBackupFilePath}");
                        }
                    }
                }

                jobBackup.Status = BackupStatus.Completed;
                stateManager.UpdateState(new StateEntry { TaskName = jobBackup.Name, Timestamp = DateTime.Now, Status = "Completed" });

                loggerStrategy.DisplayLogFileContent();
                Console.WriteLine(languageManager.GetTranslation("diff_backup_finished"));
            }
            catch (Exception ex)
            {
                jobBackup.Status = BackupStatus.Error;
                stateManager.UpdateState(new StateEntry { TaskName = jobBackup.Name, Timestamp = DateTime.Now, Status = "Error" });
                Console.WriteLine(languageManager.GetTranslation("error_diff_backup") + $"{ex.Message}");
            }
        }
    



        public void Restore(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            Console.WriteLine(languageManager.GetTranslation("start_backup_restore"));
            Console.WriteLine(languageManager.GetTranslation("input_complete_backup_path"));
            string lastFullBackupDir = Console.ReadLine();

            var tempBackupJob = new BackupJob(jobBackup.Name, lastFullBackupDir, jobBackup.SourceDirectory, jobBackup.BackupStrategy);

            try
            {
                var completeBackup = new CompleteBackup(this.languageManager, this.stateManager);
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

