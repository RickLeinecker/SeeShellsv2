using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// The root of a shellbag tree from a registry hive
    /// </summary>
    public class RegistryHive
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
        /// User who owns the registry hive
        /// </summary>
        public User User { get; init; }

        /// <summary>
        /// Top level children of the shellbag root
        /// </summary>
        public AsyncObservableCollection<IShellItem> Children { get; init; }

        public RegistryHive()
        {
            Children = new AsyncObservableCollection<IShellItem>();
        }
    }
}
