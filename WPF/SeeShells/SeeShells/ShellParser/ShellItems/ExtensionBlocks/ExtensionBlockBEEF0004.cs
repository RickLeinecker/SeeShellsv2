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

namespace SeeShells.ShellParser.ShellItems.ExtensionBlocks
{
    public class ExtensionBlockBEEF0004 : ExtensionBlock
    {
        public DateTime CreationDate { get; protected set; }
        public DateTime AccessedDate { get; protected set; }
        public ushort LongNameSize { get; protected set; }
        public string LongName { get; protected set; }
        public string LocalizedName { get; protected set; }
        public ExtensionBlockBEEF0004(byte[] buf, int offset) : base(buf, offset)
        {
            int off = 0x08;  //pass all the known Extension Block Headers

            if (ExtensionVersion >= 0x03)
            {
                CreationDate = unpack_dosdate(off);
                off += 4;
                AccessedDate = unpack_dosdate(off);
                off += 4;
                off += 2; // unknown
            }
            else
            {
                CreationDate = DateTime.MinValue;
                AccessedDate = DateTime.MinValue;
            }

            if (ExtensionVersion >= 0x07)
            {
                off += 2;
                off += 8; // fileref
                off += 8; // unknown
            }

            if (ExtensionVersion >= 0x03)
            {
                LongNameSize = unpack_word(off);
                off += 2;
            }

            if (ExtensionVersion >= 0x09)
                off += 4; // unknown

            if (ExtensionVersion >= 0x08)
                off += 4; // unknown

            if (ExtensionVersion >= 0x03)
            {
                LongName = unpack_wstring(off);
                off += 2 * (LongName.Length + 1);
            }
            if (ExtensionVersion >= 0x03 && ExtensionVersion < 0x07 && LongNameSize > 0)
            {
                LocalizedName = unpack_string(off);
            }
            else if (ExtensionVersion >= 0x07 && LongNameSize > 0)
            {
                LocalizedName = unpack_wstring(off);
            }
            else
            {
                LocalizedName = string.Empty;
            }
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, Constants.CREATION_DATE, CreationDate);
            AddPairIfNotNull(ret, Constants.ACCESSED_DATE, AccessedDate);
            AddPairIfNotNull(ret, Constants.LONG_NAME, LongName);
            AddPairIfNotNull(ret, Constants.LOCALIZED_NAME, LocalizedName);
            return ret;
        }
    }
}