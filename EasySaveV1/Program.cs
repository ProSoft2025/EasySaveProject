using EasySave;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        IBackupStrategy complete = new CompleteBackup();
        var backupJobs1 = new BackupJob("Save1", "/Source/Path", "/destination/path", complete);

        var Manager1 = new EasySaveApp();
        Manager1.AddBackup(backupJobs1);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json_test = JsonSerializer.Serialize(backupJobs1, options);

        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string logFilePath = Path.Combine(projectDirectory,"Logs.json");


        File.AppendAllText(logFilePath, json_test + Environment.NewLine);

        var Logger_test = new Logger(logFilePath);

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
                    foreach (var Jobs in Manager1.BackupJobs)
                    {
                        Console.WriteLine(Jobs);
                    }
                    break;
                case '2':
                    BackupJobFactory backupJobFactory = new BackupJobFactory();

                    Console.Write("Entrez le nom du job : ");
                    string name = Console.ReadLine();

                    Console.Write("Entrez le répertoire source : ");
                    string sourceDirectory = Console.ReadLine();

                    Console.Write("Entrez le répertoire cible : ");
                    string targetDirectory = Console.ReadLine();

                    Console.Write("Choisissez le type de sauvegarde (1=Complète, 2=Différentielle) (default=1) : ");
                    string strategyChoice = Console.ReadLine();

                    IBackupStrategy strategy;

                    if (strategyChoice == "2")
                    {
                        strategy = new DifferentialBackup();
                    }
                    else
                    {
                        strategy = new CompleteBackup();
                    }

                    Manager1.AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy));
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
    }
}