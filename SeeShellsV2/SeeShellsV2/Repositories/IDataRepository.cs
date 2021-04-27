using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace SeeShellsV2.Repositories
{
    /// <summary>
    /// A global data structure that is synchronized with the UI thread.
    /// Global instances of repositories are made available via the Unity dependency injection container found in <see cref="SeeShellsV2.Program.Main"/>.
    /// </summary>
    public interface IDataRepository<T> : ICollection<T>, INotifyPropertyChanged, INotifyCollectionChanged where T : IComparable<T>
    {
        SynchronizationContext SynchronizationContext { get; }
    }
}
