using System.Threading.Tasks;

using Avalonia.Controls;

namespace EasySaveV2.Services
{
    public class MessageService
    {
        public async Task ShowMessage(Window owner, string message)
        {
            var dialog = new Window
            {
                Content = new TextBlock { Text = message },
                Width = 300,
                Height = 100,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            await dialog.ShowDialog(owner);
        }
    }
}

