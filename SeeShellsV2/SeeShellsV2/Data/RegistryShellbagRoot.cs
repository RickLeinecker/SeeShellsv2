using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// The root of a shellbag tree from a registry hive
    /// </summary>
    public class RegistryShellbagRoot
    {
        /// <summary>
        /// Name of the source registry hive
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// absolute path to the registry hive
        /// </summary>
        public string Path { get; init; }

        /// <summary>
        /// Top level children of the shellbag root
        /// </summary>
        public ObservableCollection<IShellItem> Children { get; init; }

        public RegistryShellbagRoot(string name, string path)
        {
            Name = name;
            Path = path;
            Children = new ObservableCollection<IShellItem>();
        }
    }
}
