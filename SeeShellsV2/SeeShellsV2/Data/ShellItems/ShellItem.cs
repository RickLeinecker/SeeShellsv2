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
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace SeeShellsV2.Data
{
    public abstract class ShellItem : IShellItem
    {
        public string Description
        {
            init => fields["Description"] = value;
            get => fields.GetClassOrDefault("Description", "??");
        }

        public Place Place
        {
            init => fields["Place"] = value;
            get => fields.GetClassOrDefault<Place>("Place", null);
        }

        public RegistryHive RegistryHive
        {
            init => fields["RegistryHive"] = value;
            get => fields.GetClassOrDefault<RegistryHive>("RegistryHive", null);
        }

        public byte[] Value
        {
            init => fields["Value"] = value;
            get => fields.GetClassOrDefault<byte[]>("Value", null);
        }

        public int? NodeSlot
        {
            init => fields["NodeSlot"] = value;
            get => fields.GetClassOrDefault<object>("NodeSlot", null) as int?;
        }

        public DateTime? SlotModifiedDate
        {
            init => fields["SlotModifiedDate"] = value;
            get => fields.GetClassOrDefault<object>("SlotModifiedDate", null) as DateTime?;
        }

        public DateTime LastRegistryWriteDate
        {
            init => fields["LastRegistryWriteDate"] = value;
            get => fields.GetStructOrDefault("LastRegistryWriteDate", DateTime.MinValue);
        }

        public IShellItem Parent
        {
            init; get;
        }

        public IList<IShellItem> Children
        {
            get => children;
        }

        public ushort Size
        {
            init => fields["Size"] = value;
            get => fields.GetStructOrDefault<ushort>("Size", 0);
        }

        public byte Type
        {
            init => fields["Type"] = value;
            get => fields.GetStructOrDefault<byte>("Type", 0);
        }

        public uint Signature
        {
            init => fields["Signature"] = value;
            get => fields.GetStructOrDefault<uint>("Signature", 0);
        }

        public string TypeName
        {
            init => fields["TypeName"] = value;
            get => fields.GetClassOrDefault("TypeName", "Unknown");
        }

        public string SubtypeName
        {
            init => fields["SubtypeName"] = value;
            get => fields.GetClassOrDefault("SubtypeName", "Unknown");
        }

        public IReadOnlyDictionary<string, object> Fields
        {
            get => fields;
        }

        public IReadOnlyCollection<IExtensionBlock> ExtensionBlocks
        {
            init => extensionBlocks = value;
            get => extensionBlocks;
        }

        public ISet<IShellTag> Tags
        {
            get => tags;
        }

        protected IList<IShellItem> children = new List<IShellItem>();
        protected Dictionary<string, object> fields = new Dictionary<string, object>();
        protected IReadOnlyCollection<IExtensionBlock> extensionBlocks = new List<IExtensionBlock>();
        protected SortedSet<IShellTag> tags = new SortedSet<IShellTag>();
    }
}
