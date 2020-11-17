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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.ShellParser.ShellItems.ExtensionBlocks
{

    public class ExtensionBlock : Block, IExtensionBlock
    {

        public virtual ushort Size { get; protected set; }
        public virtual ushort ExtensionVersion { get; protected set; }
        public virtual uint Signature { get; protected set; }


        public ExtensionBlock(byte[] buf, int offset) : base (buf, offset)
        {
            Size = unpack_word(0x00);
            ExtensionVersion = unpack_word(0x02);
            Signature = unpack_dword(0x04);
        }

        public virtual IDictionary<string, string> GetAllProperties()
        {
            SortedDictionary<string, string> properties = new SortedDictionary<string, string>();
            AddPairIfNotNull(properties, Constants.SIZE, Size.ToString("X2") ); //hexidecimal with 2 numerical places (aka a byte)
            AddPairIfNotNull(properties, Constants.EXTENSION_VERSION, ExtensionVersion.ToString() );
            AddPairIfNotNull(properties, Constants.SIGNATURE, Signature.ToString("X4") );
            return properties;
        }
    }
}
