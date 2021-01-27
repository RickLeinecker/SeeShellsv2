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

namespace SeeShellsV2.Data
{
    /// <summary>
    /// File Entry Shell Item
    /// </summary>
    //Reference for File Entry Shell Types:
    //https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#34-file-entry-shell-item
    public class ShellItem0x30 : ShellItem, IShellItem
    {
        public uint FileSize
        {
            init => fields["FileSize"] = value;
            get => fields.GetStructOrDefault<uint>("FileSize", 0);
        }

        //TODO - File Attributes should be enumerated they are a list of file flags.
        //https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#71-file-attribute-flags
        public ushort FileAttributes
        {
            init => fields["FileAttributes"] = value;
            get => fields.GetStructOrDefault<ushort>("FileAttributes", 0);
        }

        public byte Flags
        {
            init => fields["Flags"] = value;
            get => fields.GetStructOrDefault<byte>("Flags", 0);
        }

        public ushort ExtensionOffset
        {
            init => fields["ExtensionOffset"] = value;
            get => fields.GetStructOrDefault<ushort>("ExtensionOffset", 0);
        }

        public string ShortName
        {
            init => fields["ShortName"] = value;
            get => fields.GetClassOrDefault("ShortName", string.Empty);
        }

        public ShellItem0x30() { }

        public ShellItem0x30(byte[] buf) : base(buf)
        {
            fields["TypeName"] = "File Entry";

            int offset = 0x03;

            fields["Flags"] = Block.UnpackByte(buf, offset);
            offset += 1;
            fields["FileSize"] = Block.UnpackDWord(buf, offset);
            offset += 4;
            fields["ModifiedDate"] = Block.UnpackDosDateTime(buf, offset);
            offset += 4;
            fields["FileAttributes"] = Block.UnpackWord(buf, offset);
            offset += 2;
            fields["ExtensionOffset"] = Block.UnpackWord(buf, Size - 2);

            if ((Type & 0x04) != 0)
                fields["ShortName"] = Block.UnpackWString(buf, offset);
            else
                fields["ShortName"] = Block.UnpackString(buf, offset);

            ExtensionBlockBEEF0004 extensionBlock = new ExtensionBlockBEEF0004(buf, ExtensionOffset + offset);
            extensionBlocks.Add(extensionBlock);

            fields["Name"] = extensionBlock.LongName;
            fields["CreationDate"] = extensionBlock.CreationDate;
            fields["AccessedDate"] = extensionBlock.AccessedDate;
        }
    }
}