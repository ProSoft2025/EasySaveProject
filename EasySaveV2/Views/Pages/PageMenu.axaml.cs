using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EasySaveV2
{
    public partial class PageMenu : UserControl
    {
        public PageMenu()
        {
            InitializeComponent();
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
                        ContentArea.Content = new AddBackupPage();
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
                        

                }
            }
        }
    }
}
