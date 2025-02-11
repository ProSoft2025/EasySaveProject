using System;



namespace BackupLogger
{
    public class LogEntry
    {
        public string Timestamp { get; set; }
        public string TaskName { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public long FileSize { get; set; }
        public int TransferTime { get; set; }

        //constructor
        public LogEntry(string taskName, string sourcePath, string targetPath, long fileSize, int transferTime)
        {
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            TaskName = taskName;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            FileSize = fileSize;
            TransferTime = transferTime;
        }
    }

}