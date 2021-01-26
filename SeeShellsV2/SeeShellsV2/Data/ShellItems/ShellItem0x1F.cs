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

namespace SeeShellsV2.Data
{
    public class ShellItem0x1F : ShellItem, IShellItem
    {
        public string Guid
        {
            init => fields["Guid"] = value;
            get => fields.GetClassOrDefault("Guid", string.Empty);
        }

        public string FolderId
        {
            init => fields["FolderId"] = value;
            get => fields.GetClassOrDefault("FolderId", string.Empty);
        }

        public ShellItem0x1F() { }

        public ShellItem0x1F(byte[] buf) : base(buf)
        {
            fields["TypeName"] = "Root Folder";
            fields["Guid"] = Block.unpack_guid(buf, 0x04);

            if (KnownGuids.dict.ContainsKey(Guid))
                fields["Name"] = string.Format("{{{0}}}", KnownGuids.dict[Guid]);
            else
                fields["Name"] = string.Format("{{{0}}}", Guid);

            byte id = Block.unpack_byte(buf, 0x03);

            switch (id)
            {
                case 0x00:
                    fields["FolderId"] = "INTERNET_EXPLORER";
                    break;
                case 0x42:
                    fields["FolderId"] = "LIBRARIES";
                    break;
                case 0x44:
                    fields["FolderId"] = "USERS";
                    break;
                case 0x48:
                    fields["FolderId"] = "MY_DOCUMENTS";
                    break;
                case 0x50:
                    fields["FolderId"] = "MY_COMPUTER";
                    break;
                case 0x58:
                    fields["FolderId"] = "NETWORK";
                    break;
                case 0x60:
                    fields["FolderId"] = "RECYCLE_BIN";
                    break;
                case 0x68:
                    fields["FolderId"] = "INTERNET_EXPLORER";
                    break;
                case 0x70:
                    fields["FolderId"] = "UNKNOWN";
                    break;
                case 0x80:
                    fields["FolderId"] = "MY_GAMES";
                    break;
                default:
                    fields["FolderId"] = "";
                    break;
            }
        }
    }
}