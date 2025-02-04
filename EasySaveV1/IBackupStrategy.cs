namespace EasySave
{
    public interface IBackupStrategy
    {
        public void ExecuteBackup(string source, string target) { }
    }
}
