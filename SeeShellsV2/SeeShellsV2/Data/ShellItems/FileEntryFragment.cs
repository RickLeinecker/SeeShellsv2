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

namespace SeeShellsV2.Data
{
    enum ShellItemType
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

    public class FileEntryFragment : ShellItem
    {
        public uint FileSize
        {
            init => fields["FileSize"] = value;
            get => fields.GetStructOrDefault<uint>("FileSize", 0);
        }

        public ushort FileAttributes
        {
            init => fields["FileAttributes"] = value;
            get => fields.GetStructOrDefault<ushort>("FileAttributes", 0);
        }

        public string ShortName
        {
            init => fields["ShortName"] = value;
            get => fields["ShortName"] as string ?? string.Empty;
        }

        public FileEntryFragment() { }

        public FileEntryFragment(byte[] buf, int offset, object parent, int filesize_offset) : base(buf)
        {
            int off = filesize_offset;
            fields["FileSize"] = Block.unpack_dword(buf, off);
            off += 4;
            fields["ModifiedDate"] = Block.unpack_dosdate(buf, off);
            off += 4;
            fields["FileAttributes"] = Block.unpack_word(buf, off);
            off += 2;
            fields["ShortName"] = Block.unpack_string(buf, off);

            fields["Name"] = ShortName;
        }
    }
}