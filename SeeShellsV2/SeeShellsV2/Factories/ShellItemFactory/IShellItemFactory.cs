using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using SeeShellsV2.Data;

namespace SeeShellsV2.Factories
{
    public interface IShellItemFactory
    {
        /// <summary>
        /// Get the type for the shell item of a given type id, if if exists
        /// </summary>
        /// <param name="type">shell item type identifier</param>
        /// <returns>shell item type if it exists, or null otherwise</returns>
        Type GetShellType(byte type);

        /// <summary>
        /// Create a new shell item from a byte array
        /// </summary>
        /// <param name="buf">the byte array containing shell item data</param>
        /// <returns>a new shell item instance if the buffer can be parsed or null otherwise</returns>
        IShellItem Create(byte[] buf);
    }
}
