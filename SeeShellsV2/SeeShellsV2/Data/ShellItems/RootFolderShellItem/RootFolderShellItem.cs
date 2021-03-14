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
        /// <summary>
        /// Windows GUID string that corresponds to a particular root folder
        /// </summary>
        public string RootFolderGuid
        {
            init => fields["RootFolderGuid"] = value;
            get => fields.GetClassOrDefault("RootFolderGuid", string.Empty);
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
    }
}