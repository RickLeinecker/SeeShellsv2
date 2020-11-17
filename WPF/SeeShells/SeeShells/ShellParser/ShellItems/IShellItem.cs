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
    public interface IShellItem
    {
        /// <summary>
        /// Tells the Size of the entire ShellItem. Includes the two bytes to represent the sie parameter.
        /// If Size is 0 it means the rest of the shell item is empty. (e.g. 2 byte array with the size value of 0x00 is an empty shellitem) 
        /// </summary>
        ushort Size { get; }
        /// <summary>
        /// Unique byte identifier that represents an indicator for the shell item.
        /// Subtypes exist and must be further seperated by other information in the object. 
        /// Has no effect on signature based Shell items.
        /// See <seealso cref="TypeName"/> for a Human readable interpretation of the Shell Item.
        /// </summary>
        byte Type { get; }
        /// <summary>
        /// Human readable interpretation of  <see cref="Type"/>
        /// </summary>
        string TypeName { get; }
        /// <summary>
        /// A best effort value identifier for a particular ShellItem meant to give the most important / recognizable
        /// piece of information about the Shellitem.
        /// Name can be used to refer to:
        /// <list type="bullet">
        /// <item>filename (e.g. foo.zip)</item>
        /// <item>directory name</item> (e.g. "C:\" for the C drive directory)
        /// <item>GUID (or the known correspondence of a GUID (see <see cref="KnownGuids"/>)</item>
        /// </list> 
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The last known modification date for the data represented in this ShellItem.
        /// If the value was not found or unreadable, the value returned is <see cref="DateTime.MinValue"/>
        /// </summary>
        DateTime ModifiedDate { get; }
        /// <summary>
        /// The last known access date for the data represented in this ShellItem.
        /// If the value was not found or unreadable, the value returned is <see cref="DateTime.MinValue"/>
        /// </summary>
        DateTime AccessedDate { get; }
        /// <summary>
        /// The creation date for the data represented in this ShellItem.
        /// If the value was not found or unreadable, the value returned is <see cref="DateTime.MinValue"/>
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        /// This function returns all properties that exist in the ShellItem
        /// in Key-Value Format. Implementations of this method should append
        /// the base implementation and append any additional fields as created.
        /// </summary>
        /// <returns>A Dictionary with all gettable properties in string form.</returns>
        IDictionary<string, string> GetAllProperties();

    }
}
