namespace BackupLogger
{
    public interface IConfigManager
    {
        string LogDirectory { get; set; }
        string LogFormat { get; set; }
        void SaveConfig();
        void SetLogFormat(string format);
        void SetLogDirectory(string directory);
    }
}
