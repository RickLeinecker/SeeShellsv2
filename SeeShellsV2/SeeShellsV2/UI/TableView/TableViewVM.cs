using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;

using SeeShellsV2.Data;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{
    public class TableViewVM : ViewModel, ITableViewVM
    {
        [Dependency]
        public IShellCollection ShellItems { get; set; }


    }
}
