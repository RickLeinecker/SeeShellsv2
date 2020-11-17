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
using System.Runtime.InteropServices;
using System.Text;


namespace SeeShells.ShellParser.ShellItems
{
    enum SHITEMTYPE
    {
        UNKNOWN0 = 0x00,
        UNKNOWN1 = 0x01,
        UNKNOWN2 = 0x2E,
        FILE_ENTRY = 0x30,
        FOLDER_ENTRY = 0x1F,
        VOLUME_NAME = 0x20,
        NETWORK_LOCATION = 0x40,
        URI = 0x61,
        CONTROL_PANEL = 0x71,
        DELETEGATE_ITEM = 0x74
    };

    public class FILEENTRY_FRAGMENT : ShellItem
    {
        public uint filesize { get; protected set; }
        public ushort fileattrs { get; protected set; }
        public string short_name { get; protected set; }
        public override DateTime ModifiedDate { get; protected set; }
        public override string Name
        {
            get
            {
                return short_name;
            }
        }
        public FILEENTRY_FRAGMENT(byte[] buf, int offset, object parent, int filesize_offset)
            : base(buf, offset)
        {
            int off = filesize_offset;
            filesize = unpack_dword(off);
            off += 4;
            ModifiedDate = unpack_dosdate(off);
            off += 4;
            fileattrs = unpack_word(off);
            off += 2;
            short_name = unpack_string(off);
            off += short_name.Length + 1;
            off = align(off, 2);
        }
    }
}