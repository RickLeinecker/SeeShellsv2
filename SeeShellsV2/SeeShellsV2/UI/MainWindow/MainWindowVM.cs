using System;
using System.Windows;

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

        public async void ImportFromOnlineRegistry()
        {
            IRegistryImporter.Result t = await RegImporter.ImportOnlineRegistry(true);
            MessageBox.Show(string.Format("{0} shell items parsed, {1} shell items failed, {2} milliseconds.", t.parsed, t.failed, t.elapsedMilliseconds));
        }

        public async void ImportFromOfflineRegistry(string hiveLocation)
        {
            
            IRegistryImporter.Result t = await RegImporter.ImportOfflineRegistry(hiveLocation);
            MessageBox.Show(string.Format("{0} shell items parsed, {1} shell items failed, {2} milliseconds.", t.parsed, t.failed, t.elapsedMilliseconds));
        }
    }
}
