// App
// Copyright (c) 2022 Arosy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Security.Principal;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Win32;

namespace WTU.MainApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Windows Telemetry Utility", "Due security reasons you have to run this application as\nadministrator in order to make changes to the windows\nregistry.");


            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);

                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    await messageBoxStandardWindow.Show();
                    Environment.Exit(-1);
                }
            }

            var explorerReg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer");
            var searchReg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Search");
            var telmReg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection");
            var updateReg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate");
            var vm = ViewModels.ViewModelLocator.Main;



            if (explorerReg.GetValue("DisableSearchBoxSuggestions") != null)
            {
                vm.EnableSearchBoxSuggestions = (int)explorerReg.GetValue("DisableSearchBoxSuggestions") == 0;
            }
            else
            { // create default
                explorerReg.SetValue("DisableSearchBoxSuggestions", 0, RegistryValueKind.DWord);
                vm.EnableSearchBoxSuggestions = true;
            }

            if (searchReg.GetValue("BingSearchEnabled") != null)
            {
                vm.BingSearchEnabled = (int)searchReg.GetValue("BingSearchEnabled") > 0;
            }
            else
            {
                searchReg.SetValue("BingSearchEnabled", 1, RegistryValueKind.DWord);
                vm.BingSearchEnabled = true;
            }

            if (searchReg.GetValue("CortanaConsent") != null)
            {
                vm.CortanaConsent = (int)searchReg.GetValue("CortanaConsent") > 0;
            }
            else
            {
                searchReg.SetValue("CortanaConsent", 1, RegistryValueKind.DWord);
                vm.CortanaConsent = true;
            }
            
            if (telmReg.GetValue("AllowTelemetry") != null)
            {
                vm.Telemetry = (TelemetryLevel)telmReg.GetValue("AllowTelemetry");
            }
            else
            {
                telmReg.SetValue("AllowTelemetry", 1, RegistryValueKind.DWord);
                vm.Telemetry = TelemetryLevel.Basic;
            }

            if (updateReg.GetValue("TargetReleaseVersion") != null && updateReg.GetValue("ProductVersion") != null && updateReg.GetValue("TargetReleaseVersionInfo") != null)
            {
                vm.BlockWindowsUpgrade = true;
            }


            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
