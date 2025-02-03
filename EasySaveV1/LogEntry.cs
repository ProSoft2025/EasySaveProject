namespace EasySave
{
    public class LogEntry
    {
        public string Timestamp { get; set; }
        public string TaskName { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public int FileSize { get; set; }
        public int TransferTime { get; set; }
    }
}
