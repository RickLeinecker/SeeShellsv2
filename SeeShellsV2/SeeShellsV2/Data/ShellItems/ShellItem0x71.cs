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
using System.Collections.Generic;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Control Panel Shell Item, with a constant GUID for identification.
    /// </summary>
    public class ShellItem0x71 : ShellItem, IShellItem
    {
        public string Guid
        {
            init => fields["Guid"] = value;
            get => fields.GetClassOrDefault("Guid", string.Empty);
        }

        public byte Flags
        {
            init => fields["Flags"] = value;
            get => fields.GetStructOrDefault<byte>("Flags", 0);
        }

        public ShellItem0x71() { }

        public ShellItem0x71(byte[] buf) : base(buf)
        {
            fields["Guid"] = Block.UnpackGuid(buf, 0x0E);
            fields["Flags"] = Block.UnpackByte(buf, 0x03);
            fields["TypeName"] = "Control Panel";

            if (KnownGuids.dict.ContainsKey(Guid))
                fields["Name"] = string.Format("{{CONTROL PANEL: {0}}}", KnownGuids.dict[Guid]);
            else
                fields["Name"] = string.Format("{{CONTROL PANEL: {0}}}", Guid);
        }
    }
}