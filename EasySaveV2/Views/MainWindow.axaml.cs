using Avalonia.Controls;
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
            _initialContent = this.FindControl<ContentControl>("MainContent");
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
    }
}
