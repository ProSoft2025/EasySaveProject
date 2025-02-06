using EasySave;
using System.Text.Json;
using static EasySave.UserInterface;

class Program
{
    public static void Main(string[] args)
    {
        var manager = EasySaveApp.GetInstance();
        var logger = new Logger("Logs.json");
        var ui = new UserInterface();

        IBackupStrategy complete = new CompleteBackup();
        IBackupStrategy differentielle = new DifferentialBackup();
        var backupJob = new BackupJob("Save1", "/path/source", "/path/destBKP", differentielle);
        var backupJob1 = new BackupJob("Save2", "/path/source", "/path/destBKP", differentielle);
        var backupJob2 = new BackupJob("Save3", "/path/source", "/path/destBKP", complete);
        var backupJob3 = new BackupJob("Save4", "/path/source", "/path/destBKP", differentielle);

        manager.AddBackup(backupJob);
        manager.AddBackup(backupJob1);
        manager.AddBackup(backupJob2);
        manager.AddBackup(backupJob3);

        // Sérialisation JSON (peut être déplacée dans une méthode séparée si besoin)
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(backupJob, options);
        File.AppendAllText("Logs.json", json + Environment.NewLine);

        // Démarrer le menu
        var menu = new MenuManager(ui, manager, logger);
        menu.Run();
    }
}
