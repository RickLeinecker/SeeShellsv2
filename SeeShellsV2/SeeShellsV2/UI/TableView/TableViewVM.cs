using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using System.Text;
using System.Threading.Tasks;

using Unity;

using SeeShellsV2.Repositories;
using SeeShellsV2.Services;

namespace SeeShellsV2.UI
{
    public class TableViewVM : ViewModel, ITableViewVM
    {

        [Dependency]
        public IShellEventCollection ShellEvents { get; set; }

        [Dependency]
        public TemporaryShellEventGeneratorDontUseMe EventGenerator { get; set; }

        public TableViewVM() { }

        public void GenerateRandomShellEvents()
        {
            EventGenerator.Generate();
        }
    }
}
