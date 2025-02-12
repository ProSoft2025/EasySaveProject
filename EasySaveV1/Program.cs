using EasySave;
using BackupLogger;

partial class Program
{
    public static void Main(string[] args)
    {
        // Initialisation des menus 
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string filelog = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, $"{date}.json");
        var manager = EasySaveApp.GetInstance();
        var loggerService = new LoggerService(filelog);
        var langageManager = new LanguageManager();
        var ui = new UserInterface(langageManager);

        // Create a StateManager instance
        var stateManager = new StateManager("state.json");

        IBackupStrategy complete = new CompleteBackup();

        loggerService.LogBackupCreation(filelog);

        // Démarrer le menu
        var menu = new UserInterface.MenuManager(ui, manager, loggerService, langageManager,stateManager);
        menu.Run(); // Changed from menu.DisplayMenu() to menu.Run()
    }
}
