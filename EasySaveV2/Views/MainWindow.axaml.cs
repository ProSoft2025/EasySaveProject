using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EasySaveV1;
using Microsoft.Extensions.DependencyInjection;
namespace EasySaveV2.Views
{
    public partial class MainWindow : Window
    {
        private readonly ServiceProvider _serviceProvider;
        public MainWindow()
        {
            
            InitializeComponent();
            
        }

     

        private void OnStartButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            var contentControl = this.FindControl<ContentControl>("MainContent");
            contentControl.Content = new PageMenu();
        }
    }
}
