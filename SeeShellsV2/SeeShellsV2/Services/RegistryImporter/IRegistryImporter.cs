using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Services
{
    public interface IRegistryImporter
    {
        /// <summary>
        /// Import shell items from the active Windows Registry on this machine
        /// </summary>
        /// <param name="parseAllUsers">set true to parse other user's offline hives</param>
        /// <returns>task that resolves to a tuple (num shell items parsed, num shell items failed to parse, elapsed milliseconds)</returns>
        public Task<(int, int, long)> ImportOnlineRegistry(bool parseAllUsers = false);

        /// <summary>
        /// Import shell items from an offline registry hive
        /// </summary>
        /// <param name="registryFilePath">location of the hive</param>
        /// <returns>task that resolves to a tuple (num shell items parsed, num shell items failed to parse, elapsed milliseconds)</returns>
        public Task<(int, int, long)> ImportOfflineRegistry(string registryFilePath);
    }
}
