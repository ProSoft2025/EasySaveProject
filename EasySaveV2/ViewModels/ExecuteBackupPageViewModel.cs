using EasySaveV2.Localization;
using EasySaveV2.ViewModels;

/// summary :
/// regroup in datacontext both TranslationManager and BackupViewModel for ExecuteBackupPage

namespace EasySaveV2.ViewModels
{
    public class ExecuteBackupPageViewModel
    {
        public TranslationManager Translation { get; } = TranslationManager.Instance;
        public BackupViewModel Backup { get; } = new BackupViewModel();
    }
}
