using BackupLogger;
using CryptoSoft;
using System.Diagnostics;

namespace EasySaveV1
{
    public class CompleteBackup : IBackupStrategy
    {
        private LanguageManager languageManager;

        public CompleteBackup(LanguageManager languageManager)
        {
            this.languageManager = languageManager;
        }

        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy loggerStrategy)
        {
            //Console.WriteLine(languageManager.GetTranslation("start_complete_backup"));

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
                    if (jobBackup.ExtensionsToEncrypt.Contains(fileExtension))
                    {
                        var fileManager = new CryptoSoft.FileManager(targetFilePath, "EasySave");
                        int ElapsedTime = fileManager.TransformFile();
                        loggerStrategy.Update(jobBackup.Name, sourceFilePath, targetFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, ElapsedTime);
                        Console.WriteLine($"{targetFilePath} a été chiffré");
                    }
                    else
                    {
                        loggerStrategy.Update(jobBackup.Name, sourceFilePath, targetFilePath, new FileInfo(sourceFilePath).Length, stopwatch.ElapsedMilliseconds, 0);
                        //Console.WriteLine((languageManager.GetTranslation("copied")) + $" {sourceFilePath}" + (languageManager.GetTranslation("to")) + $"{targetFilePath}");
                    }
                }
                loggerStrategy.DisplayLogFileContent();
                //Console.WriteLine(languageManager.GetTranslation(("complete_backup_finished")));                    
            }
            catch (Exception ex)
            {
               // Console.WriteLine((languageManager.GetTranslation("complete_backup_error")) + $"{ex.Message}");
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

                Console.WriteLine(languageManager.GetTranslation("restore_success"));
            }
            catch (Exception ex)
            {
                Console.WriteLine((languageManager.GetTranslation("restore_error")) + $"{ex.Message}");
            }
        }
    }
}