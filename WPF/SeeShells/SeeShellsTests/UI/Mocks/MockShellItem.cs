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
using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI.Mocks
{
    class MockShellItem : IShellItem
    {
        readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        public MockShellItem(string name, byte type)
        {
            Name = name;
            Type = type;
        }
        public MockShellItem()
        {
            Name = "";
            Type = 0x99;
        }

        public ushort Size {get; set;}

        public byte Type {get; set;}

        public string TypeName {get; set;}

        public string Name {get; set;}

        public DateTime ModifiedDate {get; set;}

        public DateTime AccessedDate {get; set;}

        public DateTime CreationDate {get; set;}

        public IDictionary<string, string> GetAllProperties()
        {

            properties["Name"] = Name;
            properties["Type"] = Type.ToString("X2");
            properties["thing1"]  = "thing2";
            properties["thing3"]  = "thing4";
            properties["thing5"] = "thing6";
            return properties;
        }

        public void AddProperty(string key, string value)
        {
            properties.Add(key, value);
        }
    }
}
