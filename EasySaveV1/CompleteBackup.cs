using BackupLogger;

namespace EasySave
{
    public class CompleteBackup : IBackupStrategy
    {
        public void ExecuteBackup(BackupJob jobBackup, LoggerService serviceLogger)
        {
            Console.WriteLine("Début de la sauvegarde totale.");

            try
            {
                var sourceFiles = Directory.GetFiles(jobBackup.SourceDirectory, "*", SearchOption.AllDirectories)
                                            .Select(f => f.Substring(jobBackup.SourceDirectory.Length + 1))
                                            .ToList();

                foreach (var file in sourceFiles)
                {
                    var sourceFilePath = Path.Combine(jobBackup.SourceDirectory, file);
                    var targetFilePath = Path.Combine(jobBackup.TargetDirectory, file);

                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
                    File.Copy(sourceFilePath, targetFilePath, true);

                    LogEntry EntreeLog = new LogEntry(jobBackup.Name, sourceFilePath, targetFilePath, new FileInfo(sourceFilePath).Length, 10);
                    serviceLogger.GetBackupLogger().LogAction(EntreeLog);

                    Console.WriteLine($"Copié : {sourceFilePath} vers {targetFilePath}");
                }
                Console.WriteLine("La sauvegarde totale est terminée.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde totale : {ex.Message}");
            }
        }

        public void Restore(BackupJob jobBackup, LoggerService serviceLogger)
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

                Console.WriteLine("Restauration des fichiers effectuée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la restauration des fichiers : {ex.Message}");
            }
        }
    }
}