using System.Text.Json;

/// <summary>
/// Represents a configuration manager that loads and saves the application configuration
/// </summary>
namespace BackupLogger
{
    public class ConfigManager : IConfigManager
    {
        private static readonly string ConfigFilePath = "C:\\EasySave\\Config\\config.json";

        public string LogDirectory { get; set; }
        public string LogFormat { get; set; }
        public ConfigManager()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                try
                {
                    string directoryPath = Path.GetDirectoryName(ConfigFilePath);

                    string json = File.ReadAllText(ConfigFilePath);

                    JsonDocument doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    LogDirectory = root.GetProperty("LogDirectory").GetString() ?? "C:\\EasySave\\logs";
                    LogFormat = root.GetProperty("LogFormat").GetString() ?? "JSON";

                    if (!Directory.Exists(LogDirectory))
                    {
                        Directory.CreateDirectory(LogDirectory);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERREUR] Impossible de charger la configuration: {ex.Message}");
                    SetDefaults();
                }
            }
            else
            {
                SetDefaults();
                SaveConfig();
            }
        }

        private void SetDefaults()
        {
            LogDirectory = "C:\\EasySave\\logs"; // default log directory value
            LogFormat = "JSON"; // default log format value

            Directory.CreateDirectory("C:\\EasySave\\Config");
            Directory.CreateDirectory("C:\\EasySave\\logs");
        }

        public void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR] Impossible de sauvegarder la configuration : {ex.Message}");
            }
        }

        public void SetLogDirectory(string directory)
        {
            LogDirectory = directory;
            SaveConfig();
        }

        public void SetLogFormat(string format)
        {
            LogFormat = format;
            SaveConfig();
        }
    }
}