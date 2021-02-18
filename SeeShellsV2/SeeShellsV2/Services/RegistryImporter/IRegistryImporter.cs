using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Services
{
    public interface IRegistryImporter
    {
        public struct Result
        {
            public int parsed;
            public int failed;
            public long elapsedMilliseconds;

            public Result(int p, int f, long e)
            {
                parsed = p;
                failed = f;
                elapsedMilliseconds = e;
            }
        }

        /// <summary>
        /// Import shell items from the active Windows Registry on this machine
        /// </summary>
        /// <param name="parseAllUsers">set true to parse other user's offline hives</param>
        /// <returns>task that resolves to the number of shell items parsed</returns>
        public Task<Result> ImportOnlineRegistry(bool parseAllUsers = false);

        /// <summary>
        /// Import shell items from an offline registry hive
        /// </summary>
        /// <param name="registryFilePath">location of the hive</param>
        /// <returns>task that resolves to the number of shell items parsed</returns>
        public Task<Result> ImportOfflineRegistry(string registryFilePath);
    }
}
