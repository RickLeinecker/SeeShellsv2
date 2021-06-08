using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.Services;

namespace SeeShellsV2.UI
{
    public class MainWindowVM : ViewModel, IMainWindowVM
    {
        [Dependency] public IRegistryImporter RegImporter { get; set; }
        [Dependency] public IShellEventManager ShellEventManager { get; set; }
        [Dependency] public ISelected Selected { get; set; }

        public string WebsiteUrl => @"https://rickleinecker.github.io/SeeShellsv2";
        public string GithubUrl => @"https://github.com/RickLeinecker/SeeShellsv2";

        public Visibility StatusVisibility => Status != string.Empty ? Visibility.Visible : Visibility.Collapsed;
        public string Status { get => _status; private set { _status = value; NotifyPropertyChanged(nameof(Status)); NotifyPropertyChanged(nameof(StatusVisibility)); } }
        private string _status = string.Empty;

        public void RestartApplication(bool runAsAdmin = false)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
            proc.StartInfo.UseShellExecute = true;

            if (runAsAdmin)
                proc.StartInfo.Verb = "runas";

            proc.Start();
            Application.Current.Shutdown();
        }

        public bool ImportFromRegistry(string hiveLocation = null)
        {
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if (isElevated || hiveLocation != null)
            {
                ImportFromRegistryInternal(hiveLocation);
                return true;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Active Registry Import requires administrator access. Do you want to restart the application as administrator (this will reset the application)?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    RestartApplication(true);
            }

            return false;
        }

        private async void ImportFromRegistryInternal(string hiveLocation = null)
        {
            Status = "Parsing Registry Hive...";
            (RegistryHive root, IEnumerable<IShellItem> parsedItems) = hiveLocation == null ?
                await Task.Run(() => RegImporter.ImportRegistry(true)) :
                await Task.Run(() => RegImporter.ImportRegistry(false, true, hiveLocation));

            if (root == null || parsedItems == null)
            {
                Status = "No New Shellbags Found.";
            }
            else
            {
                Selected.CurrentInspector = root;

                Status = "Generating User Action Events...";
                await Task.Run(() => ShellEventManager.GenerateEvents(parsedItems));
                Status = "Done.";
            }

            await Task.Run(() => Thread.Sleep(3000));
            Status = string.Empty;
        }
    }
}
