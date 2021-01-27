using System.Collections.Generic;

using SeeShellsV2.Data;
using SeeShellsV2.Utilities;

namespace SeeShellsV2.Repositories
{
    public class ShellCollection : ObservableSortedList<IShellItem>, IShellCollection
    {
        public ShellCollection() : base(new ShellComparer()) { }
    }

    internal class ShellComparer : IComparer<IShellItem>
    {
        public int Compare(IShellItem a, IShellItem b)
        {
            return a.Type <= b.Type ? -1 : 1;
        }
    }
}
