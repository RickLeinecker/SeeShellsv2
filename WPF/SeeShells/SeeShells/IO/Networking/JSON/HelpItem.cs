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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SeeShells.IO.Networking.JSON
{
    public class HelpItem
    {
        [JsonProperty("id", Required = Required.Always)]
        public long Id { private get; set; }

        [JsonProperty("title", Required = Required.Always)]
        public string Title { private get; set; }

        [JsonProperty("description", Required = Required.Always)]
        public string Description { private get; set; }

        public HelpItem()
        {
        }

        public string GetHelpContent()
        {
            return Description;
        }

    }
}
