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
            init => fields[nameof(Description)] = value;
            get => fields.GetClassOrDefault(nameof(Description), "??");
        }

        public Place Place
        {
            init { fields[nameof(Place)] = value; value.Items.Add(this); }
            get => fields.GetClassOrDefault<Place>(nameof(Place), null);
        }

        public RegistryHive RegistryHive
        {
            init => fields[nameof(RegistryHive)] = value;
            get => fields.GetClassOrDefault<RegistryHive>(nameof(RegistryHive), null);
        }

        public byte[] Value
        {
            init => fields[nameof(Value)] = value;
            get => fields.GetClassOrDefault<byte[]>(nameof(Value), null);
        }

        public int? NodeSlot
        {
            init => fields[nameof(NodeSlot)] = value;
            get => fields.GetClassOrDefault<object>(nameof(NodeSlot), null) as int?;
        }

        public DateTime? SlotModifiedDate
        {
            init => fields[nameof(SlotModifiedDate)] = value;
            get => fields.GetClassOrDefault<object>(nameof(SlotModifiedDate), null) as DateTime?;
        }

        public DateTime LastRegistryWriteDate
        {
            init => fields[nameof(LastRegistryWriteDate)] = value;
            get => fields.GetStructOrDefault(nameof(LastRegistryWriteDate), DateTime.MinValue);
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
            init => fields[nameof(Size)] = value;
            get => fields.GetStructOrDefault<ushort>(nameof(Size), 0);
        }

        public byte Type
        {
            init => fields[nameof(Type)] = value;
            get => fields.GetStructOrDefault<byte>(nameof(Type), 0);
        }

        public uint Signature
        {
            init => fields[nameof(Signature)] = value;
            get => fields.GetStructOrDefault<uint>(nameof(Signature), 0);
        }

        public string TypeName
        {
            init => fields[nameof(TypeName)] = value;
            get => fields.GetClassOrDefault(nameof(TypeName), "Unknown");
        }

        public string SubtypeName
        {
            init => fields[nameof(SubtypeName)] = value;
            get => fields.GetClassOrDefault(nameof(SubtypeName), "Unknown");
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

        protected IList<IShellItem> children = new List<IShellItem>();
        protected Dictionary<string, object> fields = new Dictionary<string, object>();
        protected IReadOnlyCollection<IExtensionBlock> extensionBlocks = new List<IExtensionBlock>();

        public int CompareTo(IShellItem other)
        {
            return (RegistryHive, Place, Type, Signature).CompareTo((other.RegistryHive, other.Place, other.Type, other.Signature));
        }
    }
}
