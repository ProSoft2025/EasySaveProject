using System;

namespace EasySaveV1
{
    public class BackupJobFactory
    {
        public BackupJobFactory() { }

        public BackupJob CreateBackupJob(string name, string source, string target, IBackupStrategy strategy, StateManager stateManager)
        {
            return new BackupJob(name, source, target, strategy);
        }
    }
}
