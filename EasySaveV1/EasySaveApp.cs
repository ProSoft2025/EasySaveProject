using EasySave;

namespace EasySave
{
    public class EasySaveApp
    {
        // Déclaration des variables
        BackupJobs[] TabBackups { get; set; } = new BackupJobs[5];

        // Déclaration builder

        public EasySaveApp(BackupJobs[] TabBackups)
        {
            this.TabBackups = TabBackups;
        }

        public int FirstEmpty()
        {
            for (int i = 0; i < TabBackups.Length; i++)
            {
                if (TabBackups[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddBackup(BackupJobs BackupJob)
        {
            if (FirstEmpty() == -1)
            {
                Console.WriteLine("Le tableau est plein !");
            }
            else
            {
                TabBackups[FirstEmpty()] = BackupJob;
            }
        }
        
        public void AfficherSauvegardes ()
        {
            for (int i = 0;i < TabBackups.Length;i++)
            {
                // Console.WriteLine(TabBackups[i].AfficherAttributs());
            }
        }

    }
}
