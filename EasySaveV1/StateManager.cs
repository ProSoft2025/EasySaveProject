using System.Text.Json;

/* SUMMARY
 * persisting and retrieving backup progress information to a JSON file.
 * updates it with current progress, and retrieves the latest state
*/

namespace EasySaveV1
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