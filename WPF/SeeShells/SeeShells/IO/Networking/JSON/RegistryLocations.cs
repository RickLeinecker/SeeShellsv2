#region copyright
// SeeShells Copyright (c) 2019-2020 Aleksandar Stoyanov, Bridget Woodye, Klayton Killough, 
// Richard Leinecker, Sara Frackiewicz, Yara As-Saidi
// SeeShells is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// SeeShells is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with this program;
// if not, see <https://www.gnu.org/licenses>
#endregion
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.IO.Networking.JSON
{
    /// <summary>
    /// Holds a one-to-many resolution of an Operating System's Registry files and registry key path to Shellbags.
    /// </summary>
    public class RegistryLocations
    {

        /// <summary>
        /// Operating System 
        /// </summary>
        [JsonProperty(propertyName:"os", Required = Required.Always)]
        public string OperatingSystem { get; set; }

        [JsonProperty(propertyName:"files", Required = Required.Always)]
        public IList<string> RegistryFilePaths { private get; set; }


        public RegistryLocations(string operatingSystem, IList<string> registryFilePaths)
        {
            OperatingSystem = operatingSystem;
            RegistryFilePaths = registryFilePaths;
        }

        public RegistryLocations()
        {
        }

        /// <summary>
        /// Returns a Dictionary of pairings of Registry files and the registy paths for the shellbags in them.
        /// Example: ("NTUSER.DAT", ["p\a\t\h\1", "p\a\t\h\2"])
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IList<string>> GetRegistryFilePaths()
        {
            var retDict = new Dictionary<string, IList<string>>();
            foreach(string path in RegistryFilePaths)
            {
                string[] pathSplit = path.Split('\\');
                string regPath = string.Join("\\", pathSplit.AsEnumerable().Skip(1).ToArray());
                if (retDict.ContainsKey(pathSplit.First()))
                {
                    retDict[pathSplit.First()].Add(regPath);
                }
                else
                {
                    retDict.Add(pathSplit.First(), new List<string> { regPath });
                }
            }
            return retDict;
        }

    }
}
