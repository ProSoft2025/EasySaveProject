

namespace EasySaveV1
{
    public class EasySaveApp
    {
        private LanguageManager languageManager;

        public EasySaveApp(LanguageManager languageManager)
        {
            this.languageManager = languageManager;
        }

        // Déclaration des variables
        private static EasySaveApp? _instance;

        private static readonly object _lock = new object();
        public List<BackupJob> BackupJobs { get; set; } = new List<BackupJob>();
        public List<string> ExtensionToEncrypt { get; set; } = new List<string>();

        private EasySaveApp() { }

        public static EasySaveApp GetInstance(LanguageManager languageManager)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new EasySaveApp(languageManager);
                    }
                }
            }
            return _instance;
        }
        public int AddBackup(BackupJob job)
        {
            // Vérification nom unique
            foreach (var backup in BackupJobs)
            {
                if (backup.Name == job.Name)
                {
                    return 1;
                }
            }
            BackupJobs.Add(job);
            return 0;
        }

        public void RemoveBackup(string name)
        {
            var job = BackupJobs.FirstOrDefault(j => j.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (job != null)
            {
                BackupJobs.Remove(job);
                Console.WriteLine((languageManager.GetTranslation("save")) + $"'{name}'" + (languageManager.GetTranslation("successfull_delete")));
            }
            else
            {
                Console.WriteLine((languageManager.GetTranslation("no_backup_found_name")) + $"'{name}'.");
            }
        }
        public void AddExtension(string extension)
        {
            ExtensionToEncrypt.Add(extension);
        }
        public void RemoveExtension(string extension)
        {
            if (ExtensionToEncrypt.Contains(extension))
                ExtensionToEncrypt.Remove(extension);
            else throw new Exception("Extension not found");
        }
    }
}
