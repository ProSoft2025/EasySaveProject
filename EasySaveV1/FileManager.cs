namespace EasySave
{
    public class FileManager
    {
        static void ListDirectories(string source)
        {
            try
            {
                foreach (string directory in Directory.GetDirectories(source))
                {
                    Console.WriteLine(directory);
                    ListDirectories(directory); // Appel récursif pour lister les sous-répertoires
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Accès refusé à {source}: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors de la lecture de {source}: {e.Message}");
            }
        }

        public static void CopyDirectory(string sourceDir, string destinationDir)
        {
            // Créer le dossier de destination si nécessaire
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Copier les fichiers du dossier source vers le dossier de destination
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            // Copier les sous-dossiers de manière récursive
            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                string destDir = Path.Combine(destinationDir, Path.GetFileName(directory));
                CopyDirectory(directory, destDir);
            }
        }
    }
}
