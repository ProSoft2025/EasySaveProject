using System.Runtime.CompilerServices;
using System.Xml.Linq;
using BackupLogger;

namespace EasySave
{

    public class BackupJob
    {
        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public StateManager StateManager { get; set; }
        public IBackupStrategy BackupStrategy { get; set; }

        public BackupJob(string name, string sourceDirectory, string targetDirectory, IBackupStrategy backupStrategy, StateManager stateManager)
        {
            Name = name;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            StateManager = stateManager;
            BackupStrategy = backupStrategy;
        }

        // Executes the backup job and updates state after each file copy
        public void Execute(ILoggerStrategy logger)
        {

            BackupStrategy.ExecuteBackup(this, logger);

            if (!Directory.Exists(SourceDirectory) || !Directory.Exists(TargetDirectory))
            {
                Console.WriteLine("Error: Source or Target directory does not exist.");
                return;
            }


            string[] files = Directory.GetFiles(SourceDirectory);
            long totalSize = 0;
            foreach (string file in files)
                totalSize += new FileInfo(file).Length;

            int totalFiles = files.Length;
            int filesProcessed = 0;
            long sizeProcessed = 0;

            foreach (string file in files)
            {
                string destination = Path.Combine(TargetDirectory, Path.GetFileName(file));
                File.Copy(file, destination, true);
                filesProcessed++;
                sizeProcessed += new FileInfo(file).Length;

                // Update the state after each file transfer
                StateEntry state = new StateEntry
                {
                    TaskName = Name,
                    Timestamp = DateTime.Now,
                    Status = "Active",
                    TotalFiles = totalFiles,
                    TotalSize = totalSize,
                    Progress = (int)((double)filesProcessed / totalFiles * 100),
                    RemainingFiles = totalFiles - filesProcessed,
                    RemainingSize = totalSize - sizeProcessed,
                    CurrentSource = file,
                    CurrentTarget = destination
                };
                StateManager.UpdateState(state);
            }

            StateManager.UpdateState(new StateEntry { TaskName = Name, Timestamp = DateTime.Now, Status = "Completed" });
        }
        
        public void Restore(ILoggerStrategy logger) 
        { 
            BackupStrategy.Restore(this, logger);
        }

        public void displayAttributs()
        {
            string strategyType = BackupStrategy is CompleteBackup ? "Complete" :
                              BackupStrategy is DifferentialBackup ? "Differentielle" : "N/A";
            Console.WriteLine("Name:" + Name + "\nSource:" + SourceDirectory + "\nDestination:" + TargetDirectory + "\nStrategy:" + strategyType);
        }
    }
}
