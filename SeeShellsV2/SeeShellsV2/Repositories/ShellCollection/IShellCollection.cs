using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    public interface IShellCollection : ICollection<IShellItem>, INotifyPropertyChanged, INotifyCollectionChanged
    {
    }
}
