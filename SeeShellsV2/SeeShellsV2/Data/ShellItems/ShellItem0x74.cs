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
    /// Delegate Shell Item
    /// </summary>
    public class ShellItem0x74 : ShellItem, IShellItem
    {
        public uint Signature
        {
            init => fields["Signature"] = value;
            get => fields.GetStructOrDefault<uint>("Signature", 0);
        }

        public FileEntryFragment SubItem
        {
            init => fields["SubItem"] = value;
            get => fields.GetClassOrDefault<FileEntryFragment>("SubItem", null);
        }

        public string DelegateItemIdentifier
        {
            init => fields["DelegateItemIdentifier"] = value;
            get => fields.GetClassOrDefault("DelegateItemIdentifier", string.Empty);
        }

        public string ItemClassIdentifier
        {
            init => fields["ItemClassIdentifier"] = value;
            get => fields.GetClassOrDefault("ItemClassIdentifier", string.Empty);
        }

        public ShellItem0x74() { }

        public ShellItem0x74(byte[] buf) : base(buf)
        {
            // Unknown - Empty ( 1 byte)
            // Unknown - size? - 2 bytes
            // CFSF - 4 bytes
            fields["Signature"] = Block.unpack_dword(buf, 0x06);
            // sub shell item data size - 2 bytes

            int off = 0x0A;
            fields["SubItem"] = new FileEntryFragment(buf, off, this, 0x04);
            off += SubItem.Size;

            off += 2; // Empty extension block?

            // 5e591a74-df96-48d3-8d67-1733bcee28ba
            fields["DelegateItemIdentifier"] = Block.unpack_guid(buf, off);
            off += 16;
            fields["ItemClassIdentifier"] = Block.unpack_guid(buf, off);
            off += 16;

            var extensionBlock = new ExtensionBlockBEEF0004(buf, off);
            extensionBlocks.Add(extensionBlock);

            fields["TypeName"] = "Delegate Item";

            if (extensionBlock != null && extensionBlock.LongName != null && extensionBlock.LongName.Length > 0)
                fields["Name"] = extensionBlock.LongName;
            else
                fields["Name"] = SubItem.ShortName;

            fields["ModifiedDate"] = SubItem.ModifiedDate;

            if (extensionBlock != null)
            {
                fields["AccessedDate"] = extensionBlock.CreationDate;
                fields["CreationDate"] = extensionBlock.CreationDate;
            }
        }
    }
}