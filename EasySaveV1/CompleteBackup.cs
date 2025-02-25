using BackupLogger;
using CryptoSoft;
using System.Diagnostics;

namespace EasySaveV1
{
    public class CompleteBackup : IBackupStrategy
    {
        private LanguageManager languageManager;
        private readonly StateManager stateManager;

        public CompleteBackup(LanguageManager languageManager, StateManager stateManager)
        {
            this.languageManager = languageManager;
            this.stateManager = stateManager;
        }
        /// <summary>
        /// Used to do a full copy of a directory including logs, encryption, state
        /// </summary>
        /// <param name="jobBackup">The job on which the copy is executed</param>
        /// <param name="loggerStrategy">Instance of the logger used to write in log</param>
        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            jobBackup.Status = BackupStatus.Running; // Définir l’état comme en cours d’exécution

            try
            {
                var sourceFiles = Directory.GetFiles(jobBackup.SourceDirectory, "*", SearchOption.AllDirectories)
                                           .Select(f => f.Substring(jobBackup.SourceDirectory.Length + 1))
                                           .ToList();

                int totalFiles = sourceFiles.Count;
                long totalSize = sourceFiles.Sum(file => new FileInfo(Path.Combine(jobBackup.SourceDirectory, file)).Length);
                int filesProcessed = 0;
                long sizeProcessed = 0;

                stateManager.UpdateState(new StateEntry {TaskName = jobBackup.Name, Timestamp = DateTime.Now, Status = "Running", TotalFiles = totalFiles,
                    TotalSize = totalSize, Progress = 0, RemainingFiles = totalFiles - filesProcessed, RemainingSize = totalSize - sizeProcessed});

                foreach (var file in sourceFiles)
                {
                    var sourceFilePath = Path.Combine(jobBackup.SourceDirectory, file);
                    var targetFilePath = Path.Combine(jobBackup.TargetDirectory, file);

                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    File.Copy(sourceFilePath, targetFilePath, true);

                    filesProcessed++;
                    sizeProcessed += new FileInfo(sourceFilePath).Length;

                    // Update the state after each file copied
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
                        CurrentTarget = targetFilePath
                    };
                    stateManager.UpdateState(state);

                    stopwatch.Stop();

                    // Manage file encryption
                    var fileExtension = Path.GetExtension(sourceFilePath);
                    if (jobBackup.ExtensionsToEncrypt.Contains(fileExtension))
                    {
                        var fileManager = new CryptoSoft.FileManager(targetFilePath, "EasySave");
                        int ElapsedTime = fileManager.TransformFile();
                        loggerStrategy.Update(jobBackup.Name, sourceFilePath, targetFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, ElapsedTime);

                    }
                    else
                    {
                        loggerStrategy.Update(jobBackup.Name, sourceFilePath, targetFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, 0);
                    }
                }

                // Update status once completed
                jobBackup.Status = BackupStatus.Completed;
                stateManager.UpdateState(new StateEntry
                {
                    TaskName = jobBackup.Name,
                    Timestamp = DateTime.Now,
                    Status = "Completed",
                    TotalFiles = totalFiles,
                    TotalSize = totalSize,
                    Progress = 100,
                    RemainingFiles = totalFiles - filesProcessed,
                    RemainingSize = totalSize - sizeProcessed
                });
                loggerStrategy.DisplayLogFileContent();
            }
            catch (Exception ex)
            {
                jobBackup.Status = BackupStatus.Error; // Gestion des erreurs
                stateManager.UpdateState(new StateEntry { TaskName = jobBackup.Name, Timestamp = DateTime.Now, Status = "Error" });
            }
        }

        /// <summary>
        /// Used to restore the source directory to the state he was during the backup
        /// </summary>
        /// <param name="jobBackup">The backup to restore</param>
        /// <param name="loggerStrategy">Instance of the logger used to write in log</param>
        public void Restore(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            try
            {
                // Error verification if the source already exist
                if (Directory.Exists(jobBackup.SourceDirectory))
                {
                    Directory.Delete(jobBackup.SourceDirectory, true);
                }

                // Create a temporary Backupjob to simplify the code using Execute method
                var tempBackupJob = new BackupJob
                (
                    jobBackup.Name,
                    jobBackup.TargetDirectory,
                    jobBackup.SourceDirectory,
                    jobBackup.BackupStrategy
                );

                tempBackupJob.ExtensionsToEncrypt = jobBackup.ExtensionsToEncrypt;

                ExecuteBackup(tempBackupJob, loggerStrategy);
            }
            catch (Exception ex)
            { }
        }
    }
}