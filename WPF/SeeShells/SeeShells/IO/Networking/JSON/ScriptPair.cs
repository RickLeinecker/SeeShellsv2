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
    public class ScriptPair
    {

        public int typeidentifier { get; set; }

        private string decodedScript;
        public string script { 
            get { return decodedScript; }
            set
            {
                decodedScript = Base64Decode(value);
            }
        }

        public ScriptPair() { }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }

    public class DecodedScriptPair
    {

        public int typeidentifier { get; set; }
        public string script { get; set; }

        public DecodedScriptPair() { }

        public KeyValuePair<int, string> getScript()
        {
            return new KeyValuePair<int, string>(typeidentifier, script);
        }
    }
}
