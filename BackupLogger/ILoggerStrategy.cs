using System;

/// <summary>
/// Represent the base class for all log strategies
/// </summary>

namespace BackupLogger
{
    public abstract class ILoggerStrategy
    {

        public string Timestamp { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string TaskName { get; set; } = String.Empty;
        public string SourcePath { get; set; } = String.Empty;
        public string TargetPath { get; set; } = String.Empty;
        public long FileSize { get; set; } = 0;
        public int TransferTime { get; set; } = 0;

        public abstract void Update(string taskName, string sourcePath, string targetPath, long fileSize, int transferTime);

        public abstract void DisplayLogFileContent();
    }
}