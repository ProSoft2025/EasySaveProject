using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EasySaveV2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Initialise avec le contenu par défaut si nécessaire
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnStartButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var contentControl = this.FindControl<ContentControl>("MainContent");
            contentControl.Content = new PageMenu();
        }
    }
}
