using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using SeeShellsV2.Data;
using SeeShellsV2.Utilities;

namespace SeeShellsV2.Repositories
{
    public class ShellItemCollection : ObservableSortedList<IShellItem>, IShellItemCollection
    { }
}
