using EasySave;
using System.Text.Json;

partial class Program
{
    public static void Main(string[] args)
    {
        var manager = EasySaveApp.GetInstance();
        var logger = new Logger("Logs.json");

        IBackupStrategy complete = new CompleteBackup();
        var backupJob = new BackupJob("Save1", "/Source/Path", "/destination/path", complete);
        manager.AddBackup(backupJob);

        // Sérialisation JSON (peut être déplacée dans une méthode séparée si besoin)
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(backupJob, options);
        File.AppendAllText("Logs.json", json + Environment.NewLine);

        // Initialisation du gestionnaire de langues
        LanguageManager languageManager = new LanguageManager();

        // Initialisation de l'interface utilisateur avec gestion de la langue
        var ui = new UserInterface(languageManager);

        // Démarrer le menu
        var menu = new MenuManager(ui, manager, logger);
        menu.Run();
    }
}
