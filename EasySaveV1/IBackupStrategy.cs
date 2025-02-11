using BackupLogger;

namespace EasySave
{
    public interface IBackupStrategy
    {
        public void ExecuteBackup(BackupJob jobBackup, LoggerService serviceLogger) { }
        public void Restore(BackupJob jobBackup, LoggerService serviceLogger) { }
    }
}
