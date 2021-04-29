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
    /// <summary>
    /// A collection of IShellEvent objects. Services and ViewModels can get a reference to this collection
    /// by declaring an appropriate constructor or property dependency.
    /// </summary>
    public interface IShellEventCollection : IDataRepository<IShellEvent>
    {
        /// <summary>
        /// An event that is fired for each item in this collection when viewed using <see cref="FilteredView"/>.
        /// The handlers of this event decide whether a particular IShellEvent is visible or not.
        /// </summary>
        event FilterEventHandler Filter;

        /// <summary>
        /// A view of this collection that has been filtered by the <see cref="Filter"/> event handlers.
        /// </summary>
        ICollectionView FilteredView { get; }
    }
}
