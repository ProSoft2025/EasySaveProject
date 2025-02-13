using BackupLogger;
using CryptoSoft;
using System.Diagnostics;

namespace EasySave
{
    public class CompleteBackup : IBackupStrategy
    {
        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
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
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    File.Copy(sourceFilePath, targetFilePath, true);
                    stopwatch.Stop();

                    var fileExtension = Path.GetExtension(sourceFilePath);
                    if (jobBackup.extensionsToEncrypt.Contains(fileExtension))
                    {
                        var fileManager = new CryptoSoft.FileManager(targetFilePath, "EasySaveCESICESICESICESI");
                        int ElapsedTime = fileManager.TransformFile();
                        loggerStrategy.Update(jobBackup.Name, sourceFilePath, targetFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, ElapsedTime);
                        Console.WriteLine($"{targetFilePath} a été chiffré");
                    }
                    else
                    {
                        loggerStrategy.Update(jobBackup.Name, sourceFilePath, targetFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, 0);
                        Console.WriteLine($"Copié : {sourceFilePath} vers {targetFilePath}");
                    }
                }
                loggerStrategy.DisplayLogFileContent();
                Console.WriteLine("La sauvegarde totale est terminée.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde totale : {ex.Message}");
            }
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

                Console.WriteLine("Restauration des fichiers effectuée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la restauration des fichiers : {ex.Message}");
            }
        }
    }
}