using BackupLogger;

namespace EasySave
{
    public interface IBackupStrategy
    {
        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy serviceLogger) { }
        public void Restore(BackupJob jobBackup, ILoggerStrategy serviceLogger) { }
    }
}
