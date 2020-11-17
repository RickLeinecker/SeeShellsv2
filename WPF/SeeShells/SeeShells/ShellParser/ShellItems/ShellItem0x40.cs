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

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Network Location Shell Item
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#35-network-location-shell-item
    public class ShellItem0x40 : ShellItem
    {
        public byte Flags { get; protected set; }
        public string Location { get; protected set; }
        public string Description { get; protected set; }
        public string Comments { get; protected set; }
        public override string TypeName { get => "Network Location"; }
        public override string Name => Location;
        public ShellItem0x40(byte[] buf) : base(buf)
        {
            Flags = unpack_byte(0x04);
            Location = unpack_string(0x05);
            int off = 0x05;
            off += Name.Length + 1;
            if ((Flags & 0x80) != 0)
            {
                Description = unpack_string(off);
                off += Description.Length + 1;
            }
            if ((Flags & 0x40) != 0)
            {
                Comments = unpack_string(off);
            }
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret =  base.GetAllProperties();
            AddPairIfNotNull(ret, Constants.FLAGS, Flags);
            AddPairIfNotNull(ret, Constants.LOCATION, Location);
            AddPairIfNotNull(ret, Constants.DESCRIPTION, Description);
            AddPairIfNotNull(ret, Constants.COMMENTS, Comments);
            return ret;
        }
    }
}