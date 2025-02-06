using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    public class StateEntry
    {
        public string TaskName { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int TotalFiles { get; set; }
        public int TotalSize { get; set; }
        public int Progress { get; set; }
        public int RemainingFiles { get; set; }
        public int RemainingSize { get; set; }
        public string CurrentSource { get; set; } = string.Empty;
        public string CurrentTarget { get; set; } = string.Empty;


    }
}
