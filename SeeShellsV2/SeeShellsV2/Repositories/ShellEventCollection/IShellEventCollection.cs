using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    public interface IShellEventCollection : IDataRepository<IShellEvent>
    {
        event FilterEventHandler Filter;
        ICollectionView FilteredView { get; }
    }
}
