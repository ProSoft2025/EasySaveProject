using System.Xml.Serialization;
using BackupLogger;

public class XmlLogger : ILogger
{
    public string LogFilePath { get; set; }

    public XmlLogger(string logFilePath)
    {
        LogFilePath = logFilePath;
    }

    public void LogAction(LogEntry entry)
    {
        var logEntries = new List<LogEntry>();
        if (File.Exists(LogFilePath))
        {
            var existingLogs = File.ReadAllText(LogFilePath);
            using (var stringReader = new StringReader(existingLogs))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<LogEntry>));
                logEntries = (List<LogEntry>)xmlSerializer.Deserialize(stringReader) ?? new List<LogEntry>();
            }
        }
        logEntries.Add(entry);

        using (var stringWriter = new StringWriter())
        {
            var xmlSerializer = new XmlSerializer(typeof(List<LogEntry>));
            xmlSerializer.Serialize(stringWriter, logEntries);
            File.WriteAllText(LogFilePath, stringWriter.ToString());
        }
    }

    public void DisplayLogFileContent()
    {
        if (File.Exists(LogFilePath))
        {
            Console.WriteLine($"Contenu du log ({LogFilePath}):");
            Console.WriteLine(File.ReadAllText(LogFilePath));
        }
        else
        {
            Console.WriteLine("Log file not found.");
        }
    }
}