using EasySave;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
<<<<<<< Updated upstream
        var backupJobs1 = new BackupJobs("Save1", "/Source/Path", "/destination/path", BackupJobs.BackupType.Complete);
        backupJobs1.AfficherAttributs();

        var Manager1 = new EasySaveApp([backupJobs1]);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json_test = JsonSerializer.Serialize(backupJobs1, options);

        File.AppendAllText("Logs.json", json_test + Environment.NewLine);

        var Logger_test = new Logger("Test", DateTime.Now);

        // MENU
        static void AfficherMenu()
        {
            Console.WriteLine("===== Menu Sauvegarde =====");
            Console.WriteLine("1. Voir les sauvegardes");
            Console.WriteLine("2. Ajouter une sauvegarde");
            Console.WriteLine("3. Supprimer une sauvegarde");
            Console.WriteLine("4. Restaurer une sauvegarde");
            Console.WriteLine("5. Executer les sauvegardes");
            Console.WriteLine("6. Voir les logs");
            Console.WriteLine("7. Quitter");
            Console.WriteLine("============================");
            Console.Write("Votre choix : ");
        }
        Boolean quitter = false;
        while (!quitter) {
            AfficherMenu();
            ConsoleKeyInfo choix = Console.ReadKey();
            Console.Clear();
            switch (choix.KeyChar)
            {
                case '1':
                    Manager1.AfficherSauvegardes();
                    break;
                case '2':
                    break;
                case '3':
                    break;
                case '4':
                    break;
                case '5':
                    break;
                case '6':
                    Logger_test.DisplayLogFileContent();
                    break;
                case '7':
                    quitter = true;
                    Console.WriteLine("Fermeture de EasySoft.");
                    break;
                default:
                    Console.WriteLine("Choix non valide, veuillez réessayer.");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
            Console.Clear();
        }
=======
        var manager = EasySaveApp.GetInstance();
        var logger = new Logger("Logs.json");

        // Initialisation du gestionnaire de langues
        LanguageManager languageManager = new LanguageManager();

        // Initialisation de l'interface utilisateur avec gestion de la langue
        var ui = new UserInterface(languageManager);

        // Démarrer le menu
        var menu = new MenuManager(ui, manager, logger);
        menu.Run();
>>>>>>> Stashed changes
    }
}