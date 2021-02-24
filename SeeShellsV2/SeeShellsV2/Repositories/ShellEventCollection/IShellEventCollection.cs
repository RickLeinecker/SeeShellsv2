using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    /// <summary>
    /// Collection of <see cref="IShellEvent"/> sorted by TimeStamp
    /// </summary>
    public interface IShellEventCollection : ICollection<IShellEvent>, INotifyPropertyChanged, INotifyCollectionChanged
    {

    }
}
