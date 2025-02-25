using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;
using EasySaveV2.Views;
using EasySaveV2.Services;
using BackupLogger;
using EasySaveV1;
using static EasySaveV1.UserInterface;

namespace EasySaveV2.Views
{
    public partial class PageMenu : UserControl
    {
        private MenuManager menuManager;
        private LanguageManager languageManager;
        private MessageService messageService;

        public PageMenu()
        {
            InitializeComponent(); // ⚠️ Vérifie que cette ligne est bien appelée
            DataContext = EasySaveV2.Localization.TranslationManager.Instance;

            InitializeMenuManager();

            // Sélectionne automatiquement "Home" au démarrage
            if (MenuListBox != null)
            {
                var homeItem = MenuListBox.Items.OfType<ListBoxItem>().FirstOrDefault(item => item.Tag.ToString() == "Home");
                if (homeItem != null)
                {
                    MenuListBox.SelectedItem = homeItem;
                }
            }
        }

        private void InitializeMenuManager()
        {
            var ui = new UserInterface(new LanguageManager());
            var config = new ConfigManager();
            messageService = new MessageService();
            menuManager = new MenuManager(ui, EasySaveApp.GetInstance(), config, new JSONLog(config), new LanguageManager(), new StateManager("state.json"));
        }

        private void OnMenuSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedItem = (ListBoxItem)e.AddedItems[0];
                if (selectedItem != null && ContentArea != null)
                {
                    switch (selectedItem.Tag)
                    {
                        case "Home":
                            ContentArea.Content = new HomePage();
                            break;
                        case "ViewBackup":
                            ContentArea.Content = new ViewBackupPage();
                            break;
                        case "AddBackup":
                            ContentArea.Content = new AddBackupPage(menuManager.GetBackupJobFactory(), menuManager.GetStateManager(), menuManager.GetEasySaveApp());
                            break;
                        case "RemoveBackup":
                            ContentArea.Content = new RemoveBackupPage();
                            break;
                        case "ExecuteBackup":
                            ContentArea.Content = new ExecuteBackupPage();
                            break;
                        case "RestoreBackup":
                            ContentArea.Content = new RestoreBackupPage();
                            break;
                        case "ViewLogs":
                            ContentArea.Content = new ViewLogsPage();
                            break;
                        case "Encryption":
                            ContentArea.Content = new EncryptionPage(menuManager.GetEasySaveApp(), messageService);
                            break;
                        case "Processus":
                            ContentArea.Content = new JobProcess(menuManager.GetEasySaveApp(), messageService);
                            break;
                    }
                }
            }
        }
    }
}
