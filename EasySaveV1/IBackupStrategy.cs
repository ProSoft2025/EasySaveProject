using BackupLogger;

namespace EasySaveV1
{
    public interface IBackupStrategy
    {
        public void ExecuteBackup(BackupJob jobBackup, LoggerService serviceLogger) { }
        public void Restore(BackupJob jobBackup, LoggerService serviceLogger) { }
    }
}
