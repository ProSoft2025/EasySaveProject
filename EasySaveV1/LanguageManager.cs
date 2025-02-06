using System;
using System.Collections.Generic;

public class LanguageManager
{
    private string currentLanguage = "en"; // Langue par défaut

    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        { "en", new Dictionary<string, string>
            {
                { "menu_title", "Choose an option:" },
                { "view_backups", "View backups" },
                { "add_backup", "Add a backup" },
                { "delete_backup", "Delete a backup" },
                { "restore_backup", "Restore a backup" },
                { "execute_backups", "Execute backups" },
                { "view_logs", "View logs" },
                { "change_language", "Change language" },
                { "exit", "Exit" },
                { "exit_message", "Exit message" },
                { "your_choice", "Your choice" },
                { "language_selected", "Language changed successfully!" },
                { "no_backups", "No backups available." },
                { "list_of_backups", "List of backups:" },
                { "invalid_choice", "Invalid choice." },
                { "press_any_key", "Press any key to return to the menu" },
                { "choose_language", "Choose a language:" },
                { "fr", "French" },
                { "en", "English" },
                { "backup_added", "Backup added successfully!" },
                { "backup_removed", "Backup removed:" },
                { "restore_not_implemented", "Restore function not implemented." },
                { "executing_backups", "Executing backups..." },
                { "enter_job_name", "Enter the job name:" },
                { "enter_source_directory", "Enter the source directory:" },
                { "enter_target_directory", "Enter the target directory:" },
                { "choose_backup_type", "Choose backup type (1=Complete, 2=Differential):" }
            }
        },
        { "fr", new Dictionary<string, string>
            {
                { "menu_title", "Choisissez une option :" },
                { "view_backups", "Voir les sauvegardes" },
                { "add_backup", "Ajouter une sauvegarde" },
                { "delete_backup", "Supprimer une sauvegarde" },
                { "restore_backup", "Restaurer une sauvegarde" },
                { "execute_backups", "Exécuter les sauvegardes" },
                { "view_logs", "Voir les journaux" },
                { "change_language", "Changer de langue" },
                { "exit", "Quitter" },
                { "exit_message", "Message de sortie" },
                { "your_choice", "Votre choix" },
                { "language_selected", "Langue changée avec succès !" },
                { "no_backups", "Aucune sauvegarde disponible." },
                { "list_of_backups", "Liste des sauvegardes :" },
                { "invalid_choice", "Choix invalide." },
                { "press_any_key", "Appuyez sur n'importe quelle touche pour revenir au menu" },
                { "choose_language", "Choisissez une langue :" },
                { "fr", "Français" },
                { "en", "Anglais" },
                { "backup_added", "Sauvegarde ajoutée avec succès !" },
                { "backup_removed", "Sauvegarde supprimée :" },
                { "restore_not_implemented", "La fonction de restauration n'est pas implémentée." },
                { "executing_backups", "Exécution des sauvegardes..." },
                { "enter_job_name", "Entrez le nom du job :" },
                { "enter_source_directory", "Entrez le répertoire source :" },
                { "enter_target_directory", "Entrez le répertoire de destination :" },
                { "choose_backup_type", "Choisissez le type de sauvegarde (1=Complète, 2=Differentielles) :" }
            }
        }
    };

    public string GetTranslation(string key)
    {
        return translations[currentLanguage].GetValueOrDefault(key, key); // Retourne la traduction ou la clé si elle est absente
    }

    public void SetLanguage(string language)
    {
        if (translations.ContainsKey(language))
        {
            currentLanguage = language;
            Console.WriteLine(GetTranslation("language_selected"));
        }
        else
        {
            Console.WriteLine(GetTranslation("invalid_choice"));
        }
    }
}
