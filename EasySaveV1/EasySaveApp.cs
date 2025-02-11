using EasySave;

namespace EasySave
{
    public class EasySaveApp
    {
        // Déclaration des variables
        private static EasySaveApp? _instance;

        private static readonly object _lock = new object();
        public List<BackupJob> BackupJobs { get; set; } = new List<BackupJob>();

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
        public void AddBackup(BackupJob job)
        {
            if (BackupJobs.Count < 5) {
                BackupJobs.Add(job);
            }
            else
            {
                Console.Error.WriteLine("Il y a déjà 5 sauvegardes, veuillez en supprimez une");
            }
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
    }
}
