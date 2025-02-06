using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

/* Summary */

namespace EasySave
{
    public class StateManager
    {
        public string StateFilePath { get; set; }

        public StateManager(string stateFilePath)
        {
            StateFilePath = stateFilePath;
        }

        public void UpdateState(StateEntry entry)
        {
            try
            {
                string json = JsonSerializer.Serialize(entry, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(StateFilePath, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur mise à jour état: {ex.Message}");
            }
        }

        public StateEntry GetState()
        {
            try
            {
                if (!File.Exists(StateFilePath)) return null;
                string json = File.ReadAllText(StateFilePath);
                return JsonSerializer.Deserialize<StateEntry>(json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur récupération état: {ex.Message}");
                return null;
            }
        }
    }
}