using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    public interface BackupStrategy
    {
        void ExecuteBackup(string source, string target);
    }
}
