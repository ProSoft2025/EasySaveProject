namespace EasySave
{
    public class CompleteBackup : IBackupStrategy
    {
        public void ExecuteBackup(string source, string target)
        {
            Console.WriteLine("Début de la sauvegarde totale.");

            try
            {
                var sourceFiles = Directory.GetFiles(source, "*", SearchOption.AllDirectories)
                                            .Select(f => f.Substring(source.Length + 1))
                                            .ToList();

                foreach (var file in sourceFiles)
                {
                    var sourceFilePath = Path.Combine(source, file);
                    var targetFilePath = Path.Combine(target, file);

                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
                    File.Copy(sourceFilePath, targetFilePath, true);

                    Console.WriteLine($"Copié : {sourceFilePath} vers {targetFilePath}");
                }
                Console.WriteLine("La sauvegarde totale est terminée.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde totale : {ex.Message}");
            }
        }
    }
}
