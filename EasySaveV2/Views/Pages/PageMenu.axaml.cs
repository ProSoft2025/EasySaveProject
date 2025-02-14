
using Avalonia.Controls;
using Avalonia.Interactivity;
using BackupLogger;
using EasySaveV1;
using EasySaveV2.Views;
using static EasySaveV1.UserInterface;
using EasySaveV2.Views;
namespace EasySaveV2
{
    public partial class PageMenu : UserControl
    {
        private MenuManager menuManager;


        public PageMenu()
        {
            InitializeComponent();
            InitializeMenuManager();
        }
        private void InitializeMenuManager()
        {
            var ui = new UserInterface(new LanguageManager());
            menuManager = new MenuManager(ui, EasySaveApp.GetInstance(), new LoggerService("path/to/log/file"), new LanguageManager(), new StateManager("state.json"));
        }

        private void OnMenuSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedItem = (ListBoxItem)e.AddedItems[0];
                switch (selectedItem.Tag)
                {
                    case "Home":
                        ContentArea.Content = new HomePage();
                        break;

                    case "ViewBackup":
                        ContentArea.Content = new ViewBackupPage();
                        break;
                    case "AddBackup":
                        ContentArea.Content = new AddBackupPage(menuManager.GetBackupJobFactory(),
                            menuManager.GetStateManager(),
                            menuManager.GetEasySaveApp());
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
                    case "DailyLogs":
                        ContentArea.Content = new DailyLogsPage();
                        break;
                    case "ViewState":
                        //ContentArea.Content = new ViewStatePage();
                        break;


                }
            }
        }
    }
}
