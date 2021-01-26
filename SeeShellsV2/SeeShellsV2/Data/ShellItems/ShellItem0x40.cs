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
    /// Network Location Shell Item
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#35-network-location-shell-item
    public class ShellItem0x40 : ShellItem, IShellItem
    {
        public byte Flags
        {
            init => fields["Flags"] = value;
            get => fields.GetStructOrDefault<byte>("Flags", 0);
        }

        public string Location
        {
            init => fields["Location"] = value;
            get => fields.GetClassOrDefault("Location", string.Empty);
        }

        public string Description
        {
            init => fields["Description"] = value;
            get => fields.GetClassOrDefault("Description", string.Empty);
        }

        public string Comments
        {
            init => fields["Comments"] = value;
            get => fields.GetClassOrDefault("Comments", string.Empty);
        }

        public ShellItem0x40() { }

        public ShellItem0x40(byte[] buf) : base(buf)
        {
            fields["Flags"] = Block.unpack_byte(buf, 0x04);
            fields["Location"] = Block.unpack_string(buf, 0x05);

            int off = 0x05;
            off += Name.Length + 1;

            if ((Flags & 0x80) != 0)
            {
                fields["Description"] = Block.unpack_string(buf, off);
                off += Description.Length + 1;
            }

            if ((Flags & 0x40) != 0)
            {
                fields["Comments"] = Block.unpack_string(buf, off);
            }

            fields["TypeName"] = "Network Location";
            fields["Name"] = fields["Location"];
        }
    }
}