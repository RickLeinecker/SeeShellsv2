using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using SeeShellsV2.Data;
using SeeShellsV2.Utilities;

namespace SeeShellsV2.Repositories
{
    public interface IShellItemCollection : ICollection<IShellItem>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        AsyncObservableCollection<RegistryHive> RegistryRoots { get; }
    }
}
