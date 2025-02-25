using Avalonia.Threading;
using EasySaveV1;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

/// summary :
/// display the progress and status of the backup in the UI in getting the state from the state.json file

namespace EasySaveV2.ViewModels
{

    public class BackupViewModel : ViewModelBase
    {
        private int _progress;
        private string _status;
        private readonly string stateFilePath = "C:\\EasySave\\state.json";

        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public BackupViewModel()
        {
            StartMonitoringState();
        }

        private void StartMonitoringState()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000); // Vérification chaque seconde
                    LoadState();
                }
            });
        }

        private void LoadState()
        {
            if (File.Exists(stateFilePath))
            {
                try
                {
                    string json = File.ReadAllText(stateFilePath);
                    var state = JsonSerializer.Deserialize<StateEntry>(json);

                    if (state != null)
                    {
                        Dispatcher.UIThread.Post(() =>
                        {
                            Progress = state.Progress;
                            Status = state.Status;
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur de chargement de l'état: {ex.Message}");
                }
            }
        }
    }

}

