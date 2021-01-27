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
    /// <summary>
    /// URI Shell Type
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#37-uri-shell-item
    public class ShellItem0x61 : ShellItem, IShellItem
    {
        public string Uri
        {
            init => fields["Uri"] = value;
            get => fields.GetClassOrDefault("Uri", string.Empty);
        }

        public string FTPHostname
        {
            init => fields["FTPHostname"] = value;
            get => fields.GetClassOrDefault("FTPHostname", string.Empty);
        }

        public string FTPUsername
        {
            init => fields["FTPUsername"] = value;
            get => fields.GetClassOrDefault("FTPUsername", string.Empty);
        }

        public string FTPPassword
        {
            init => fields["FTPPassword"] = value;
            get => fields.GetClassOrDefault("FTPPassword", string.Empty);
        }

        public byte Flags
        {
            init => fields["Flags"] = value;
            get => fields.GetStructOrDefault<byte>("Flags", 0);
        }

        public DateTime ConnectionDate
        {
            init => fields["Uri"] = value;
            get => fields.GetStructOrDefault("ConnectionDate", DateTime.MinValue);
        }

        public ShellItem0x61() { }

        public ShellItem0x61(byte[] buf) : base(buf)
        {
            fields["TypeName"] = "URI";

            int off = 0x03;

            fields["Flags"] = Block.UnpackByte(buf, off);
            off += 1;

            ushort dataSize = Block.UnpackWord(buf, off); //0 if no data, does not include 2 bytes of the normal size indicator
            if (dataSize != 0)
            {
                off += 2; //move past Size of Data
                off += 4; //move past unknown
                off += 4; //move past unknown
                if (off < Size)
                {
                    fields["ConnectionDate"] = Block.UnpackFileTime(buf, off); //timestamp in "FILETIME" format (location: 0x0E)
                    off += 8; //move past ConnectionTime
                }
                off += 4; //move past unknown 0000 or FFFF
                off += 12; //move past unknown empty section
                off += 4; //unknown
                if (off < Size) 
                {
                    uint hostnameSize = Block.UnpackDWord(buf, off);
                    off += 4; //move past hostnameSize
                    fields["FTPHostname"] = Block.UnpackString(buf, off);
                    off += (int)hostnameSize; //move past Uri
                }
                if (off < Size)
                {
                    uint usernameSize = Block.UnpackDWord(buf, off);
                    off += 4; //move past hostnameSize
                    fields["FTPUsername"] = Block.UnpackString(buf, off);
                    off += (int)usernameSize; //move past Uri

                }
                if (off < Size)
                {
                    uint passwordSize = Block.UnpackDWord(buf, off);
                    off += 4; //move past hostnameSize
                    fields["FTPPassword"] = Block.UnpackString(buf, off);
                    off += (int)passwordSize; //move past Uri

                }
                if (off < Size) //immediately afterwards is a common Uri
                {
                    fields["Uri"] = Block.UnpackString(buf, off);
                }
            }

            fields["Name"] = FTPHostname ?? Uri ?? string.Empty;
        }
    }
}