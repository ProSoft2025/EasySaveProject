using Avalonia.Controls;
using ReactiveUI;
using EasySaveV1; // Assurez-vous d'avoir la référence à votre projet EasySaveV1
using System;
using System.Collections.ObjectModel; // Pour ObservableCollection

namespace EasySaveV2
{
    public class ViewStatePageViewModel : ReactiveObject
    {
        private StateEntry _currentState;
        public StateEntry CurrentState
        {
            get => _currentState;
            set => this.RaiseAndSetIfChanged(ref _currentState, value);
        }

        public ObservableCollection<StateHistoryEntry> StateHistory { get; } = new();

        private bool _isInProgress;
        public bool IsInProgress
        {
            get => _isInProgress;
            set => this.RaiseAndSetIfChanged(ref _isInProgress, value);
        }

        private readonly StateManager _stateManager;

        public ViewStatePageViewModel(StateManager stateManager)
        {
            _stateManager = stateManager;

            // Exemple de mise à jour en temps réel (à adapter à votre logique)
            // Utilisez un timer ou un événement pour déclencher la mise à jour
            var timer = new System.Timers.Timer(1000); // Mise à jour toutes les secondes
            timer.Elapsed += (sender, e) => UpdateState();
            timer.Start();

            UpdateState(); // Mettre à jour l'état initial
        }

        private void UpdateState()
        {
            CurrentState = _stateManager.GetState();

            if (CurrentState != null)
            {
                IsInProgress = CurrentState.Status == "Running"; // Exemple : définir IsInProgress si le statut est "Running"

                // Ajouter l'état à l'historique
                StateHistory.Add(new StateHistoryEntry { State = CurrentState });
            }
        }
    }

    public class StateHistoryEntry
    {
        public StateEntry State { get; set; }

        public string FormattedState => $"{State.Timestamp}: {State.TaskName} - {State.Status} - Progress: {State.Progress}%";
    }

    public partial class ViewStatePage : UserControl
    {
        public ViewStatePage()
        {
            InitializeComponent();
            DataContext = new ViewStatePageViewModel(new StateManager("state.json")); // Assurez-vous du bon chemin
        }
    }
}