using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Data;

namespace SeeShellsV2.Services
{
    /// <summary>
    /// A service that imports registry hives as collections of <see cref="IShellItem"/> objects.
    /// </summary>
    public interface IRegistryImporter
    {
        public event EventHandler RegistryImportBegin;
        public event EventHandler RegistryImportEnd;

        /// <summary>
        /// Import shell items from the active Windows Registry on this machine
        /// </summary>
        /// <param name="parseAllUsers">set true to parse other user's offline hives</param>
        /// <returns>task that resolves to a tuple (num shell items parsed, num shell items failed to parse, elapsed milliseconds)</returns>
        public (RegistryHive, IEnumerable<IShellItem>) ImportRegistry(bool parseAllUsers = false, bool useOfflineHive = false, string hiveLocation = null);
    }
}
