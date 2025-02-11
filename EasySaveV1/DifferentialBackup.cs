using BackupLogger;

namespace EasySave
{
    public class DifferentialBackup : IBackupStrategy
    {
        public void ExecuteBackup(BackupJob jobBackup, LoggerService serviceLogger)
        {
            Console.WriteLine("Début de la sauvegarde différentielle.");
            Console.WriteLine("Saisir le chemin de la dernière sauvegarde totale :");
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
                        File.Copy(sourceFilePath, differentialBackupFilePath, true);

                        LogEntry EntreeLog = new LogEntry(jobBackup.Name, sourceFilePath, differentialBackupFilePath, new FileInfo(sourceFilePath).Length, 10);
                        serviceLogger.GetBackupLogger().LogAction(EntreeLog);

                        Console.WriteLine($"Copié : {sourceFilePath} vers {differentialBackupFilePath}");
                    }
                }
                Console.WriteLine("La sauvegarde différentielle est terminée");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde différentielle : {ex.Message}");
            }
        }

        public void Restore(BackupJob jobBackup, LoggerService serviceLogger)
        {
            Console.WriteLine("Début de la restauration de la sauvegarde :");
            Console.WriteLine("Saisir le chemin de la dernière sauvegarde totale :");
            string lastFullBackupDir = Console.ReadLine();

            var tempBackupJob = new BackupJob(jobBackup.Name, lastFullBackupDir, jobBackup.SourceDirectory, jobBackup.BackupStrategy, jobBackup.StateManager);

            try
            {
                var completeBackup = new CompleteBackup();
                // Restaurer d'abord la dernière sauvegarde totale en utilisant la stratégie complète
                completeBackup.Restore(tempBackupJob, serviceLogger);

                // Utiliser la méthode utilitaire pour copier les fichiers et sous-répertoires de la sauvegarde différentielle
                FileManager.CopyDirectory(jobBackup.TargetDirectory, jobBackup.SourceDirectory);

                LogEntry EntreeLog = new LogEntry(jobBackup.Name, jobBackup.TargetDirectory, jobBackup.SourceDirectory, new FileInfo(jobBackup.TargetDirectory).Length, 10);
                serviceLogger.GetBackupLogger().LogAction(EntreeLog);

                Console.WriteLine("Restauration des fichiers effectuée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la restauration des fichiers : {ex.Message}");
            }
        }
    }
}

