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

            private void RestoreBackup(EasySaveApp manager)
            {
                Console.WriteLine("Entrez le nom de la sauvegarde à restaurer : ");
                string userChoice = ui.GetUserInput();

                if (string.IsNullOrWhiteSpace(userChoice))
                {
                    Console.WriteLine("L'entrée est vide ou non valide.");
                    return;
                }

                bool found = false;
                for (int i = 0; i < manager.BackupJobs.Count; i++)
                {
                    if (manager.BackupJobs[i].name.Equals(userChoice, StringComparison.OrdinalIgnoreCase))
                    {
                        string backupDirectory = manager.BackupJobs[i].targetDirectory;
                        string restoreDirectory = manager.BackupJobs[i].sourceDirectory;

                        // Appel de la méthode de restauration via l'interface
                        manager.BackupJobs[i].BackupStrategy.Restore(backupDirectory, restoreDirectory);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Nom de sauvegarde invalide.");
                }
            }

            public List<int> ReadBackupExecute(List<BackupJob> backupJobs)
            {
                Console.WriteLine("Saisir le numéro de Backup à exécuter : \n" +
                "Exemple : '1-3' -> Exécuter les sauvegardes 1,2,3" +
                "\nExemple2 : '1;3;4' -> Exécute les sauvegardes 1, 3 et 4");
                string userChoice = ui.GetUserInput();

                if (string.IsNullOrWhiteSpace(userChoice))
                {
                    Console.WriteLine("L'entrée est vide ou non valide.");
                    return new List<int>();
                }

                var backupsToRun = new List<int>();

                var segments = userChoice.Split(';');
                foreach (var segment in segments)
                {
                    var rangeParts = segment.Split('-');
                    if (rangeParts.Length == 2)
                    {
                        if (int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                        {
                            if (start <= end && start > 0 && end <= backupJobs.Count)
                            {
                                backupsToRun.AddRange(Enumerable.Range(start, end - start + 1).Select(i => i - 1));
                            }
                            else
                            {
                                Console.WriteLine($"Intervalle invalide : {segment}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Format d'intervalle incorrect : {segment}");
                        }
                    }
                    else
                    {
                        if (int.TryParse(segment, out int singleBackup) && singleBackup > 0 && singleBackup <= backupJobs.Count)
                        {
                            backupsToRun.Add(singleBackup - 1);
                        }
                        else
                        {
                            Console.WriteLine($"Numéro de sauvegarde invalide : {segment}");
                        }
                    }
                }

                return backupsToRun;
            }

            private void BackupExecute()
            {
                List<int> backupsToRun = ReadBackupExecute(manager.BackupJobs);
                foreach (int backupIndex in backupsToRun)
                {
                    if (backupIndex >= 0 && backupIndex < manager.BackupJobs.Count)
                    {
                        Console.WriteLine($"Exécution de la sauvegarde {backupIndex + 1}: {manager.BackupJobs[backupIndex].name}");
                        manager.BackupJobs[backupIndex].Execute();
                    }
                }
                Console.WriteLine("Exécution de toutes les sauvegardes terminée.");
            }

        }
    }
}
