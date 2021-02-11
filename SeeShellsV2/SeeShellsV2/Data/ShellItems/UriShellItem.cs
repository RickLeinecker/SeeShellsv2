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
    /// Shell items that document URIs:
    /// 0x61
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#37-uri-shell-item
    public class UriShellItem : ShellItem, IShellItem
    {
        public static IReadOnlySet<byte> KnownTypes = new HashSet<byte> { 0x61 }; // libwsi and v1 team

        [Flags]
        public enum UriFlagBits
        {
            None = 0x00,
            HasUnicodeStrings = 0x80
        }

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

        public UriFlagBits UriFlags
        {
            init => fields["UriFlags"] = (int)value;
            get => (UriFlagBits)fields.GetStructOrDefault("UriFlags", 0);
        }

        public DateTime ConnectionDate
        {
            init => fields["ConnectionDate"] = value;
            get => fields.GetStructOrDefault("ConnectionDate", DateTime.MinValue);
        }

        public UriShellItem() { }

        public UriShellItem(byte[] buf)
        {
            try
            {
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Type"] = Block.UnpackByte(buf, 0x02);

                fields["TypeName"] = "URI";

                int off = 0x03;

                fields["UriFlags"] = (int)Block.UnpackByte(buf, off);
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
                        off = Block.AlignTo(2, off, 4);
                        uint length = Block.UnpackDWord(buf, off);
                        off += 4; //move past size
                        off = Block.AlignTo(2, off, 4);
                        if ((Type & (byte)UriFlagBits.HasUnicodeStrings) != 0)
                        {
                            fields["FTPHostname"] = Block.UnpackWString(buf, off);
                        }
                        else
                        {
                            fields["FTPHostname"] = Block.UnpackString(buf, off);
                        }
                        off += (int)length; //move past string
                    }
                    if (off < Size)
                    {
                        off = Block.AlignTo(2, off, 4);
                        uint length = Block.UnpackDWord(buf, off);
                        off += 4; //move past size
                        off = Block.AlignTo(2, off, 4);
                        if ((Type & (byte)UriFlagBits.HasUnicodeStrings) != 0)
                        {
                            fields["FTPUsername"] = Block.UnpackWString(buf, off);
                        }
                        else
                        {
                            fields["FTPUsername"] = Block.UnpackString(buf, off);
                        }
                        off += (int)length; //move past string
                    }
                    if (off < Size)
                    {
                        off = Block.AlignTo(2, off, 4);
                        uint length = Block.UnpackDWord(buf, off);
                        off += 4; //move past size
                        off = Block.AlignTo(2, off, 4);
                        if ((Type & (byte)UriFlagBits.HasUnicodeStrings) != 0)
                        {
                            fields["FTPPassword"] = Block.UnpackWString(buf, off);
                        }
                        else
                        {
                            fields["FTPPassword"] = Block.UnpackString(buf, off);
                        }
                        off += (int)length; //move past string
                    }
                    if (off < Size) //immediately afterwards is a common Uri
                    {
                        off = Block.AlignTo(2, off, 4);
                        if ((Type & (byte)UriFlagBits.HasUnicodeStrings) != 0)
                        {
                            fields["Uri"] = Block.UnpackWString(buf, off);
                        }
                        else
                        {
                            fields["Uri"] = Block.UnpackString(buf, off);
                        }
                    }
                }

                fields["Description"] = FTPHostname ?? Uri ?? string.Empty;
            }
            catch (ShellParserException ex)
            {
                throw new ArgumentException("byte array could not be parsed into UriShellItem", ex);
            }
        }
    }
}