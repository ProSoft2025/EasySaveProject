using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleDistante
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnSendButtonClick(object sender, RoutedEventArgs e)
        {
            // Réinitialiser le message
            this.FindControl<TextBlock>("MessageTextBlock").Text = "";

            // Récupérer les valeurs des TextBox
            var ipAddressText = this.FindControl<TextBox>("IPAddessTextBox").Text;
            var portText = this.FindControl<TextBox>("PortTextBox").Text;

            // Validation de l'adresse IP et du port
            bool isValidIp = IPAddress.TryParse(ipAddressText, out IPAddress serverIP);
            bool isValidPort = int.TryParse(portText, out int serverPort) && serverPort > 1024 && serverPort < 65535;

            if (isValidIp && isValidPort)
            {
                // Tenter de se connecter au serveur
                var clientSocket = await ConnectToServerAsync(serverIP, serverPort);
                if (clientSocket != null)
                {
                    this.FindControl<TextBlock>("MessageTextBlock").Text = "Connexion réussie.";

                    // Réception des données du serveur
                    var buffer = new byte[1024];
                    var bytesRead = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                    var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);


                    var backupJobList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
                    this.FindControl<TextBlock>("MessageTextBlock").Text = "Données reçues.";

                    // Ouvrir la fenêtre RemoteBackupsWindow et passer les données
                    var remoteBackupsWindow = new RemoteBackups();
                    remoteBackupsWindow.UpdateBackupJobList(backupJobList);
                    remoteBackupsWindow.ShowDialog(this);

                    clientSocket.Close();


                }
                else
                {
                    this.FindControl<TextBlock>("MessageTextBlock").Text = "La connexion au serveur a échoué.";
                }
            }
            else
            {
                string errorMessage = !isValidIp ? "L'adresse IP n'est pas valide." : "Le port doit être compris entre 1024 et 65535.";
                this.FindControl<TextBlock>("MessageTextBlock").Text = errorMessage;
            }
        }

        private static async Task<Socket> ConnectToServerAsync(IPAddress serverIP, int serverPort)
        {
            try
            {
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await clientSocket.ConnectAsync(new IPEndPoint(serverIP, serverPort));
                return clientSocket;
            }
            catch { return null; }
        }
    }
}
