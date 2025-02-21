using BackupLogger;

namespace EasySaveV1
{
    public enum BackupStatus
    {
        Idle,   // En attente
        Running,
        Paused,
        Stopped,
        Completed,
        Error
    }

    public class BackupJob
    {
        

        private bool _isSelected;
        private bool _isPaused;

        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public StateManager StateManager { get; set; }
        public IBackupStrategy BackupStrategy { get; set; }
        public List<string> ExtensionsToEncrypt { get; set; } = new List<string>();

        public BackupStatus Status { get; set; } = BackupStatus.Idle;


        public BackupJob(string name, string sourceDirectory, string targetDirectory, IBackupStrategy backupStrategy, StateManager stateManager)
        {
            Name = name;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            StateManager = stateManager;
            BackupStrategy = backupStrategy;
        }


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
                while (_isPaused)
                {
                    Thread.Sleep(100); // Attendre que la pause soit terminée
                }

                string destination = Path.Combine(TargetDirectory, Path.GetFileName(file));
                // File.Copy(file, destination, true);
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
        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        public void Restore(ILoggerStrategy logger) 
        { 
            BackupStrategy.Restore(this, logger);
        }

        public void displayAttributs()
        {
            string strategyType = BackupStrategy is CompleteBackup ? "Complete" :
                              BackupStrategy is DifferentialBackup ? "Differential" : "N/A";
            Console.WriteLine("Name:" + Name + "\nSource:" + SourceDirectory + "\nDestination:" + TargetDirectory + "\nStrategy:" + strategyType);
        }
        public void updateExtensionsToEncrypt(List<string> extensions)
        {
            ExtensionsToEncrypt = extensions;
        }

    }
}
