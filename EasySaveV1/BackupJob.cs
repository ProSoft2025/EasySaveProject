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
        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public StateManager StateManager { get; set; }
        public IBackupStrategy BackupStrategy { get; set; }
        public List<string> extensionsToEncrypt { get; set; } = new List<string>();
        public List<string> PriorityExtensions { get; private set; } = new List<string>(); // New property for priority extensions
        public BackupStatus Status { get; set; } = BackupStatus.Idle;

        public BackupJob(string name, string sourceDirectory, string targetDirectory, IBackupStrategy backupStrategy, StateManager stateManager)
        {
            Name = name;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            BackupStrategy = backupStrategy;
            StateManager = stateManager;
        }

        public void Execute(ILoggerStrategy logger)
        {
            BackupStrategy.ExecuteBackup(this, logger);
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
            extensionsToEncrypt = extensions;
        }

        public void UpdatePriorityExtensions(List<string> extensions)
        {
            PriorityExtensions = extensions;
        }

        public bool HasPendingPriorityExtensions()
        {
            var currentFiles = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories)
                                        .Select(f => Path.GetExtension(f))
                                        .ToList();

            return PriorityExtensions.Any(ext => currentFiles.Contains(ext));
        }
    }
}