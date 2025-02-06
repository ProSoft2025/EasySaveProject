namespace EasySave
{
    public class DifferentialBackup : IBackupStrategy
    {
        public void ExecuteBackup(string source, string target, string lastFullBackupDir)
        {
            Console.WriteLine("Début de la sauvegarde différentielle.");

            try
            {
                var lastFullBackupFiles = Directory.GetFiles(lastFullBackupDir, "*", SearchOption.AllDirectories)
                                                    .Select(f => f.Substring(lastFullBackupDir.Length + 1))
                                                    .ToList();

                var currentFiles = Directory.GetFiles(source, "*", SearchOption.AllDirectories)
                                            .Select(f => f.Substring(source.Length + 1))
                                            .ToList();

                foreach (var file in currentFiles)
                {
                    var sourceFilePath = Path.Combine(source, file);
                    var differentialBackupFilePath = Path.Combine(target, file);
                    var lastFullBackupFilePath = Path.Combine(lastFullBackupDir, file);

                    if (!File.Exists(lastFullBackupFilePath) || File.GetLastWriteTime(sourceFilePath) > File.GetLastWriteTime(lastFullBackupFilePath))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(differentialBackupFilePath));
                        File.Copy(sourceFilePath, differentialBackupFilePath, true);

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

        public void Restore(string backupDirectory, string restoreDirectory, string lastFullBackupDir)
        {
            try
            {
                // Restaurer d'abord la dernière sauvegarde totale en utilisant la stratégie complète
                CompleteBackup.Restore(lastFullBackupDir, restoreDirectory);

                // Utiliser la méthode utilitaire pour copier les fichiers et sous-répertoires de la sauvegarde différentielle
                FileManager.CopyDirectory(backupDirectory, restoreDirectory);

                Console.WriteLine("Restauration des fichiers effectuée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la restauration des fichiers : {ex.Message}");
            }
        }
    }
}

