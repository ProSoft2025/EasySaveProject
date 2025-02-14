using System;
using System.Collections.Generic;
/* SUMMARY 
 * It stores translations in a dictionary, using a key word linked to EN and FR */
namespace EasySaveV1
{

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
                { "french", "French" },
                { "english", "English" },
                { "backup_added", "Backup added successfully!" },
                { "backup_removed", "Backup removed:" },
                { "restore_not_implemented", "Restore function not implemented." },
                { "executing_backups", "Executing backups..." },
                { "enter_job_name", "Enter the job name:" },
                { "enter_source_directory", "Enter the source directory:" },
                { "enter_target_directory", "Enter the target directory:" },
                { "choose_backup_type", "Choose backup type (1=Complete, 2=Differential):" },
                { "enter_backup_name_to_remove", "Enter the backup name to remove :"},
                { "menu_log", "===== Logs Menu =====" },
                { "logs_format", "Choose Log format"},
                { "daily_logs", "See Daily Logs"},
                { "real_time", "See Logs in real time"},
                { "daily_logs2", "Display of daily logs"},
                { "real_time2", "Display of logs in real time"},
                { "menu_logs_format", "===== Logs Format Menu =====" },
                { "change_logs_format", "Do you want to change the Logs format ? (Y/N)" },
                { "current_logs_format", "The current Logs format is :" },
                { "format_xml_json", "Please enter new logs format (XML or JSON) :" },
                { "incorrect_xml_json", "Incorrect format. Please enter 'XML' or 'JSON'." },
                { "update_format", "Logs format has been updated to : " },
                { "backup_state_availability", "No backup state available." },
                { "backup_task", "Backup Task:" },
                { "time_stamp", "Timestamp:" },
                { "status","Status:" },
                { "progress", "Progress:" },
                { "total_files", "Total Files:" },
                { "remaining_files", "Remaining Files:" },
                { "total_size", "Total Size:" },
                { "remaining_size" , "Remaining Size:" },
                { "current_source" , "Current Source:" },
                { "current_target" , "Current Target: " },
                { "bytes", "Bytes" },
                { "execute_backup_finished", "Execution of every backup finished." },
                { "execute_backup", "Execution of the backup" },
                { "backup_number_incorrect", "Backup number incorrect :" },
                { "intervals_format_incorrect", "Incorrect intervals format :" },
                { "incorrect_interval", "Incorrect interval :" },
                { "invalid_input", "Input is empty or incorrect." },
                { "backup_number_execute", "Enter the Backup number to execute :" },
                { "1-3", "Exemple : '1-3' -> Execute the backups 1,2,3" },
                { "1, 2 and 3", "Exemple : '1-3;5' -> Execute the backups 1, 2, 3 and 5" },
                { "backup_name_incorrect", "Backup name incorrect." },
                { "backup_name_restore", "Enter backup name to restore : " },
                { "incorrect_choice_try_again", "Incorrect choice, please try again" },
                { "start_complete_backup", "Start of the complete backup." },
                { "copied", "Copied : " },
                { "to", " to " },
                { "complete_backup_finished", "The complete backup is finished." },
                { "complete_backup_error", "Error during the complete backup :" },
                { "restore_success", "Restoration of the files successful." },
                { "restore_error", "An error has occurrend during the restoration of the files :" },
                { "5_backup_rule", "There is already 5 backups, please delete at least one" },
                { "no_backup_found_name", "No backup found with the name" },
                { "save", " Backup" },
                { "successfull_delete", "successfully deleted." },
                { "start_diff_backup", "Start of the differential backup." },
                { "input_complete_backup_path", "Input the path of the last complete backup :" },
                { "diff_backup_finished", "The differential backup is finished" },
                { "error_diff_backup", "An error has occured during the differential backup :" },
                { "start_backup_restore", "Start the restore of the backup :" },
                { "encrypted", "has been encrypted" }


                { "Error_BackupAdd", "Error : The Backup Name already exists."}

            
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
                { "french", "Français" },
                { "english", "Anglais" },
                { "backup_added", "Sauvegarde ajoutée avec succès !" },
                { "backup_removed", "Sauvegarde supprimée :" },
                { "restore_not_implemented", "La fonction de restauration n'est pas implémentée." },
                { "executing_backups", "Exécution des sauvegardes..." },
                { "enter_job_name", "Entrez le nom du job :" },
                { "enter_source_directory", "Entrez le répertoire source :" },
                { "enter_target_directory", "Entrez le répertoire de destination :" },
                { "choose_backup_type", "Choisissez le type de sauvegarde (1=Complète, 2=Differentielles) :" },
                { "enter_backup_name_to_remove", "Entrez le nom de la sauvegarde à supprimer :"},
                { "menu_log", "===== Menu Logs =====" },
                { "logs_format", "Choisissez le format de log"},
                { "daily_logs", "Voir les logs journaliers"},
                { "real_time", "Voir les logs en temps réel"},
                { "daily_logs2", "Affichage des logs journaliers"},
                { "real_time2", "Affichage des logs en temps réel"},
                { "menu_logs_format", "===== Menu Format des logs =====" },
                { "change_logs_format", "Voulez-vous modifier le format des logs ? (O/N)" },
                { "current_logs_format", "Le format actuel des logs est :" },
                { "format_xml_json", "Veuillez entrer le nouveau format des logs (XML ou JSON) :" },
                { "incorrect_xml_json", "Format invalide. Veuillez entrer 'XML' ou 'JSON'." },
                { "update_format", "Le format des logs a été mis à jour en : " },
                { "backup_state_availability", "Pas d'état de sauvegarde disponible." },
                { "backup_task", "Tâche de sauvegarde:" },
                { "time_stamp", "Horodatage:" },
                { "status","Status:" },
                { "progress", "Progression:" },
                { "total_files", "Fichiers totaux:" },
                { "remaining_files", "Fichiers restants:" },
                { "total_size", "Taille total:" },
                { "remaining_size" , "Taille restante:" },
                { "current_source" , "Source actuelle:" },
                { "current_target" , "Cible actuelle: " },
                { "bytes", "Octets" },
                { "execute_backup_finished", "Exécution de toutes les sauvegardes terminée." },
                { "execute_backup", "Exécution de la sauvegarde" },
                { "backup_number_incorrect", "Numéro de sauvegarde invalide :" },
                { "intervals_format_incorrect", "Format d'intervalle incorrect :" },
                { "incorrect_interval", "Intervalle invalide :" },
                { "invalid_input", "L'entrée est vide ou non valide." },
                { "backup_number_execute", "Saisir le numéro de Backup à exécuter :" },
                { "1-3", "Exemple : '1-3' -> Exécuter les sauvegardes 1,2,3" },
                { "1, 2 and 3", "Exemple : '1-3;5' -> Exécute les sauvegardes 1, 2, 3 et 5" },
                { "backup_name_incorrect", "Nom de sauvegarde invalide." },
                { "backup_name_restore", "Entrez le nom de la sauvegarde à restaurer : " },
                { "incorrect_choice_try_again", "Choix Incorrect, veuillez recommencer" },
                { "start_complete_backup", "Début de la sauvegarde totale." },
                { "copied", "Copié : " },
                { "to", " vers " },
                { "complete_backup_finished", "La sauvegarde totale est terminée." },
                { "complete_backup_error", "Erreur lors de la sauvegarde totale :" },
                { "restore_success", "Restauration des fichiers effectuée avec succès." },
                { "restore_error", "Une erreur s'est produite lors de la restauration des fichiers :" },
                { "5_backup_rule", "Il y a déjà 5 sauvegardes, veuillez en supprimez une" },
                { "no_backup_found_name", "Aucune sauvegarde trouvée avec le nom" },
                { "save", "Sauvegarde" },
                { "successfull_delete", "supprimée avec succès." },
                { "start_diff_backup", "Début de la sauvegarde différentielle." },
                { "input_complete_backup_path", "Saisir le chemin de la dernière sauvegarde totale :" },
                { "diff_backup_finished", "La sauvegarde différentielle est terminée" },
                { "error_diff_backup", "Erreur lors de la sauvegarde différentielle :" },
                { "start_backup_restore", "Début de la restauration de la sauvegarde :" },
                { "encrypted", "a été chiffré" }

                { "Error_BackupAdd", "Erreur : Ce nom de sauvegarde existe déjà."}

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
}