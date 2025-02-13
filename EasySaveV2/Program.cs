using System;
using System.IO;
using Avalonia;
using Avalonia.ReactiveUI;
using EasySaveV1;


namespace EasySaveV2
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();

        //string date = DateTime.Now.ToString("yyyy-MM-dd");
        //string filelog = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, $"{date}.json");
        //var manager = EasySaveApp.GetInstance();
        //var loggerService = new LoggerService(filelog);
        //var langageManager = new LanguageManager();
        //var ui = new UserInterface(langageManager);

        //// Create a StateManager instance
        //var stateManager = new StateManager("state.json");

        //IBackupStrategy complete = new CompleteBackup();

        //loggerService.LogBackupCreation(filelog);

        //// Démarrer le menu
        //var menu = new UserInterface.MenuManager(ui, manager, loggerService, langageManager, stateManager);
        //menu.Run(); // Changed from menu.DisplayMenu() to menu.Run()

    }
}
