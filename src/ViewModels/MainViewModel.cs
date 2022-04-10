using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace WTU.MainApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ICollection<TelemetryLevel> _telemetries;
        private TelemetryLevel _telemetry;
        private string _productVersion;
        private string _targetReleaseVersionInfo;
        private bool _enableSearchBoxSuggestions;
        private bool _bingSearchEnabled;
        private bool _cortanaConsent;
        private bool _blockWindowsUpgrade;
        private int _targetReleaseVersion;


        public string TargetReleaseVersionInfo
        {
            get
            {
                return _targetReleaseVersionInfo;
            }
            set
            {
                this.Set(ref _targetReleaseVersionInfo, value);
            }
        }
        public string ProductVersion
        {
            get
            {
                return _productVersion;
            }
            set
            {
                this.Set(ref _productVersion, value);
            }
        }

        public bool EnableSearchBoxSuggestions
        {
            get
            {
                return _enableSearchBoxSuggestions;
            }
            set
            {
                this.Set(ref _enableSearchBoxSuggestions, value);
            }
        }

        public bool BingSearchEnabled
        {
            get
            {
                return _bingSearchEnabled;
            }
            set
            {
                this.Set(ref _bingSearchEnabled, value);
            }
        }

        public bool CortanaConsent
        {
            get
            {
                return _cortanaConsent;
            }
            set
            {
                this.Set(ref _cortanaConsent, value);
            }
        }

        public bool BlockWindowsUpgrade
        {
            get
            {
                return _blockWindowsUpgrade;
            }
            set
            {
                this.Set(ref _blockWindowsUpgrade, value);
            }
        }

        public TelemetryLevel Telemetry
        {
            get
            {
                return _telemetry;
            }
            set
            {
                this.Set(ref _telemetry, value);
            }
        }

        public int TargetReleaseVersion
        {
            get
            {
                return _targetReleaseVersion;
            }
            set
            {
                this.Set(ref _targetReleaseVersion, value);
            }
        }

        public ICollection<TelemetryLevel> Telemetries
        {
            get
            {
                return _telemetries;
            }
        }

        public ICommand SaveChangesCommand
        {
            get;
            private set;
        }


        public MainViewModel()
        {
            _targetReleaseVersionInfo = "21H2";
            _telemetries = new List<TelemetryLevel>()
            {
                TelemetryLevel.Basic,
                TelemetryLevel.Enhanced,
                TelemetryLevel.Full
            };

            this.SaveChangesCommand = new RelayCommand(this.SaveChanges);
        }

        public async void SaveChanges()
        {
            var explorerReg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer");
            var updateReg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate");
            var searchReg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Search");
            var telmReg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection");



            explorerReg.SetValue("DisableSearchBoxSuggestions", this.EnableSearchBoxSuggestions ? 0 : 1, RegistryValueKind.DWord);
            searchReg.SetValue("BingSearchEnabled", this.BingSearchEnabled ? 1 : 0, RegistryValueKind.DWord);
            searchReg.SetValue("CortanaConsent", this.CortanaConsent ? 1 : 0, RegistryValueKind.DWord);
            telmReg.SetValue("AllowTelemetry", 1, RegistryValueKind.DWord);

            if (_blockWindowsUpgrade)
            {
                updateReg.SetValue("TargetReleaseVersion", 1, RegistryValueKind.DWord);
                updateReg.SetValue("ProductVersion", "Windows 10", RegistryValueKind.String);
                updateReg.SetValue("TargetReleaseVersionInfo", _targetReleaseVersionInfo, RegistryValueKind.String);
            }
            else
            {
                updateReg.DeleteValue("TargetReleaseVersion");
                updateReg.DeleteValue("ProductVersion");
                updateReg.DeleteValue("TargetReleaseVersionInfo");
            }

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Windows Telemetry Utility", "Your changes have been saved in the registry, it could be necessary\nthat you restart your computer for the changes to take effect.");
            
            
            await messageBoxStandardWindow.Show();
        }
    }
}
