using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SeeShellsV2.Repositories
{
    public class Config : IConfig
    {
        [JsonProperty(propertyName: "KnownUsernameLocations", Required = Required.Always)]
        public IReadOnlyCollection<string> UsernameLocations { init; get; }

        [JsonProperty(propertyName: "KnownUserRegistryFileLocations", Required = Required.Always)]
        public IReadOnlyCollection<string> UserRegistryLocations { init; get; }

        [JsonProperty(propertyName: "KnownShellbagRootLocations", Required = Required.Always)]
        public IReadOnlyCollection<string> ShellbagRootLocations { init; get; }

        [JsonProperty(propertyName: "KnownGuids", Required = Required.Always)]
        public IReadOnlyDictionary<string, string> KnownGuids { init; get; }
    }
}
