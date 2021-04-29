using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Unity;
using SeeShellsV2.Data;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{
    public class RegistryViewVM : ViewModel, IRegistryViewVM
    {
        [Dependency]
        public IShellItemCollection ShellItems { get; set; }

        [Dependency]
        public IUserCollection Users { get; set; }

        [Dependency]
        public ISelected Selected { get; set; }
    }
}
