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
    public interface IExtensionBlock
    {
        /// <summary>
        /// Size of the extension block
        /// </summary>
        ushort Size { init; get; }

        /// <summary>
        /// Version of the extension block
        /// </summary>
        ushort ExtensionVersion { init; get; }

        /// <summary>
        /// extension block signature unique to each type of extension block
        /// </summary>
        uint Signature { init; get; }

        /// <summary>
        /// all properties that exist in the ExtensionBlock in Key-Value Format.
        /// </summary>
        IReadOnlyDictionary<string, object> Fields { get; }
    }
}