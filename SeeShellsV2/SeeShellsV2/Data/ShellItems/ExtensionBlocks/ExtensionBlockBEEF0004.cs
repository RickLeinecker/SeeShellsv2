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

namespace SeeShellsV2.Data
{
    public class ExtensionBlockBEEF0004 : ExtensionBlock
    {
        public DateTime CreationDate
        {
            get => fields["CreationDate"] as DateTime? ?? DateTime.MinValue;
        }

        public DateTime AccessedDate
        {
            get => fields["AccessedDate"] as DateTime? ?? DateTime.MinValue;
        }

        public ushort LongNameSize
        {
            get => fields["LongNameSize"] as ushort? ?? 0;
        }

        public string LongName
        {
            get => fields["LongName"] as string ?? string.Empty;
        }

        public string LocalizedName
        {
            get => fields["LocalizedName"] as string ?? string.Empty;
        }

        public ExtensionBlockBEEF0004(byte[] buf, int offset) : base(buf, offset)
        {
            int off = 0x08;  //pass all the known Extension Block Headers

            if (ExtensionVersion >= 0x03)
            {
                fields["CreationDate"] = Block.unpack_dosdate(buf, offset + off);
                off += 4;
                fields["AccessedDate"] = Block.unpack_dosdate(buf, offset + off);
                off += 4;
                off += 2; // unknown
            }

            if (ExtensionVersion >= 0x07)
            {
                off += 2;
                off += 8; // fileref
                off += 8; // unknown
            }

            if (ExtensionVersion >= 0x03)
            {
                fields["LongNameSize"] = Block.unpack_word(buf, offset + off);
                off += 2;
            }

            if (ExtensionVersion >= 0x09)
                off += 4; // unknown

            if (ExtensionVersion >= 0x08)
                off += 4; // unknown

            if (ExtensionVersion >= 0x03)
            {
                fields["LongName"] = Block.unpack_wstring(buf, offset + off);
                off += 2 * (LongName.Length + 1);
            }
            if (ExtensionVersion >= 0x03 && ExtensionVersion < 0x07 && LongNameSize > 0)
            {
                fields["LocalizedName"] = Block.unpack_string(buf, offset + off);
            }
            else if (ExtensionVersion >= 0x07 && LongNameSize > 0)
            {
                fields["LocalizedName"] = Block.unpack_wstring(buf, offset + off);
            }
        }
    }
}