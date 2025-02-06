using System;

namespace EasySave
{
    public class UserInterface
    {
        public LanguageManager LanguageManager { get; }

        public UserInterface(LanguageManager languageManager)
        {
            this.LanguageManager = languageManager;
        }

        public void DisplayMenu()
        {
            Console.WriteLine(LanguageManager.GetTranslation("menu_title"));
            Console.WriteLine("1. " + LanguageManager.GetTranslation("view_backups"));
            Console.WriteLine("2. " + LanguageManager.GetTranslation("add_backup"));
            Console.WriteLine("3. " + LanguageManager.GetTranslation("delete_backup"));
            Console.WriteLine("4. " + LanguageManager.GetTranslation("restore_backup"));
            Console.WriteLine("5. " + LanguageManager.GetTranslation("execute_backups"));
            Console.WriteLine("6. " + LanguageManager.GetTranslation("view_logs"));
            Console.WriteLine("7. " + LanguageManager.GetTranslation("change_language"));
            Console.WriteLine("8. " + LanguageManager.GetTranslation("exit"));
            Console.Write(LanguageManager.GetTranslation("your_choice") + " : ");
        }

        public string GetUserInput()
        {
            return Console.ReadLine();
        }
    }

    public class MenuManager
    {
        private readonly UserInterface ui;
        private readonly EasySaveApp manager;
        private readonly Logger logger;
        private readonly BackupJobFactory backupJobFactory;
        private readonly LanguageManager languageManager;

        // Le LanguageManager est maintenant instancié correctement
        public MenuManager(UserInterface ui, EasySaveApp manager, Logger logger)
        {
            this.ui = ui;
            this.manager = manager;
            this.logger = logger;
            this.backupJobFactory = new BackupJobFactory();
            this.languageManager = ui.LanguageManager; // Utilisation du LanguageManager de l'UI
        }

        public void Run()
        {
            bool quitter = false;

            while (!quitter)
            {
                // Affichage du menu une seule fois par boucle
                ui.DisplayMenu();
                string choix = ui.GetUserInput();
                Console.Clear();

                switch (choix)
                {
                    case "1":
                        DisplayBackup();
                        break;
                    case "2":
                        AddBackupMenu();
                        break;
                    case "3":
                        RemoveBackupMenu();
                        break;
                    case "4":
                        RestoreBackup();
                        break;
                    case "5":
                        BackupExecute();
                        break;
                    case "6":
                        logger.DisplayLogFileContent();
                        break;
                    case "7":
                        ChangeLanguageMenu();
                        break;
                    case "8":
                        quitter = true;
                        Console.WriteLine(languageManager.GetTranslation("exit"));
                        break;
                    default:
                        Console.WriteLine(languageManager.GetTranslation("invalid_choice"));
                        break;
                }

                // Si l'utilisateur choisit de ne pas quitter, on attend une entrée avant de revenir au menu
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
                Console.WriteLine(languageManager.GetTranslation("no_backups"));
                return;
            }

            Console.WriteLine(languageManager.GetTranslation("list_of_backups"));
            foreach (var job in manager.BackupJobs)
            {
                Console.WriteLine(job);
            }
        }

        private void ChangeLanguageMenu()
        {
            Console.WriteLine(languageManager.GetTranslation("change_language"));
            Console.WriteLine("1. " + languageManager.GetTranslation("fr"));
            Console.WriteLine("2. " + languageManager.GetTranslation("en"));

            Console.WriteLine(languageManager.GetTranslation("choose_language"));
            string choice = ui.GetUserInput();

            if (choice == "1")
                languageManager.SetLanguage("fr");
            else if (choice == "2")
                languageManager.SetLanguage("en");
            else
                Console.WriteLine(languageManager.GetTranslation("invalid_choice"));

            // Affichage du menu à jour après avoir changé la langue
            Console.Clear();
            ui.DisplayMenu();
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
            Console.Write(languageManager.GetTranslation("enter_backup_name"));
            string name = ui.GetUserInput();
            manager.RemoveBackup(name);
            Console.WriteLine(languageManager.GetTranslation("backup_removed") + $" '{name}'");
        }

        private void RestoreBackup()
        {
            Console.WriteLine(languageManager.GetTranslation("restore_not_implemented"));
        }

        private void BackupExecute()
        {
            Console.WriteLine(languageManager.GetTranslation("executing_backups"));
            manager.ExecuteAllBackupJobs();
        }
    }
}
