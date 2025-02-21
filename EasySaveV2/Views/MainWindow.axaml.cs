using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using EasySaveV1;
using Microsoft.Extensions.DependencyInjection;
namespace EasySaveV2.Views
{
    public partial class MainWindow : Window
    {
        private object _initialContent;
        private bool _isHomePage;
        public MainWindow()
        {

            InitializeComponent();
            var contentControl = this.FindControl<ContentControl>("MainContent");
            _initialContent = contentControl.Content;
            _isHomePage = true;
            this.KeyDown += OnKeyDown;
        }



        private void OnStartButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            var contentControl = this.FindControl<ContentControl>("MainContent");
            contentControl.Content = new PageMenu();
            _isHomePage = false;
        }
       

       public void ResetToMainContent()
        {
            var contentControl = this.FindControl<ContentControl>("MainContent");
            contentControl.Content = _initialContent;
            _isHomePage =true;  
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_isHomePage && e.Key == Key.Enter)
            {
                var startButton = this.FindControl<Button>("StartButton");
                startButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
