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

        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            jobBackup.Status = BackupStatus.Running; // Définir l’état comme en cours d’exécution
            stateManager.UpdateState(new StateEntry { TaskName = jobBackup.Name, Timestamp = DateTime.Now, Status = "Running" });

            var sourceFiles = Directory.GetFiles(jobBackup.SourceDirectory, "*", SearchOption.AllDirectories)
                                       .Select(f => f.Substring(jobBackup.SourceDirectory.Length + 1))
                                       .ToList();

            // Separate priority files from non-priority files
            var priorityFiles = sourceFiles.Where(f => jobBackup.PriorityExtensions.Contains(Path.GetExtension(f))).ToList();
            var nonPriorityFiles = sourceFiles.Except(priorityFiles).ToList();

            // Combine priority files first, then non-priority files
            var orderedFiles = priorityFiles.Concat(nonPriorityFiles).ToList();

            int totalFiles = sourceFiles.Count;
            long totalSize = sourceFiles.Sum(file => new FileInfo(Path.Combine(jobBackup.SourceDirectory, file)).Length);
            int filesProcessed = 0;
            long sizeProcessed = 0;

            foreach (var file in orderedFiles)
            {
                var sourceFilePath = Path.Combine(jobBackup.SourceDirectory, file);
                var targetFilePath = Path.Combine(jobBackup.TargetDirectory, file);

                Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                File.Copy(sourceFilePath, targetFilePath, true);

                filesProcessed++;
                sizeProcessed += new FileInfo(sourceFilePath).Length;

                // Mise à jour de l’état après chaque fichier copié
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

                // Gestion du chiffrement des fichiers
                var fileExtension = Path.GetExtension(sourceFilePath);
                if (jobBackup.extensionsToEncrypt.Contains(fileExtension))
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

            // Mise à jour du statut une fois terminé
            jobBackup.Status = BackupStatus.Completed;
            stateManager.UpdateState(new StateEntry { TaskName = jobBackup.Name, Timestamp = DateTime.Now, Status = "Completed" });

            loggerStrategy.DisplayLogFileContent();
        }

        public void Restore(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            try
            {
                // Créer le répertoire de restauration s'il n'existe pas
                if (!Directory.Exists(jobBackup.SourceDirectory))
                {
                    Directory.CreateDirectory(jobBackup.SourceDirectory);
                }

                // Utiliser la méthode utilitaire pour copier les fichiers et sous-répertoires
                FileManager.CopyDirectory(jobBackup.TargetDirectory, jobBackup.SourceDirectory);

                Console.WriteLine(languageManager.GetTranslation("restore_success"));
            }
            catch (Exception ex)
            {
                Console.WriteLine((languageManager.GetTranslation("restore_error")) + $"{ex.Message}");
            }
        }
    }
}