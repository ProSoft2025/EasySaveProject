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
        public MainWindow()
        {

            InitializeComponent();
            var contentControl = this.FindControl<ContentControl>("MainContent");
            _initialContent = contentControl.Content;
            this.KeyDown += OnKeyDown;
        }



        private void OnStartButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            var contentControl = this.FindControl<ContentControl>("MainContent");
            contentControl.Content = new PageMenu();
        }
       

       public void ResetToMainContent()
        {
            var contentControl = this.FindControl<ContentControl>("MainContent");
            contentControl.Content = _initialContent;
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var startButton = this.FindControl<Button>("StartButton");
                startButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
