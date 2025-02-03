using EasySave;

namespace EasySave
{
    public class BackupJobFactory
    {
        public BackupJobFactory() { }

        public BackupJob CreateBackupJob (string name, string source, string target, BackupStrategy strategy)
        {
            return new BackupJob(name, source, target, strategy);
        }
    }
}
