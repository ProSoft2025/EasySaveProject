using EasySave;

namespace EasySave
{
    public class EasySaveApp
    {
        // Déclaration des variables
        public List<BackupJob> BackupJobs { get; set; } = new List<BackupJob>();

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
        public void RemoveBackup(BackupJob job)
        {
            BackupJobs.Remove(job);
        }
        public void ExecuteBackupJob (string[] names)
        {
            foreach (var name in names)
            {
                var job = BackupJobs.Find(j => j.name == name);
                job?.Execute();
            }
        }
        public void ExecuteAllBackupJobs()
        {
            foreach (var job in BackupJobs)
            {
                job.Execute();
            }
        }
    }
}
