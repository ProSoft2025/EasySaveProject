namespace EasySave
{
    public class CompleteBackup : IBackupStrategy
    {
        public void ExecuteBackup(string source, string target)
        {

        }
        public static void Restore(string backupDirectory, string restoreDirectory)
        {
            try
            {
                // Créer le répertoire de restauration s'il n'existe pas
                if (!Directory.Exists(restoreDirectory))
                {
                    Directory.CreateDirectory(restoreDirectory);
                }

                // Utiliser la méthode utilitaire pour copier les fichiers et sous-répertoires
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
