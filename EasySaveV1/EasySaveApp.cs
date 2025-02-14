using EasySave;

namespace EasySave
{
    public class EasySaveApp
    {
        // Déclaration des variables
        private static EasySaveApp? _instance;

        private static readonly object _lock = new object();
        public List<BackupJob> BackupJobs { get; set; } = new List<BackupJob>();
        public List<string> ExtensionToEncrypt { get; set; } = new List<string>();

        private EasySaveApp() { }


        public static EasySaveApp GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new EasySaveApp();
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
                Console.WriteLine($"Sauvegarde '{name}' supprimée avec succès.");
            }
            else
            {
                Console.WriteLine($"Aucune sauvegarde trouvée avec le nom '{name}'.");
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
