using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Repositories
{
    /// <summary>
    /// A configuration object that stores registry importer settings. This object can be expanded to 
    /// support more settings as needed.
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// A list of registry paths that are known to contain username strings (usually via user directory paths)
        /// </summary>
        public IReadOnlyCollection<string> UsernameLocations { init; get; }

        /// <summary>
        /// A list of all known file system paths that contain offline user hive files (NTUSER.dat, UsrClass.dat)
        /// </summary>
        public IReadOnlyCollection<string> UserRegistryLocations { init; get; }

        /// <summary>
        /// A list of registry paths that are known to root shellbag trees
        /// </summary>
        public IReadOnlyCollection<string> ShellbagRootLocations { init; get; }

        /// <summary>
        /// A list of known GUIDs and their associated human-readable strings
        /// </summary>
        public IReadOnlyDictionary<string, string> KnownGuids { init; get; }
    }
}
