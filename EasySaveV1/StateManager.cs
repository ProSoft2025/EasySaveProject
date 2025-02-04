using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    public class StateManager
    {
        public string StateFilePath { get; set; }

        public void UpdateState(StateEntry entry)
        {
            // Implémentation pour mettre à jour l'état dans un fichier
        }

        public string GetState()
        {
            // Implémentation pour récupérer l'état actuel
            return "";
        }
    }
}
