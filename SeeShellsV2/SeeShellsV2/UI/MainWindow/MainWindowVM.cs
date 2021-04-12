using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.Services;
using System.Threading;
using System.Windows;

namespace SeeShellsV2.UI
{
    public class MainWindowVM : ViewModel, IMainWindowVM
    {
        [Dependency] public ICsvImporter CsvImporter { get; set; }
        [Dependency] public IRegistryImporter RegImporter { get; set; }
        [Dependency] public IShellEventManager ShellEventManager { get; set; }
        [Dependency] public ISelected Selected { get; set; }

        public string WebsiteUrl => @"https://shellbags.github.io/v2";
        public string GithubUrl => @"https://github.com/ShellBags/v2";

        public Visibility StatusVisibility => Status != string.Empty ? Visibility.Visible : Visibility.Collapsed;
        public string Status { get => _status; private set { _status = value; NotifyPropertyChanged(nameof(Status)); NotifyPropertyChanged(nameof(StatusVisibility)); } }
        private string _status = string.Empty;

        public void ImportFromCSV(string path)
        {
            // importer.Import(path); => needs work
        }

        public void ExportToCSV(string path)
        {

        }

        public async void ImportFromRegistry(string hiveLocation = null)
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

                Selected.Current = root;
                // Selected.CurrentEnumerable = root.Items;

                Status = "Generating User Action Events...";
                await Task.Run(() => ShellEventManager.GenerateEvents(parsedItems));
                Status = "Done.";
            }

            await Task.Run(() => Thread.Sleep(3000));
            Status = string.Empty;
        }
    }
}
