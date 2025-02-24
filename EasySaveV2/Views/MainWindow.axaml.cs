using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using EasySaveV2.Localization;

namespace EasySaveV2.Views
{
    public partial class MainWindow : Window
    {
        private object _initialContent;

        public MainWindow()
        {
            InitializeComponent();

            var contentControl = this.FindControl<ContentControl>("MainContent");
            _initialContent = contentControl.Content;

            this.KeyDown += OnKeyDown;

            // Associer la gestion de la traduction
            this.DataContext = TranslationManager.Instance;
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            var contentControl = this.FindControl<ContentControl>("MainContent");

            if (contentControl != null)
            {
                contentControl.Content = new PageMenu();
            }
        }

        public void ResetToMainContent()
        {
            var contentControl = this.FindControl<ContentControl>("MainContent");
            if (contentControl != null)
            {
                contentControl.Content = _initialContent;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var startButton = this.FindControl<Button>("StartButton");
                if (startButton != null)
                {
                    startButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }

        // Ajout des méthodes de changement de langue
        private void SetLanguageEnglish(object sender, RoutedEventArgs e)
        {
            TranslationManager.Instance.SetLanguage("en-US");
        }

        private void SetLanguageFrench(object sender, RoutedEventArgs e)
        {
            TranslationManager.Instance.SetLanguage("fr-FR");
        }
    }
}
