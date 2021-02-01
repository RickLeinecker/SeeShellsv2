using System;
using System.Collections.Generic;
using System.Text;

using Unity;

using SeeShellsV2.Repositories;
using SeeShellsV2.Services;

namespace SeeShellsV2.UI
{
    public class MainWindowVM : ViewModel, IMainWindowVM
    {
        [Dependency] public ICsvImporter importer { get; set; }

        public string Title { get { return "SeeShells"; } }

        public void ImportFromCSV(string path)
        {
            // importer.Import(path); => needs work
        }

        public void ExportToCSV(string path)
        {
        
        }
    }
}
