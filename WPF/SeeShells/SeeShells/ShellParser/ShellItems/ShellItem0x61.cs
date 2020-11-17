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

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// URI Shell Type
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#37-uri-shell-item
    public class ShellItem0x61 : ShellItem
    {
        public string Uri { get; protected set; }
        public string FTPHostname { get; protected set; }
        public string FTPUsername { get; protected set; }
        public string FTPPassword { get; protected set; }

        public uint Flags { get; protected set; }
        public DateTime ConnectionDate { get; protected set; }

        public override string TypeName { get => "URI"; }
        public override string Name
        {
            get
            {
                return FTPHostname ?? Uri;
            }
        }

        public ShellItem0x61(byte[] buf) : base(buf)
        {
            Flags = unpack_dword(0x03);
            int off = 0x04;
            ushort dataSize = unpack_word(off); //0 if no data, does not include 2 bytes of the normal size indicator
            if (dataSize != 0)
            {
                off += 2; //move past Size of Data
                off += 4; //move past unknown
                off += 4; //move past unknown
                if (off < Size)
                {
                    ConnectionDate = UnpackFileTime(off); //timestamp in "FILETIME" format (location: 0x0E)
                    off += 8; //move past ConnectionTime
                }
                off += 4; //move past unknown 0000 or FFFF
                off += 12; //move past unknown empty section
                off += 4; //unknown
                if (off < Size) 
                {
                    uint hostnameSize = unpack_dword(off);
                    off += 4; //move past hostnameSize
                    FTPHostname = unpack_string(off);
                    off += (int)hostnameSize; //move past Uri
                }
                if (off < Size)
                {
                    uint usernameSize = unpack_dword(off);
                    off += 4; //move past hostnameSize
                    FTPUsername = unpack_string(off);
                    off += (int)usernameSize; //move past Uri

                }
                if (off < Size)
                {
                    uint passwordSize = unpack_dword(off);
                    off += 4; //move past hostnameSize
                    FTPPassword = unpack_string(off);
                    off += (int)passwordSize; //move past Uri

                }
                if (off < Size) //immediately afterwards is a common Uri
                {
                    Uri = unpack_string(off);
                }

            }
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, Constants.URI, Uri);
            AddPairIfNotNull(ret, Constants.FTP_HOST_NAME, FTPHostname);
            AddPairIfNotNull(ret, Constants.FTP_USER_NAME, FTPUsername);
            AddPairIfNotNull(ret, Constants.FTP_PASSWORD, FTPPassword);
            AddPairIfNotNull(ret, Constants.FLAGS, Flags);
            AddPairIfNotNull(ret, Constants.CONNECTION_DATE, ConnectionDate);
            return ret;
        }
    }
}