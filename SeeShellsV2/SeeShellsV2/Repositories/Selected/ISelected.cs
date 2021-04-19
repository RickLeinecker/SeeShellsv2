using System;
using System.Collections.Generic;
using System.ComponentModel;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    /// <summary>
    /// Item selected by the user
    /// </summary>
    public interface ISelected : INotifyPropertyChanged
    {
        object Current { get; set; }
    }
}
