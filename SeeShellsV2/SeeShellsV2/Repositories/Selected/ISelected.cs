using System;
using System.Collections.Generic;
using System.ComponentModel;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    /// <summary>
    /// A repository that stores selected objects. Selected objects can be set or displayed by any UI element.
    /// </summary>
    public interface ISelected : INotifyPropertyChanged
    {
        object CurrentInspector { get; set; }

        object CurrentData { get; set; }
    }
}
