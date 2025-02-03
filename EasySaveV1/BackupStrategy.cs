namespace EasySave
{
    public interface BackupStrategy
    {
        public void ExecuteBackup(string source, string target) { }
    }
}
