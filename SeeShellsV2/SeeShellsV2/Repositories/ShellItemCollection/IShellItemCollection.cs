using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    public interface IShellItemCollection : ICollection<IShellItem>, INotifyPropertyChanged, INotifyCollectionChanged
    {

    }
}
