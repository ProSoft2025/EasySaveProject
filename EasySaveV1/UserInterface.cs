using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackupLogger;

namespace EasySave
{
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

            private void BackupExecute()
            {
                Console.WriteLine("Exécution de toutes les sauvegardes...");
                manager.ExecuteAllBackupJobs();
            }
        }

    }
}