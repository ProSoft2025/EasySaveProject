using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave {
    public class UserInterface
    {
    private readonly LanguageManager languageManager;
    public UserInterface(LanguageManager languageManager)
    {
        this.languageManager = languageManager;
    }
        public void DisplayMenu()
        {
            Console.WriteLine(languageManager.GetTranslation("menu_title"));
            Console.WriteLine("1. " + languageManager.GetTranslation("view_backups"));
            Console.WriteLine("2. " + languageManager.GetTranslation("add_backup"));
            Console.WriteLine("3. " + languageManager.GetTranslation("delete_backup"));
            Console.WriteLine("4. " + languageManager.GetTranslation("restore_backup"));
            Console.WriteLine("5. " + languageManager.GetTranslation("execute_backups"));
            Console.WriteLine("6. " + languageManager.GetTranslation("view_logs"));
            Console.WriteLine("7. " + languageManager.GetTranslation("change_language"));
            Console.WriteLine("8. " + languageManager.GetTranslation("exit"));
            Console.Write(languageManager.GetTranslation("your_choice") + " : ");
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
            private readonly LanguageManager languageManager;

            public MenuManager(UserInterface ui, EasySaveApp manager, Logger logger, LanguageManager languageManager)
            {
                this.ui = ui;
                this.manager = manager;
                this.logger = logger;
                this.backupJobFactory = new BackupJobFactory();
                this.languageManager = languageManager; // Passer languageManager
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
                            RestoreBackup(manager);
                            break;
                        case '5':
                            BackupExecute();
                            break;
                        case '6':
                            logger.DisplayLogFileContent();
                            break;
                        case '7':
                            ChangeLanguageMenu();
                            break;
                        case '8':
                            quitter = true;
                            Console.WriteLine(languageManager.GetTranslation("exit_message"));
                            break;
                        default:
                            Console.WriteLine(languageManager.GetTranslation("invalid_choice"));
                            break;
                    }

                    if (!quitter)
                    {
                        Console.WriteLine("\n" + languageManager.GetTranslation("press_any_key"));
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }

            private void DisplayBackup()
            {
                if (manager.BackupJobs.Count == 0)
                {
                    Console.WriteLine(languageManager.GetTranslation("no_backups"));
                    return;
                }

                Console.WriteLine(languageManager.GetTranslation("backup_list"));
                foreach (var job in manager.BackupJobs)
                {
                    job.displayAttributs();
                    Console.Write("\n");
                }
            }

            private void ChangeLanguageMenu()
            {
                Console.WriteLine(languageManager.GetTranslation("change_language"));
                Console.WriteLine("1. " + languageManager.GetTranslation("french"));
                Console.WriteLine("2. " + languageManager.GetTranslation("english"));

                string choice = ui.GetUserInput();

                if (choice == "1")
                    languageManager.SetLanguage("fr");
                else if (choice == "2")
                    languageManager.SetLanguage("en");
                else
                    Console.WriteLine(languageManager.GetTranslation("invalid_choice"));
            }

            private void AddBackupMenu()
            {
                Console.Write(languageManager.GetTranslation("enter_job_name"));
                string name = ui.GetUserInput();

                Console.Write(languageManager.GetTranslation("enter_source_directory"));
                string sourceDirectory = ui.GetUserInput();

                Console.Write(languageManager.GetTranslation("enter_target_directory"));
                string targetDirectory = ui.GetUserInput();

                Console.Write(languageManager.GetTranslation("choose_backup_type"));
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
                Console.WriteLine(languageManager.GetTranslation("backup_added"));
            }

            private void RemoveBackupMenu()
            {
                Console.Write(languageManager.GetTranslation("enter_backup_name_to_remove"));
                string name = ui.GetUserInput();
                manager.RemoveBackup(name);
                Console.WriteLine(string.Format(languageManager.GetTranslation("backup_removed"), name));
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
