using System;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Platform;

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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://EasySaveV2/Assets/logo.ico")))
            };

            dialog.KeyDown += (sender, e) =>
            {
                dialog.Close();
            };

            await dialog.ShowDialog(owner);
        }
    }
}

