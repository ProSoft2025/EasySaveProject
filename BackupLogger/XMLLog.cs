using System.Xml;

/// <summary>
/// Represents a log strategy that writes logs to an XML file
/// </summary>
/// 
namespace BackupLogger
{
    public class XMLLog : ILoggerStrategy
    {
        private IConfigManager configManager;

        public XMLLog(IConfigManager configManager)
        {
            this.configManager = configManager;

            if (!Directory.Exists(configManager.LogDirectory))
            {
                Directory.CreateDirectory(configManager.LogDirectory);
            }
        }

        public override void Update(string taskName, string sourcePath, string targetPath, long fileSize, long transferTime, int EncryptionTIme)
        {
            string logFileName = Path.Combine(configManager.LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.xml");

            XmlDocument doc = new XmlDocument();

            if (File.Exists(logFileName))
            {
                doc.Load(logFileName);
            }
            else
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(xmlDeclaration);
                XmlElement root = doc.CreateElement("Logs");
                doc.AppendChild(root);
            }

            XmlElement logEntry = doc.CreateElement("LogEntry");

            XmlElement timestampElement = doc.CreateElement("Timestamp");
            timestampElement.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            logEntry.AppendChild(timestampElement);

            XmlElement taskNameElement = doc.CreateElement("TaskName");
            taskNameElement.InnerText = taskName;
            logEntry.AppendChild(taskNameElement);

            XmlElement sourcePathElement = doc.CreateElement("SourcePath");
            sourcePathElement.InnerText = sourcePath;
            logEntry.AppendChild(sourcePathElement);

            XmlElement targetPathElement = doc.CreateElement("TargetPath");
            targetPathElement.InnerText = targetPath;
            logEntry.AppendChild(targetPathElement);

            XmlElement fileSizeElement = doc.CreateElement("FileSize");
            fileSizeElement.InnerText = fileSize.ToString();
            logEntry.AppendChild(fileSizeElement);

            XmlElement transferTimeElement = doc.CreateElement("TransferTime");
            transferTimeElement.InnerText = transferTime.ToString();
            logEntry.AppendChild(transferTimeElement);

            XmlElement EncryptionTimeElement = doc.CreateElement("EncryptionTime");
            EncryptionTimeElement.InnerText = EncryptionTime.ToString();
            logEntry.AppendChild(EncryptionTimeElement);

            // Ajouter l'élément logEntry au fichier XML
            doc.DocumentElement.AppendChild(logEntry);

            // Sauvegarder le fichier XML avec les nouvelles données
            doc.Save(logFileName);

            Console.WriteLine($"[LOG UPDATED] Log entry added to {logFileName}");
        }


        public override void DisplayLogFileContent()
        {
            string logFileName = Path.Combine(configManager.LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.xml");
            if (File.Exists(logFileName))
            {
                Console.WriteLine($"Contenu du log ({logFileName}):");
                Console.WriteLine(File.ReadAllText(logFileName));
            }
            else
            {
                Console.WriteLine("Log file not found.");
            }
        }
    }

}
