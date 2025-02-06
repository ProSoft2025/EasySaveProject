﻿using System.Runtime.CompilerServices;
using System.Xml.Linq;

/* Summary */

namespace EasySave
{

    public class BackupJob
        {
            public string Name { get; set; }
            public string SourceDirectory { get; set; }
            public string TargetDirectory { get; set; }
            public StateManager StateManager { get; set; }
            public IBackupStrategy BackupStrategy { get; set; }

            public BackupJob(string name, string sourceDirectory, string targetDirectory, IBackupStrategy backupStrategy, StateManager stateManager)
            {
                this.Name = name;
                this.SourceDirectory = sourceDirectory;
                this.TargetDirectory = targetDirectory;
                this.BackupStrategy = backupStrategy;
                this.StateManager = stateManager;
            }

        public void Execute()
            {
                BackupStrategy.ExecuteBackup(SourceDirectory, TargetDirectory);


                string[] files = Directory.GetFiles(SourceDirectory);
                long totalSize = 0;
                foreach (string file in files)
                    totalSize += new FileInfo(file).Length;

                int totalFiles = files.Length;
                int filesProcessed = 0;
                long sizeProcessed = 0;

                foreach (string file in files)
                {
                    string destination = Path.Combine(TargetDirectory, Path.GetFileName(file));
                    File.Copy(file, destination, true);
                    filesProcessed++;
                    sizeProcessed += new FileInfo(file).Length;

                    StateEntry state = new StateEntry
                    {
                        TaskName = Name,
                        Timestamp = DateTime.Now,
                        Status = "Actif",
                        TotalFiles = totalFiles,
                        TotalSize = totalSize,
                        Progress = (int)((double)filesProcessed / totalFiles * 100),
                        RemainingFiles = totalFiles - filesProcessed,
                        RemainingSize = totalSize - sizeProcessed,
                        CurrentSource = file,
                        CurrentTarget = destination
                    };
                    StateManager.UpdateState(state);
                }

                StateManager.UpdateState(new StateEntry { TaskName = Name, Timestamp = DateTime.Now, Status = "Terminé" });
            }
        }
    }
