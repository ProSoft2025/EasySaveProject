using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackupLogger;

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
            private readonly LoggerService loggerService;
            private readonly BackupJobFactory backupJobFactory;
            private readonly LanguageManager languageManager;

            private readonly StateManager stateManager; // Add this line

            public MenuManager(UserInterface ui, EasySaveApp manager, LoggerService loggerService, LanguageManager languageManager,  StateManager stateManager)
            {
                this.ui = ui;
                this.manager = manager;
                this.loggerService = loggerService;
                this.backupJobFactory = new BackupJobFactory();
                this.languageManager = languageManager; // Passer languageManager
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
                            RestoreBackup(manager);
                            break;
                        case '5':
                            BackupExecute();
                            break;
                        case '6':
                            LogSubMenu();
                            break;
                        case '7':
                            ChangeLanguageMenu();
                            break;
                        case '8':
                            exit = true;
                            Console.WriteLine(languageManager.GetTranslation("exit_message"));
                            break;
                        default:
                            Console.WriteLine(languageManager.GetTranslation("invalid_choice"));
                            break;
                    }

                    if (!exit)
                    {
                        Console.WriteLine("\n" + languageManager.GetTranslation("press_any_key"));
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
                        loggerService.GetBackupLogger().DisplayLogFileContent();
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
                    Console.WriteLine(languageManager.GetTranslation("no_backups"));
                    return;
                }

                Console.WriteLine(languageManager.GetTranslation("list_of_backups"));
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

                var isErreur = manager.AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
                if (isErreur == 0)
                {
                    Console.WriteLine(languageManager.GetTranslation("backup_added"));
                }
                else if (isErreur == 1)
                {
                    Console.WriteLine(languageManager.GetTranslation("Error_BackupAdd"));

                }

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
                    if (manager.BackupJobs[i].Name.Equals(userChoice, StringComparison.OrdinalIgnoreCase))
                    {
                        string backupDirectory = manager.BackupJobs[i].TargetDirectory;
                        string restoreDirectory = manager.BackupJobs[i].SourceDirectory;

                        // Appel de la méthode de restauration via l'interface
                        manager.BackupJobs[i].Restore(loggerService);
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
                        Console.WriteLine($"Exécution de la sauvegarde {backupIndex + 1}: {manager.BackupJobs[backupIndex].Name}");
                        manager.BackupJobs[backupIndex].Execute(loggerService);
                    }
                }
                Console.WriteLine("Exécution de toutes les sauvegardes terminée.");
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


