using BackupLogger;
using CryptoSoft;
using System.Diagnostics;

namespace EasySaveV1
{
    public class DifferentialBackup : IBackupStrategy
    {
        private readonly LanguageManager languageManager;
        private readonly StateManager stateManager;
        private readonly string lastFullBackupDir;

        public DifferentialBackup(LanguageManager languageManager, StateManager stateManager, string lasFullBackupDir)
        {
            this.languageManager = languageManager;
            this.stateManager = stateManager;
            this.lastFullBackupDir = lasFullBackupDir;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobBackup"></param>
        /// <param name="loggerStrategy"></param>
        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
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

                // Separate priority files from non-priority files
                var priorityFiles = currentFiles.Where(f => jobBackup.PriorityExtensions.Contains(Path.GetExtension(f))).ToList();
                var nonPriorityFiles = currentFiles.Except(priorityFiles).ToList();

                // Combine priority files first, then non-priority files
                var orderedFiles = priorityFiles.Concat(nonPriorityFiles).ToList();

                int totalFiles = currentFiles.Count;
                long totalSize = currentFiles.Sum(file => new FileInfo(Path.Combine(jobBackup.SourceDirectory, file)).Length);
                int filesProcessed = 0;
                long sizeProcessed = 0;

                stateManager.UpdateState(new StateEntry
                {
                    TaskName = jobBackup.Name,
                    Timestamp = DateTime.Now,
                    Status = "Running",
                    TotalFiles = totalFiles,
                    TotalSize = totalSize,
                    Progress = 0,
                    RemainingFiles = totalFiles - filesProcessed,
                    RemainingSize = totalSize - sizeProcessed
                });

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
                        // Mise à jour de l'état après chaque fichier copié
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
                        }
                        else
                        {
                            loggerStrategy.Update(jobBackup.Name, sourceFilePath, differentialBackupFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, 0);
                        }
                    }
                }

                jobBackup.Status = BackupStatus.Completed;
                loggerStrategy.DisplayLogFileContent();
            }

            catch (Exception ex)
            {
                jobBackup.Status = BackupStatus.Error;
                stateManager.UpdateState(new StateEntry
                {TaskName = jobBackup.Name, Timestamp = DateTime.Now, Status = "Error", CurrentSource=ex.ToString(), CurrentTarget=lastFullBackupDir});
                // Console.WriteLine(languageManager.GetTranslation("error_diff_backup") + $"{ex.Message}");
            }
        }

        public void Restore(BackupJob jobBackup, ILoggerStrategy loggerStrategy, string lastFullBackupDir)
        {
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
                Console.WriteLine(languageManager.GetTranslation("restore_error") + $"{ex.Message}");
            }
        }
    }
}
