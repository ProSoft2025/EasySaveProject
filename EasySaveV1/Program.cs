using EasySaveV1;
using BackupLogger;
using Microsoft.Extensions.DependencyInjection;


partial class Program
{
    public static void Main(string[] args)
    {
        // Initialisation des menus
        var languageManager = new LanguageManager();
        var manager = EasySaveApp.GetInstance(languageManager);
        var ui = new UserInterface(languageManager);

        // Create a StateManager instance
        var stateManager = new StateManager("C:\\EasySave\\state");
        //var stateManager = new StateManager("state.json");

        //<<<<<<<<<<<<Gestion du format log via DI>>>>>>>>>>>> 
        var services = new ServiceCollection();
        services.AddSingleton<IConfigManager, ConfigManager>();
        services.AddTransient<ILoggerStrategy>(provider =>
        {
            var config = provider.GetRequiredService<IConfigManager>();
            return config.LogFormat == "JSON" ? new JSONLog(config) : new XMLLog(config);
        });

        var provider = services.BuildServiceProvider();
        var config = provider.GetRequiredService<IConfigManager>();


        var logFormat = provider.GetRequiredService<ILoggerStrategy>();

        //<<<<<<<<<<<<Gestion du format log via DI>>>>>>>>>>>> 

        // Démarrer le menu
        var menu = new UserInterface.MenuManager(ui, manager, config, logFormat, languageManager, stateManager);
        menu.Run(); // Changed from menu.DisplayMenu() to menu.Run()
    }
}
