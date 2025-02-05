using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave { 
    public class UserInterface
    {
        public void DisplayMenu()
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

        public string GetUserInput()
        {
            return Console.ReadLine();
        }

        public class MenuManager
        {
            private readonly UserInterface ui;
            private readonly EasySaveApp manager;
            private readonly Logger logger;
            private readonly BackupJobFactory backupJobFactory;

            public MenuManager(UserInterface ui, EasySaveApp manager, Logger logger)
            {
                this.ui = ui;
                this.manager = manager;
                this.logger = logger;
                this.backupJobFactory = new BackupJobFactory();
            }

            public void Run()
            {
                bool quitter = false;

                while (!quitter)
                {
                    ui.DisplayMenu();
                    ConsoleKeyInfo choix = Console.ReadKey();
                    Console.Clear();

                    switch (choix.KeyChar)
                    {
                        case '1':
                            DisplayBackup();
                            break;
                        case '2':
                            AddBackupMenu();
                            break;
                        case '3':
                            RemoveBackupMenu();
                            break;
                        case '4':
                            RestoreBackup();
                            break;
                        case '5':
                            BackupExecute();
                            break;
                        case '6':
                            logger.DisplayLogFileContent();
                            break;
                        case '7':
                            quitter = true;
                            Console.WriteLine("Fermeture de EasySave.");
                            break;
                        default:
                            Console.WriteLine("Choix non valide, veuillez réessayer.");
                            break;
                    }

                    if (!quitter)
                    {
                        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }

            private void DisplayBackup()
            {
                if (manager.BackupJobs.Count == 0)
                {
                    Console.WriteLine("Aucune sauvegarde enregistrée.");
                    return;
                }

                Console.WriteLine("Liste des sauvegardes :");
                foreach (var job in manager.BackupJobs)
                {
                    job.displayAttributs();
                    Console.Write("\n");
                }
            }

            private void AddBackupMenu()
            {
                Console.Write("Entrez le nom du job : ");
                string name = ui.GetUserInput();

                Console.Write("Entrez le répertoire source : ");
                string sourceDirectory = ui.GetUserInput();

                Console.Write("Entrez le répertoire cible : ");
                string targetDirectory = ui.GetUserInput();

                Console.Write("Choisissez le type de sauvegarde (1=Complète, 2=Différentielle) : ");
                string strategyChoice = ui.GetUserInput();

                IBackupStrategy strategy;
                switch (strategyChoice)
                {
                    case "1":
                        strategy = new CompleteBackup();
                        break;
                    case "2":
                        strategy = new DifferentialBackup();
                        break;
                    default:
                        Console.WriteLine("Choix Incorrect, veuillez recommencer");
                        return;
                }

                manager.AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy));
                Console.WriteLine("Sauvegarde ajoutée avec succès !");
            }

            private void RemoveBackupMenu()
            {
                Console.Write("Entrez le nom de la sauvegarde à supprimer : ");
                string name = ui.GetUserInput();
                manager.RemoveBackup(name);
                Console.WriteLine($"Sauvegarde '{name}' supprimée.");
            }

            private void RestoreBackup()
            {
                Console.WriteLine("Fonction de restauration non implémentée.");
            }

            public List<string> ReadBackupExecute(List<BackupJob> backupJobs)
            {
                Console.WriteLine("Saisir le numéro de Backup à éxécuter : \n" +
                "Exemple : '1-3' -> Exécuter les sauvegardes 1,2,3" +
                "\nExemple2 : '1;3;4' -> Exécute les sauvegardes 1 3 et 4");
                string userChoice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userChoice))
                {
                    Console.WriteLine("L'entrée est vide ou non valide.");
                    return new List<string>();
                }

                var backupsToRun = new List<int>();

                var segments = userChoice.Split(';');
                foreach (var segment in segments)
                {
                    var rangeParts = segment.Split('-');
                    if (rangeParts.Length == 2)
                    {
                        int start = int.Parse(rangeParts[0]);
                        int end = int.Parse(rangeParts[1]);
                        backupsToRun.AddRange(Enumerable.Range(start, end - start + 1));
                    }
                    else
                    {
                        backupsToRun.Add(int.Parse(segment));
                    }
                }

                var backupNamesToRun = new List<string>();
                foreach (var number in backupsToRun)
                {
                    if (number > 0 && number <= backupJobs.Count)
                    {
                        backupNamesToRun.Add(backupJobs[number - 1].name);
                    }
                    else
                    {
                        Console.WriteLine($"Le numéro de sauvegarde {number} est en dehors des limites.");
                    }
                }

                return backupNamesToRun;
            }
        

            private void BackupExecute()
            {
                List<string> backupToRun = ReadBackupExecute(manager.BackupJobs);
                for (int i = 0; i < backupToRun.Count(); i++)
                {
                    manager.BackupJobs[i].displayAttributs();
                }
                Console.WriteLine("Exécution de toutes les sauvegardes...");
            }
        }
    }
}
