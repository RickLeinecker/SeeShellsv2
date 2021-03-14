using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Repositories
{
    public interface IConfig
    {
        public IReadOnlyCollection<string> UsernameLocations { init; get; }

        public IReadOnlyCollection<string> UserRegistryLocations { init; get; }

        public IReadOnlyCollection<string> ShellbagRootLocations { init; get; }

        public IReadOnlyDictionary<string, string> KnownGuids { init; get; }
    }
}
