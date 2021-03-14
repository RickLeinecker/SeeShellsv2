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

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class ExtensionBlockBEEF0004 : ExtensionBlock
    {
        public DateTime CreationDate
        {
            init => fields["CreationDate"] = value;
            get => fields.GetStructOrDefault("CreationDate", DateTime.MinValue);
        }

        public DateTime AccessedDate
        {
            init => fields["AccessedDate"] = value;
            get => fields.GetStructOrDefault("AccessedDate", DateTime.MinValue);
        }

        public string LongName
        {
            init => fields["LongName"] = value;
            get => fields.GetClassOrDefault("LongName", string.Empty);
        }

        public string LocalizedName
        {
            init => fields["LocalizedName"] = value;
            get => fields.GetClassOrDefault("LocalizedName", string.Empty);
        }

        public ExtensionBlockBEEF0004() { }

        public ExtensionBlockBEEF0004(byte[] buf, int offset) : base(buf, offset)
        {
            int off = 0x08;  //pass all the known Extension Block Headers

            if (ExtensionVersion >= 0x03)
            {
                fields["CreationDate"] = BlockHelper.UnpackDosDateTime(buf, offset + off);
                off += 4;
                fields["AccessedDate"] = BlockHelper.UnpackDosDateTime(buf, offset + off);
                off += 4;
                off += 2; // unknown
            }

            if (ExtensionVersion >= 0x07)
            {
                off += 2;
                off += 8; // fileref
                off += 8; // unknown
            }

            int longNameSize = 0;
            if (ExtensionVersion >= 0x03)
            {
                longNameSize = BlockHelper.UnpackWord(buf, offset + off);
                off += 2;
            }

            if (ExtensionVersion >= 0x09)
                off += 4; // unknown

            if (ExtensionVersion >= 0x08)
                off += 4; // unknown

            if (ExtensionVersion >= 0x03)
            {
                fields["LongName"] = BlockHelper.UnpackWString(buf, offset + off);
                off += 2 * (LongName.Length + 1);
            }
            if (ExtensionVersion >= 0x03 && ExtensionVersion < 0x07 && longNameSize > 0)
            {
                fields["LocalizedName"] = BlockHelper.UnpackString(buf, offset + off);
            }
            else if (ExtensionVersion >= 0x07 && longNameSize > 0)
            {
                fields["LocalizedName"] = BlockHelper.UnpackWString(buf, offset + off);
            }
        }
    }
}