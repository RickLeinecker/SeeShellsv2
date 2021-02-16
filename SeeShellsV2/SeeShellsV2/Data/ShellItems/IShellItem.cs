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
    public interface IShellItem
    {
        /// <summary>
        /// A best effort description for a particular ShellItem meant to give the most important / recognizable
        /// piece of information about the Shellitem.
        /// Description can be used to refer to:
        /// <list type="bullet">
        /// <item>filename (e.g. foo.zip)</item>
        /// <item>directory name</item> (e.g. "C:\" for the C drive directory)
        /// <item>GUID (or the known correspondence of a GUID (see <see cref="KnownGuids"/>)</item>
        /// </list> 
        /// </summary>
        string Description { init; get; }

        /// <summary>
        /// Registry Key from which the shell item was parsed
        /// </summary>
        RegistryKeyWrapper RegistryKey { get; set; }

        /// <summary>
        /// Parent of this shell item
        /// </summary>
        IShellItem Parent { get; set; }

        /// <summary>
        /// Children of this shell item
        /// </summary>
        IList<IShellItem> Children { get; }

        /// <summary>
        /// Tells the Size of the entire ShellItem. Includes the two bytes to represent the sie parameter.
        /// If Size is 0 it means the rest of the shell item is empty. (e.g. 2 byte array with the size value of 0x00 is an empty shellitem) 
        /// </summary>
        ushort Size { init; get; }

        /// <summary>
        /// Unique byte identifier that represents a shell item type.
        /// Has no effect on signature based Shell items.
        /// See <seealso cref="TypeName"/>, <seealso cref="SubTypeName"/> for a Human readable interpretation of the Shell Item.
        /// </summary>
        byte Type { init; get; }

        /// <summary>
        /// Unique uint identifier that represents a shell item type.
        /// Only present on signature based shell items
        /// See <seealso cref="TypeName"/>, <seealso cref="SubTypeName"/> for a Human readable interpretation of the Shell Item.
        /// </summary>
        uint Signature { init; get; }

        /// <summary>
        /// Human readable interpretation of <see cref="Type"/>
        /// </summary>
        string TypeName { init; get; }

        /// <summary>
        /// Human readable interpretation of <see cref="Type"/>
        /// </summary>
        string SubtypeName { init; get; }

        /// <summary>
        /// The last known modification date for the data represented in this ShellItem.
        /// If the value was not found or unreadable, the value returned is <see cref="DateTime.MinValue"/>
        /// </summary>
        DateTime ModifiedDate { init; get; }

        /// <summary>
        /// The last known access date for the data represented in this ShellItem.
        /// If the value was not found or unreadable, the value returned is <see cref="DateTime.MinValue"/>
        /// </summary>
        DateTime AccessedDate { init; get; }

        /// <summary>
        /// The creation date for the data represented in this ShellItem.
        /// If the value was not found or unreadable, the value returned is <see cref="DateTime.MinValue"/>
        /// </summary>
        DateTime CreationDate { init; get; }

        /// <summary>
        /// all properties that exist in the ShellItem in Key-Value Format.
        /// </summary>
        IReadOnlyDictionary<string, object> Fields { get; }

        /// <summary>
        /// List of <see cref="IExtensionBlock"/> instances associated with the shell item
        /// </summary>
        IReadOnlyList<IExtensionBlock> ExtensionBlocks { get; }

        /// <summary>
        /// Set of tags associated with the shell item
        /// </summary>
        ISet<IShellTag> Tags { get; }
    }
}
