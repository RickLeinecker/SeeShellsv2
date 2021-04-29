using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using SeeShellsV2.Data;

namespace SeeShellsV2.Factories
{
    /// <summary>
    /// An object that constructs IShellItem objects from shellbag byte strings
    /// </summary>
    public interface IShellItemFactory
    {
        /// <summary>
        /// Get the type for the shell item of a given shellbag, if if exists
        /// </summary>
        /// <returns>shell item type if it exists, or null otherwise</returns>
        Type GetShellType(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null);

        /// <summary>
        /// Create a new shell item from a byte string
        /// </summary>
        /// <returns>a new shell item instance if the buffer can be parsed or null otherwise</returns>
        IShellItem Create(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null);
    }
}
