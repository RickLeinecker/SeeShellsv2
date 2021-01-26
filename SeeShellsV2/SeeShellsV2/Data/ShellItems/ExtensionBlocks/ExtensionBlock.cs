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

namespace SeeShellsV2.Data
{
    public class ExtensionBlock : IExtensionBlock
    {
        public ushort Size
        {
            get => fields["Size"] as ushort? ?? 0;
        }

        public ushort ExtensionVersion
        {
            get => fields["ExtensionVersion"] as ushort? ?? 0;
        }

        public uint Signature
        {
            get => fields["Signature"] as uint? ?? 0;
        }

        public IReadOnlyDictionary<string, object> Fields
        {
            get => fields;
        }

        public ExtensionBlock(byte[] buf, int offset)
        {
            fields["Size"] = Block.unpack_word(buf, offset + 0x00);
            fields["ExtensionVersion"] = Block.unpack_word(buf, offset + 0x02);
            fields["Signature"] = Block.unpack_dword(buf, offset + 0x04);
        }

        protected Dictionary<string, object> fields = new Dictionary<string, object>();
    }
}
