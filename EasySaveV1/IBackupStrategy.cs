namespace EasySave
{
    public interface IBackupStrategy
    {
        public void ExecuteBackup(string source, string target) { }
        public void Restore(string backupDirectory, string restoreDirectory) { }
    }
}
