using BackupLogger;

namespace EasySaveV1
{
    public interface IBackupStrategy
    {
        public void ExecuteBackup(BackupJob jobBackup, ILoggerStrategy serviceLogger) { }
        public void Restore(BackupJob jobBackup, ILoggerStrategy serviceLogger) { }
    }
}
