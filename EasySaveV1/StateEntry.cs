using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV1
{
    public class StateEntry
    {
        public string TaskName { get; set; }
        public string Timestamp { get; set; }
        public string Status { get; set; }
        public int TotalFiles { get; set; }
        public int TotalSize { get; set; }
        public int Progress { get; set; }
        public int RemainingFiles { get; set; }
        public int RemainingSize { get; set; }
        public string CurrentSource { get; set; }
        public string CurrentTarget { get; set; }
    }
}
