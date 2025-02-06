using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackupLogger;

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
            private readonly StateManager stateManager; // Add this line

            public MenuManager(UserInterface ui, EasySaveApp manager, Logger logger, StateManager stateManager)
            {
                this.ui = ui;
                this.manager = manager;
                this.logger = logger;
                this.backupJobFactory = new BackupJobFactory();
                this.stateManager = stateManager; // Initialize stateManager
            }
            public void Run()
            {
                bool exit = false;

                while (!exit)
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
                            LogSubMenu();
                            break;
                        case '7':
                            exit = true;
                            Console.WriteLine("Fermeture de EasySave.");
                            break;
                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }

                    if (!exit)
                    {
                        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }

            private void LogSubMenu()
            {
                Console.WriteLine("===== Menu Logs =====");
                Console.WriteLine("1. Voir les logs journaliers");
                Console.WriteLine("2. Voir l'état en temps réel");
                Console.WriteLine("3. Retour au menu principal");
                Console.WriteLine("============================");
                Console.Write("Votre choix : ");

                ConsoleKeyInfo choixLog = Console.ReadKey();
                Console.Clear();

                switch (choixLog.KeyChar)
                {
                    case '1':
                        // Implémentation pour afficher les logs journaliers
                        Console.WriteLine("Affichage des logs journaliers");
                        logger.DisplayLogFileContent();
                        break;
                    case '2':
                        // Implémentation pour afficher l'état en temps réel
                        Console.WriteLine("Affichage de l'état en temps réel");
                        DisplayBackupState();
                        break;
                    case '3':
                        // Retour au menu principal
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
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
                    Console.WriteLine(job);
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

                IBackupStrategy strategy = strategyChoice == "2" ? new DifferentialBackup() : new CompleteBackup();

                manager.AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
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

            private void BackupExecute()
            {
                Console.WriteLine("Exécution de toutes les sauvegardes...");
                manager.ExecuteAllBackupJobs();
            }

            private void DisplayBackupState()
            {
                StateEntry state = stateManager.GetState();
                if (state == null)
                {
                    Console.WriteLine("No backup state available.");
                    return;
                }

                Console.WriteLine($"Backup Task: {state.TaskName}");
                Console.WriteLine($"Timestamp: {state.Timestamp}");
                Console.WriteLine($"Status: {state.Status}");
                Console.WriteLine($"Progress: {state.Progress}%");
                Console.WriteLine($"Total Files: {state.TotalFiles}");
                Console.WriteLine($"Remaining Files: {state.RemainingFiles}");
                Console.WriteLine($"Total Size: {state.TotalSize} bytes");
                Console.WriteLine($"Remaining Size: {state.RemainingSize} bytes");
                Console.WriteLine($"Current Source: {state.CurrentSource}");
                Console.WriteLine($"Current Target: {state.CurrentTarget}");
            }
        }
    }

   }


