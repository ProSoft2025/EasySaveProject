using System.Text.Json;

///<summary>
/// Represents a configuration manager that loads and saves the application configuration

namespace BackupLogger
{
    public class ConfigManager : IConfigManager
    {
        private static readonly string ConfigFilePath = "C:\\EasySave\\Config\\config.json"; // Path to the configuration file

        public string LogDirectory { get; set; }
        public string LogFormat { get; set; }

        public ConfigManager()
        {
            LoadConfig(); // Load configuration when the manager is created
        }

        private void LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                try
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    JsonDocument doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    LogDirectory = root.GetProperty("LogDirectory").GetString() ?? "C:\\EasySave\\logs"; // Load log directory or use default
                    LogFormat = root.GetProperty("LogFormat").GetString() ?? "JSON"; // Load log format or use default

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
                SetDefaults(); // Set default if config file doesn't exist
                SaveConfig(); 
            }
        }

        private void SetDefaults()
        {
            LogDirectory = "C:\\EasySave\\logs";
            LogFormat = "JSON"; 

            // Ensure config  and log directory exists
            Directory.CreateDirectory("C:\\EasySave\\Config");
            Directory.CreateDirectory("C:\\EasySave\\logs");
            Directory.CreateDirectory("C:\\EasySave\\Sauvegardes");
        }

        public void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFilePath, json); // Write config to file
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR] Impossible de sauvegarder la configuration : {ex.Message}");
            }
        }

        // Update and save the log directory
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