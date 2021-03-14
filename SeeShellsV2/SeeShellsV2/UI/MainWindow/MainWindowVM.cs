using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;
using SeeShellsV2.Services;

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

        public void ImportFromCSV(string path)
        {
            // importer.Import(path); => needs work
        }

        public void ExportToCSV(string path)
        {

        }

        public async void ImportFromRegistry(string hiveLocation = null)
        {
            (RegistryHive root, IEnumerable<IShellItem> parsedItems) = hiveLocation == null ? 
                await Task.Run(() => RegImporter.ImportRegistry(true)) :
                await Task.Run(() => RegImporter.ImportRegistry(false, true, hiveLocation));

            Selected.Current = root;
            Selected.CurrentEnumerable = root.Children;

            await Task.Run(() => ShellEventManager.GenerateEvents(parsedItems));
        }
    }
}
