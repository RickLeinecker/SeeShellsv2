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
    /// C# object representation of an API Call which returned error information instead of it's intended information.
    /// </summary>
    public class APIError
    {
        [JsonProperty(propertyName:"success", Required = Required.Always)]
        public int Success { get; set; }
        [JsonProperty(propertyName:"error", Required = Required.Always)]
        public string Error { get; set; }

        public APIError(int success, string error)
        {
            Success = success;
            Error = error; ;
        }

        public APIError()
        {
        }
    }
}
