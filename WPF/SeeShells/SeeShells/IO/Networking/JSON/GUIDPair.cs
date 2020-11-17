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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.IO.Networking.JSON
{
    /// <summary>
    /// A GUID seen in Shellbags that has been correlated to a specific title when it appears in a Windows system.
    /// </summary>
    public class GUIDPair
    {

        [JsonProperty(propertyName: "guid", Required = Required.Always)]
        public string GUID { private get; set; }

        [JsonProperty(propertyName: "name", Required = Required.Always)]
        public string Name { private get; set; }
        public GUIDPair(string guid, string name)
        {
            GUID = guid;
            Name = name;
        }

        public GUIDPair()
        {
        }

        public KeyValuePair<string, string> getKnownGUID()
        {
            return new KeyValuePair<string, string>(GUID, Name);
        }
    }
}
