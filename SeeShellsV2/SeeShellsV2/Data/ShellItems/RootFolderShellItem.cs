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
    /// Shell items that document windows root folders:
    /// 0x1F
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#32-root-folder-shell-item
    public class RootFolderShellItem : ShellItem, IShellItem
    {
        public static IReadOnlySet<byte> KnownTypes = new HashSet<byte>{ 0x1F }; // libwsi and v1 team

        /// <summary>
        /// Windows GUID string that corresponds to a particular root folder
        /// </summary>
        public string RootFolderGuid
        {
            init => fields["RootFolderGuid"] = value;
            get => fields.GetClassOrDefault("RootFolderGuid", string.Empty);
        }

        /// <summary>
        /// Human readable name of the root folder
        /// </summary>
        public string RootFolderName
        {
            init => fields["RootFolderName"] = value;
            get => fields.GetClassOrDefault("RootFolderName", string.Empty);
        }

        /// <summary>
        /// Index presumably used for sorting objects in File Exporer
        /// Provides additional information about the shell item
        /// </summary>
        public byte SortIndex
        {
            init => fields["SortIndex"] = value;
            get => fields.GetStructOrDefault<byte>("SortIndex", 0xFF);
        }

        /// <summary>
        /// Human readable version of <see cref="SortIndex"/>
        /// </summary>
        public string SortIndexDescription
        {
            init => fields["SortIndexDescription"] = value;
            get => fields.GetClassOrDefault("SortIndexDescription", "Unknown");
        }

        public RootFolderShellItem() { }

        public RootFolderShellItem(byte[] buf)
        {
            try
            {
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Type"] = Block.UnpackByte(buf, 0x02);

                fields["TypeName"] = "Root Folder";

                fields["RootFolderGuid"] = Block.UnpackGuid(buf, 0x04);

                if (KnownGuids.dict.ContainsKey(RootFolderGuid))
                    fields["Description"] = fields["RootFolderName"] = KnownGuids.dict[RootFolderGuid];
                else
                    fields["Description"] = fields["RootFolderName"] = RootFolderGuid;

                fields["SortIndex"] = Block.UnpackByte(buf, 0x03);

                switch (SortIndex)
                {
                    case 0x00:
                        fields["SortIndexDescription"] = "INTERNET_EXPLORER";
                        break;
                    case 0x42:
                        fields["SortIndexDescription"] = "LIBRARIES";
                        break;
                    case 0x44:
                        fields["SortIndexDescription"] = "USERS";
                        break;
                    case 0x48:
                        fields["SortIndexDescription"] = "MY_DOCUMENTS";
                        break;
                    case 0x50:
                        fields["SortIndexDescription"] = "MY_COMPUTER";
                        break;
                    case 0x58:
                        fields["SortIndexDescription"] = "NETWORK";
                        break;
                    case 0x60:
                        fields["SortIndexDescription"] = "RECYCLE_BIN";
                        break;
                    case 0x68:
                        fields["SortIndexDescription"] = "INTERNET_EXPLORER";
                        break;
                    case 0x70:
                        fields["SortIndexDescription"] = "UNKNOWN";
                        break;
                    case 0x80:
                        fields["SortIndexDescription"] = "MY_GAMES";
                        break;
                    default:
                        break;
                }
            }
            catch (ShellParserException ex)
            {
                throw new ArgumentException("byte array could not be parsed into RootFolderShellItem", ex);
            }
        }
    }
}