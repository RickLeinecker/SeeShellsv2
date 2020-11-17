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
using SeeShells.ShellParser.ShellItems.ExtensionBlocks;
using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Delegate Shell Item
    /// </summary>
    public class ShellItem0x74 : ShellItemWithExtensions
    {

        protected ExtensionBlockBEEF0004 ExtensionBlock { get; }

        public uint Signature { get; protected set; }
        public FILEENTRY_FRAGMENT SubItem { get; protected set; }
        public string DelegateItemIdentifier { get; protected set; }
        public string ItemClassIdentifier { get; protected set; }
        public override string TypeName { get => "Delegate Item"; }
        public override string Name
        {
            get
            {
                if (ExtensionBlock.LongName.Length > 0)
                    return ExtensionBlock.LongName;
                return SubItem.short_name;
            }
        }

        public override DateTime ModifiedDate
        {
            get
            {
                return SubItem.ModifiedDate;
            }
        }
        public override DateTime CreationDate
        {
            get
            {
                if (ExtensionBlock != null)
                    return ExtensionBlock.CreationDate;
                return base.CreationDate;
            }
        }
        public override DateTime AccessedDate
        {
            get
            {
                if (ExtensionBlock != null)
                    return ExtensionBlock.AccessedDate;
                return base.AccessedDate;
            }
        }

        public ShellItem0x74(byte[] buf)
            : base(buf)
        {
            // Unknown - Empty ( 1 byte)
            // Unknown - size? - 2 bytes
            // CFSF - 4 bytes
            Signature = unpack_dword(0x06);
            // sub shell item data size - 2 bytes

            int off = 0x0A;
            SubItem = new FILEENTRY_FRAGMENT(buf, offset + off, this, 0x04);
            off += SubItem.Size;

            off += 2; // Empty extension block?

            // 5e591a74-df96-48d3-8d67-1733bcee28ba
            DelegateItemIdentifier = unpack_guid(off);
            off += 16;
            ItemClassIdentifier = unpack_guid(off);
            off += 16;
            ExtensionBlock = new ExtensionBlockBEEF0004(buf, offset + off);
            ExtensionBlocks.Add(ExtensionBlock);            
            
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, Constants.SIGNATURE, Signature);
            AddPairIfNotNull(ret, Constants.DELEGATE_ITEM_ID, DelegateItemIdentifier);
            AddPairIfNotNull(ret, Constants.ITEM_CLASS_ID, ItemClassIdentifier);
            return ret;
        }
    }
}