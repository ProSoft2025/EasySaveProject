
/* Summary */

namespace EasySave
{
    public class StateEntry
    {
        public string TaskName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public int TotalFiles { get; set; }
        public long TotalSize { get; set; }
        public int Progress { get; set; }
        public int RemainingFiles { get; set; }
        public long RemainingSize { get; set; }
        public string CurrentSource { get; set; }
        public string CurrentTarget { get; set; }
    }
}
