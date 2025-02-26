using EasySaveV1;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace EasySaveV2.Server
{
    public class ConsoleDistante
    {
        private Socket _serverSocket;
        public ConsoleDistante() 
        {
            StartServer();
        }

        private void StartServer()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 5212));
            _serverSocket.Listen(1);

            Thread serverThread = new Thread(ListenToClient);
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void ListenToClient()
        {
            while (true)
            {
                var clienSocket = _serverSocket.Accept();
                SendDataToClient(clienSocket);
                DisconnectClient(clienSocket);
            }
        }

        private void SendDataToClient(Socket clienSocket)
        {
            string BackupJobsListJson = JsonSerializer.Serialize(EasySaveApp.GetInstance().BackupJobs);
            byte[] data = Encoding.UTF8.GetBytes(BackupJobsListJson);
            clienSocket.Send(data);
        }

        private static void DisconnectClient(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
