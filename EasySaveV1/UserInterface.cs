﻿using System;
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
            private readonly IConfigManager configManager;
            private readonly ILoggerStrategy loggerStrategy;
            private readonly BackupJobFactory backupJobFactory;
            private readonly LanguageManager languageManager;

            private readonly StateManager stateManager; // Add this line

            public MenuManager(UserInterface ui, EasySaveApp manager, IConfigManager configManager, ILoggerStrategy loggerStrategy , LanguageManager languageManager,  StateManager stateManager)
            {
                this.ui = ui;
                this.manager = manager;
                this.configManager = configManager;
                this.loggerStrategy = loggerStrategy;
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
                Console.WriteLine(languageManager.GetTranslation(("menu_log")));
                Console.WriteLine("1. " + languageManager.GetTranslation(("daily_logs")));
                Console.WriteLine("2. " + languageManager.GetTranslation(("real_time")));
                Console.WriteLine("3. " + languageManager.GetTranslation(("logs_format")));
                Console.WriteLine("4. " + languageManager.GetTranslation(("press_any_key")));
                Console.WriteLine("============================");
                Console.Write(languageManager.GetTranslation(("your_choice")));

                ConsoleKeyInfo choixLog = Console.ReadKey();
                Console.Clear();

                switch (choixLog.KeyChar)
                {
                    case '1':
                        // Implémentation pour afficher les logs journaliers
                        Console.WriteLine(languageManager.GetTranslation(("daily_logs2")));
                        loggerStrategy.DisplayLogFileContent();
                        break;
                    case '2':
                        // Implémentation pour afficher l'état en temps réel
                        Console.WriteLine(languageManager.GetTranslation(("real_time2")));
                        DisplayBackupState();
                        break;
                    case '3':
                        Console.Clear();
                        SetLogFormat();
                        break;
                    case '4':
                        // Retour au menu principal
                        break;
                    
                    default:
                        Console.WriteLine(languageManager.GetTranslation(("invalid_choice")));
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
                        strategy = new CompleteBackup(languageManager);
                        break;
                    case "2":
                        strategy = new DifferentialBackup(languageManager);
                        break;
                    default:
                        Console.WriteLine(languageManager.GetTranslation(("incorrect_choice_try_again")));
                        return;
                }

                manager.AddBackup(backupJobFactory.CreateBackupJob(name, sourceDirectory, targetDirectory, strategy, stateManager));
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
                Console.WriteLine(languageManager.GetTranslation(("backup_name_restore")));
                string userChoice = ui.GetUserInput();

                if (string.IsNullOrWhiteSpace(userChoice))
                {
                    Console.WriteLine(languageManager.GetTranslation(("invalid_input")));
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
                        manager.BackupJobs[i].Restore(loggerStrategy);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine(languageManager.GetTranslation(("backup_name_incorrect")));
                }
            }

            public List<int> ReadBackupExecute(List<BackupJob> backupJobs)
            {
                Console.WriteLine(languageManager.GetTranslation(("backup_number_execute")));
                Console.WriteLine(languageManager.GetTranslation(("1-3")));
                Console.WriteLine(languageManager.GetTranslation(("1, 2 and 3")));
                string userChoice = ui.GetUserInput();

                if (string.IsNullOrWhiteSpace(userChoice))
                {
                    Console.WriteLine(languageManager.GetTranslation(("invalid_input")));
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
                                Console.WriteLine(languageManager.GetTranslation(("incorrect_interval")) + $"{segment}");
                            }
                        }
                        else
                        {
                            Console.WriteLine(languageManager.GetTranslation((("intervals_format_incorrect")) + $" {segment}"));
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
                            Console.WriteLine(languageManager.GetTranslation(("backup_number_incorrect")) +$" {segment}");
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
                        Console.WriteLine((languageManager.GetTranslation("execute_backup")) + $"{backupIndex + 1}: {manager.BackupJobs[backupIndex].Name}");
                        manager.BackupJobs[backupIndex].Execute(loggerStrategy);
                    }
                }
                Console.WriteLine((languageManager.GetTranslation("execute_backup_finished")));
            }

            private void DisplayBackupState()
            {
                StateEntry state = stateManager.GetState();
                if (state == null)
                {
                    Console.WriteLine(languageManager.GetTranslation(("backup_state_availability")));
                    return;
                }

                Console.WriteLine((languageManager.GetTranslation("backup_task")) + $"{state.TaskName}");
                Console.WriteLine((languageManager.GetTranslation("time_stamp")) + $"{state.Timestamp}");
                Console.WriteLine((languageManager.GetTranslation("status")) + $" {state.Status}");
                Console.WriteLine((languageManager.GetTranslation("progress")) + $" {state.Progress}%");
                Console.WriteLine((languageManager.GetTranslation("total_files")) + $" {state.TotalFiles}");
                Console.WriteLine((languageManager.GetTranslation("remaining_files")) + $" {state.RemainingFiles}");
                Console.WriteLine((languageManager.GetTranslation("total_size")) + $" {state.TotalSize} " + (languageManager.GetTranslation("bytes")));
                Console.WriteLine((languageManager.GetTranslation("remaining_size")) + $" {state.RemainingSize} " + (languageManager.GetTranslation("bytes")));
                Console.WriteLine((languageManager.GetTranslation("current_source")) + $" {state.CurrentSource}");
                Console.WriteLine((languageManager.GetTranslation("current_target")) + $"{state.CurrentTarget}");
            }

            private void SetLogFormat()
            {
                // Afficher le format actuel des logs
                Console.WriteLine(languageManager.GetTranslation(("menu_logs_format")));
                Console.WriteLine(languageManager.GetTranslation(("current_logs_format")) + configManager.LogFormat);

                // Demander à l'utilisateur s'il veut modifier le format
                Console.WriteLine(languageManager.GetTranslation(("change_logs_format")));
                string choice = Console.ReadLine()?.ToUpper();

                if (choice == "O" || choice =="Y")
                {
                    string newFormat = "";
                    bool validFormat = false;

                    while (!validFormat)
                    {
                        Console.WriteLine(languageManager.GetTranslation(("format_xml_json")));
                        newFormat = Console.ReadLine()?.ToUpper();

                        if (newFormat == "XML" || newFormat == "JSON")
                        {
                            validFormat = true;
                        }
                        else
                        {
                            Console.WriteLine(languageManager.GetTranslation(("incorrect_xml_json")));
                        }
                    }

                    // Mettre à jour le format des logs dans le ConfigManager
                    configManager.SetLogFormat(newFormat);
                    Console.WriteLine(languageManager.GetTranslation(("update_format")) + newFormat);
                }
                else
                {
                    // Si l'utilisateur ne veut pas modifier, retourner au menu principal
                    Console.WriteLine(languageManager.GetTranslation(("press_any_key")));
                }
            }

        }
    }

 }


