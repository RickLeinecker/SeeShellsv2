using System;
using System.Collections.Generic;

using SeeShellsV2.Data;
using SeeShellsV2.Utilities;

namespace SeeShellsV2.Repositories
{
    public class ShellEventCollection : ObservableSortedList<IShellEvent>, IShellEventCollection
    { }
}
