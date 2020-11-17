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
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.ShellParser.Scripting
{

    public static class ScriptHandler
    {
        /// <summary>
        /// A dictionary of key value pairs to store the scripts.
        /// The key is an int for the shell item identifier.
        /// The value is a string for the content of the script.
        /// </summary>
        public static Dictionary<int, string> scripts { get; set; }

        static ScriptHandler()
        {
            scripts = new Dictionary<int, string>();
        }

        public static bool HasScriptForShellItem(int identifier)
        {
            return scripts.ContainsKey(identifier);
        }

        public static IShellItem ParseShellItem(byte[] buf, int identifier)
        {
            scripts.TryGetValue(identifier, out string script);

            return new LuaShellItem(buf, identifier, script);

        }
    }
}