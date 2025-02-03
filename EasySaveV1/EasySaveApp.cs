using EasySave;

namespace EasySave
{
    public class EasySaveApp
    {
        // Déclaration des variables
        public List<BackupJob> BackupJobs { get; set; } = new List<BackupJob>();

        public void AddBackup(BackupJob job)
        {
            BackupJobs.Add(job);
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
