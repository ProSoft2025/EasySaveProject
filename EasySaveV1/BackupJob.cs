using System.Runtime.CompilerServices;

namespace EasySave
{
    public class BackupJob      //
    {
        
        public string name { get; set; }
        public string sourceDirectory { get; set; }
        public string targetDirectory { get; set; }
        public IBackupStrategy BackupStrategy { get; set; }

        public BackupJob(string name, string sourceDirectory, string targetDirectory, IBackupStrategy backupStrategy)
        {
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
            this.BackupStrategy = backupStrategy;
        }

        public void Execute()
        {
            BackupStrategy.ExecuteBackup(sourceDirectory, targetDirectory);            
        }

        public void displayAttributs()
        {
            string strategyType = BackupStrategy is CompleteBackup ? "Complete" :
                              BackupStrategy is DifferentialBackup ? "Differentielle" : "N/A";
            Console.WriteLine("Name:" + name + "\nSource:" + sourceDirectory + "\nDestination:" + targetDirectory + "\nStrategy:" + strategyType);
        }
    }
}
