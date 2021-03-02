using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using SeeShellsV2.Repositories;

namespace SeeShellsV2.UI
{
    public class InspectorViewVM : ViewModel, IInspectorViewVM
    {
        [Dependency]
        public ISelected Selected { get; set; }
    }
}
