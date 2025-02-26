using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace EasySaveV2.Localization
{
    public class TranslationManager : INotifyPropertyChanged
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("EasySaveV2.Resources.Strings", typeof(TranslationManager).Assembly);

        private static TranslationManager _instance;
        public static TranslationManager Instance => _instance ??= new TranslationManager();

        private CultureInfo _currentCulture = CultureInfo.CurrentCulture;
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            private set
            {
                _currentCulture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public static TranslationManager Translation => Instance;

        // Ajout des propriétés utilisées dans le XAML
        public string AppTitle => this["AppTitle"];
        public string StartButton => this["StartButton"];
        public string VersionInfo => this["VersionInfo"];
        public string Copyright => this["Copyright"];
        public string TotalDataBackedUp => this["TotalDataBackedUp"];
        public string TotalBackupTime => this["TotalBackupTime"];
        public string BackToHomePage => this["BackToHomePage"];
        public string AddNewBackup => this["AddNewBackup"];
        public string JobName => this["JobName"];
        public string SourceDirectory => this["SourceDirectory"];
        public string TargetDirectory => this["TargetDirectory"];
        public string Browse => this["Browse"];
        public string BackupType => this["BackupType"];
        public string CompleteBackup => this["CompleteBackup"];
        public string DifferentialBackup => this["DifferentialBackup"];
        public string ExecuteBackup => this["ExecuteBackup"];
        public string EnterBackupNumbers => this["EnterBackupNumbers"];
        public string OutputPlaceholder => this["OutputPlaceholder"];
        public string Home => this["Home"];
        public string RemoveBackup => this["RemoveBackup"];
        public string RestoreBackup => this["RestoreBackup"];
        public string Encryption => this["Encryption"];
        public string ViewBackup => this["ViewBackup"];
        public string ViewLogs => this["ViewLogs"];
        public string JobProcess => this["JobProcess"];
        public string EncryptionExtensions => this["EncryptionExtensions"];
        public string EnterExtensionPlaceholder => this["EnterExtensionPlaceholder"];
        public string AddExtension => this["AddExtension"];
        public string RemoveExtension => this["RemoveExtension"];
        public string ExtensionErrorEmpty => this["ExtensionErrorEmpty"];
        public string ExtensionErrorDot => this["ExtensionErrorDot"];
        public string ExtensionErrorExists => this["ExtensionErrorExists"];
        public string ExtensionErrorNotFound => this["ExtensionErrorNotFound"];
        public string ExecuteBackup2 => this["ExecuteBackup"];
        public string EnterBackupNumbers2 => this["EnterBackupNumbers"];
        public string BackupError => this["BackupError"];
        public string BackupCompleted => this["BackupCompleted"];
        public string ProcessList => this["ProcessList"];
        public string EnterProcess => this["EnterProcess"];
        public string AddProcess => this["AddProcess"];
        public string DeleteProcess => this["DeleteProcess"];
        public string TextBoxNotFound => this["TextBoxNotFound"];
        public string ProcessInvalid => this["ProcessInvalid"];
        public string ProcessExists => this["ProcessExists"];
        public string RemoveBackupTitle => this["RemoveBackupTitle"];
        public string BackupName => this["BackupName"];
        public string BackupNameInvalid => this["BackupNameInvalid"];
        public string BackupDeleted => this["BackupDeleted"];
        public string BackupNotFound => this["BackupNotFound"];





        public string this[string key] => ResourceManager.GetString(key, _currentCulture) ?? key;

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetLanguage(string cultureCode)
        {
            CurrentCulture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = CurrentCulture;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
