using System.Collections.ObjectModel;

using SeeShellsV2.Data;

namespace SeeShellsV2.Repositories
{
    public class ShellCollection : ObservableCollection<IShellItem>, IShellCollection
    {
    }
}
