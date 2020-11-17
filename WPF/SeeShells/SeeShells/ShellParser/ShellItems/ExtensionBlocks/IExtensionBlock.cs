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

namespace SeeShells.ShellParser.ShellItems.ExtensionBlocks
{
    public interface IExtensionBlock
    {
        ushort Size { get; }
        ushort ExtensionVersion { get; }
        uint Signature { get; }

        /// <summary>
        /// This function returns all properties that exist in the Extension Block
        /// in Key-Value Format. Implementations of this method should append
        /// the base implementation and append any additional fields as created.
        /// </summary>
        /// <returns>A Dictionary with all gettable properties in string form.</returns>
        IDictionary<string, string> GetAllProperties();

    }
}