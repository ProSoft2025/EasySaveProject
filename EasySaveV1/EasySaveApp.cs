using System.Diagnostics;

namespace EasySaveV1
{
    public class EasySaveApp
    {
     

        public EasySaveApp()
        {
           
        }

        // Déclaration des variables
        private static EasySaveApp? _instance;

        private static readonly object _lock = new object();
        public List<BackupJob> BackupJobs { get; set; } = new List<BackupJob>();
        public List<string> ProcessesToMonitor { get; set; } = new List<string>();
        public List<string> ExtensionsToEncrypt { get; set; } = new List<string>();

        public static EasySaveApp GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new EasySaveApp();
                    }
                }
            }
            return _instance;
        }

        public int AddBackup(BackupJob job)
        {
            // Vérification nom unique
            foreach (var backup in BackupJobs)
            {
                if (backup.Name == job.Name)
                {
                    return 1;
                }
            }
            BackupJobs.Add(job);
            return 0;
        }

        public void RemoveBackup(string name)
        {
            var job = BackupJobs.FirstOrDefault(j => j.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (job != null)
            {
                BackupJobs.Remove(job);
               
            }
          
        }

        public void AddProcess(string process)
        {
            ProcessesToMonitor.Add(process);
        }

        public void RemoveProcess(string process)
        {
            if (ProcessesToMonitor.Contains(process))
                ProcessesToMonitor.Remove(process);
            else throw new Exception("Process not found");
        }

        public void MonitorProcesses()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var processName in ProcessesToMonitor)
                    {
                        var processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));
                        if (processes.Any())
                        {
                            PauseBackups();
                        }
                        else
                        {
                            ResumeBackups();
                        }
                    }
                    await Task.Delay(1000); 
                }
            });
        }

        private void PauseBackups()
        {
            foreach (var job in BackupJobs)
            {
                job.Pause();
            }
        }

        private void ResumeBackups()
        {
            foreach (var job in BackupJobs)
            {
                job.Resume();
            }
        }
        public void AddExtension(string extension)
        {
            ExtensionsToEncrypt.Add(extension);
        }
        public void RemoveExtension(string extension)
        {
            if (ExtensionsToEncrypt.Contains(extension))
                ExtensionsToEncrypt.Remove(extension);
            else throw new Exception("Extension not found");
        }
    }
}
