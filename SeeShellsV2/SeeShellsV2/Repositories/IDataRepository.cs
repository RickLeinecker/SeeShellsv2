using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace SeeShellsV2.Repositories
{
    public interface IDataRepository<T> : ICollection<T>, INotifyPropertyChanged, INotifyCollectionChanged where T : IComparable<T>
    {
        SynchronizationContext SynchronizationContext { get; }
    }
}
