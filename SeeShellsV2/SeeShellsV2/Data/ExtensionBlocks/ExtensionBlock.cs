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

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class ExtensionBlock : IExtensionBlock
    {
        public ushort Size
        {
            init => fields["Size"] = value;
            get => fields.GetStructOrDefault<ushort>("Size", 0);
        }

        public ushort ExtensionVersion
        {
            init => fields["ExtensionVersion"] = value;
            get => fields.GetStructOrDefault<ushort>("ExtensionVersion", 0);
        }

        public uint Signature
        {
            init => fields["Signature"] = value;
            get => fields.GetStructOrDefault<uint>("Signature", 0);
        }

        public IReadOnlyDictionary<string, object> Fields
        {
            get => fields;
        }

        public ExtensionBlock() { }

        public ExtensionBlock(byte[] buf, int offset)
        {
            fields["Size"] = BlockHelper.UnpackWord(buf, offset + 0x00);
            fields["ExtensionVersion"] = BlockHelper.UnpackWord(buf, offset + 0x02);
            fields["Signature"] = BlockHelper.UnpackDWord(buf, offset + 0x04);
        }

        protected Dictionary<string, object> fields = new Dictionary<string, object>();
    }
}
