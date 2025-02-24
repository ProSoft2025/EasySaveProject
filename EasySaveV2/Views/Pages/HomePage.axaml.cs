using Avalonia.Controls;
using Avalonia.Interactivity;
using EasySaveV2.Localization;

namespace EasySaveV2.Views
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();

            // Lier le DataContext au TranslationManager
            DataContext = TranslationManager.Instance;
        }

        private void OnReturnToMainClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as MainWindow;
            mainWindow?.ResetToMainContent();
        }
    }
}
