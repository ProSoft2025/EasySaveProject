using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
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

            // Ajout de LanguageManager dans le constructeur
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
                            RestoreBackup();
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
                    Console.WriteLine(job);
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

                Console.WriteLine(languageManager.GetTranslation("language_selected"));
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

                IBackupStrategy strategy = strategyChoice == "2" ? new DifferentialBackup() : new CompleteBackup();

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

            private void RestoreBackup()
            {
                Console.WriteLine(languageManager.GetTranslation("restore_function_not_implemented"));
            }

            private void BackupExecute()
            {
                Console.WriteLine(languageManager.GetTranslation("executing_backups"));
                manager.ExecuteAllBackupJobs();
            }
        }
    }
}
