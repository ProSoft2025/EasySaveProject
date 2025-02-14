using EasySave;

namespace EasySave
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

        public void AddBackup(BackupJob job)
        {
            if (BackupJobs.Count < 5)
            {
                BackupJobs.Add(job);
            }
            else
            {
                Console.Error.WriteLine(languageManager.GetTranslation("5_backup_rule"));
            }
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
    }
}
