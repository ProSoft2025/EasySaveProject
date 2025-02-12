﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

/* Summary */

namespace EasySave
{
    // Manages the state file and updates backup progress
    public class StateManager
    {
        public string StateFilePath { get; set; } = string.Empty;


        public StateManager(string stateFilePath)
        {
            StateFilePath = stateFilePath;
            EnsureFileExists();
        }

        // Ensures the state file exists before writing
        private void EnsureFileExists()
        {
            if (!File.Exists(StateFilePath))
            {
                File.WriteAllText(StateFilePath, "{}");
            }
        }


        // Updates the state file with the current progress
        public void UpdateState(StateEntry entry)
        {
            try
            {
                string json = JsonSerializer.Serialize(entry, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(StateFilePath, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating state : {ex.Message}");
            }
        }

        // Retrieves the latest state from the file
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
                Console.Error.WriteLine($"Error retrieving state: {ex.Message}");
                return null;
            }
        }
    }
}