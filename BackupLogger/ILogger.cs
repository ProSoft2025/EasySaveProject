using System.Text.Json;

namespace BackupLogger
{
    public interface ILogger
    {
        public void LogAction(LogEntry entry);
        public void DisplayLogFileContent();
    }
}