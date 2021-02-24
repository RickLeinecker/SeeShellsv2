using System;
using System.Threading.Tasks;

using Unity;

using SeeShellsV2.Services;

namespace SeeShellsV2.UI
{
    public class MainWindowVM : ViewModel, IMainWindowVM
    {
        [Dependency] public ICsvImporter CsvImporter { get; set; }
        [Dependency] public IRegistryImporter RegImporter { get; set; }

        public string Title { get { return "SeeShells"; } }

        public void ImportFromCSV(string path)
        {
            // importer.Import(path); => needs work
        }

        public void ExportToCSV(string path)
        {

        }

        public async Task<(int, int, long)> ImportFromOnlineRegistry()
        {
            return await RegImporter.ImportOnlineRegistry(true);
        }

        public async Task<(int, int, long)> ImportFromOfflineRegistry(string hiveLocation)
        {
            
            return await RegImporter.ImportOfflineRegistry(hiveLocation);
        }
    }
}
