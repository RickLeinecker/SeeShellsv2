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
    public class ShellItem : IShellItem
    {
        public byte Type
        {
            init => fields["Type"] = value;
            get => fields.GetStructOrDefault<byte>("Type", 0);
        }

        public string TypeName
        {
            init => fields["TypeName"] = value;
            get => fields.GetClassOrDefault("TypeName", "Unknown");
        }

        public string Name
        {
            init => fields["Name"] = value;
            get => fields.GetClassOrDefault("Name", "??");
        }

        public ushort Size
        {
            init => fields["Size"] = value;
            get => fields.GetStructOrDefault<ushort>("Size", 0);
        }

        public DateTime ModifiedDate
        {
            init => fields["ModifiedDate"] = value;
            get => fields.GetStructOrDefault("ModifiedDate", DateTime.MinValue);
        }

        public DateTime AccessedDate
        {
            init => fields["AccessedDate"] = value;
            get => fields.GetStructOrDefault("AccessedDate", DateTime.MinValue);
        }

        public DateTime CreationDate
        {
            init => fields["CreationDate"] = value;
            get => fields.GetStructOrDefault("CreationDate", DateTime.MinValue);
        }

        public IReadOnlyDictionary<string, object> Fields
        {
            get => fields;
        }

        public IReadOnlyList<IExtensionBlock> ExtensionBlocks
        {
            get => extensionBlocks;
        }

        public ISet<string> Tags
        {
            get => tags;
        }

        public ShellItem() { }

        public ShellItem(byte[] buf)
        {
            fields["Type"] = Block.unpack_byte(buf, 0x02);
            fields["Size"] = Block.unpack_word(buf, 0x00);
        }

        protected Dictionary<string, object> fields = new Dictionary<string, object>();
        protected List<IExtensionBlock> extensionBlocks = new List<IExtensionBlock>();
        protected SortedSet<string> tags = new SortedSet<string>();
    }

    internal static class Utilities
    {
        public static T GetClassOrDefault<T>(this IDictionary<string, object> dict, string key, T defaultValue) where T : class
        {
            return dict.ContainsKey(key) ? dict[key] as T ?? defaultValue : defaultValue;
        }

        public static T GetStructOrDefault<T>(this IDictionary<string, object> dict, string key, T defaultValue) where T : struct
        {
            return dict.ContainsKey(key) ? dict[key] as T? ?? defaultValue : defaultValue;
        }
    }
}
