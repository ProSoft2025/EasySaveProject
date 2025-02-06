using EasySave;
using System.Text.Json;
using BackupLogger;

partial class Program
{
    public static void Main(string[] args)
    {
        var manager = EasySaveApp.GetInstance();
        var loggerService = new LoggerService("Logs.json");
        var ui = new UserInterface();

        // Create a StateManager instance
        var stateManager = new StateManager("state.json");

        IBackupStrategy complete = new CompleteBackup();
        var backupJob = new BackupJob("Save1", "/Source/Path", "/destination/path", complete, stateManager);
        manager.AddBackup(backupJob);

        loggerService.LogBackupCreation(backupJob);

        // Démarrer le menu
        var menu = new UserInterface.MenuManager(ui, manager, loggerService.GetBackupLogger());
        menu.Run();
    }
}
