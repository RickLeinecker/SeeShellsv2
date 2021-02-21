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
using System.Reflection;

namespace SeeShellsV2.Data
{
    public class ShellItem : IShellItem
    {
        public string Description
        {
            init => fields["Description"] = value;
            get => fields.GetClassOrDefault("Description", "??");
        }

        public RegistryKeyWrapper RegistryKey
        {
            set => fields["RegistryKey"] = value;
            get => fields.GetClassOrDefault<RegistryKeyWrapper>("RegistryKey", null);
        }

        public IShellItem Parent
        {
            set => parent = value;
            get => parent;
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

        public IReadOnlyList<IExtensionBlock> ExtensionBlocks
        {
            get => extensionBlocks;
        }

        public ISet<IShellTag> Tags
        {
            get => tags;
        }

        /// <summary>
        /// Create a new shell item from a byte array
        /// </summary>
        /// <param name="buf">the byte array containing shell item data</param>
        /// <returns>a new shell item instance if the buffer can be parsed or null otherwise</returns>
        public static IShellItem FromByteArray(byte[] buf, IShellItem parent = null)
        {
            // attempt to deduce type by parent type
            try
            {
                FileEntryShellItem fileEntryParent = parent as FileEntryShellItem;
                FileEntryShellItem.FileAttributeFlags compressed = FileEntryShellItem.FileAttributeFlags.FILE_ATTRIBUTE_COMPRESSED | FileEntryShellItem.FileAttributeFlags.FILE_ATTRIBUTE_ARCHIVE;

                if (parent is CompressedFolderShellItem || (fileEntryParent != null && (fileEntryParent.FileAttributes & compressed) != 0))
                {
                    return new CompressedFolderShellItem(buf);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ShellParserException)
            { }

            // attempt to deduce type by shell signature (uint located at offset 0x06)
            try
            {
                uint signature = Block.UnpackDWord(buf, 0x06);

                if (MtpDeviceShellItem.KnownSignatures.Contains(signature))
                {
                    return new MtpDeviceShellItem(buf);
                }
                else if (MtpVolumeShellItem.KnownSignatures.Contains(signature))
                {
                    return new MtpVolumeShellItem(buf);
                }
                else if (MtpFileEntryShellItem.KnownSignatures.Contains(signature))
                {
                    return new MtpFileEntryShellItem(buf);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ShellParserException)
            { }

            // Attempt to deduce type by type field (byte located at offset 0x02)
            try
            {
                byte type = Block.UnpackByte(buf, 0x02);

                switch (type & 0x70) // 0x70 is the assumed mask for the shell class type
                {
                    case 0x10 when type == 0x1F:
                        return new RootFolderShellItem(buf);
                    case 0x20 when VolumeShellItem.KnownTypes.Contains(type):
                        return new VolumeShellItem(buf);
                    case 0x30 when FileEntryShellItem.KnownTypes.Contains(type):
                        return new FileEntryShellItem(buf);
                    case 0x40 when NetworkShellItem.KnownTypes.Contains(type):
                        return new NetworkShellItem(buf);
                    case 0x60 when type == 0x61:
                        return new UriShellItem(buf);
                    default:
                        break;
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ShellParserException)
            { }

            return null;
        }

        /// <summary>
        /// Create a new shell item from a byte array
        /// </summary>
        /// <param name="buf">the byte array containing shell item data</param>
        /// <returns>a new shell item instance if the buffer can be parsed or null otherwise</returns>
        public static Type GetShellType(byte type, IShellItem parent = null)
        {
            switch (type & 0x70) // 0x70 is the assumed mask for the shell class type
            {
                case 0x10 when type == 0x1F:
                    return typeof(RootFolderShellItem);
                case 0x20 when VolumeShellItem.KnownTypes.Contains(type):
                    return typeof(VolumeShellItem);
                case 0x30 when FileEntryShellItem.KnownTypes.Contains(type):
                    return typeof(FileEntryShellItem);
                case 0x40 when NetworkShellItem.KnownTypes.Contains(type):
                    return typeof(NetworkShellItem);
                case 0x60 when type == 0x61:
                    return typeof(UriShellItem);
                default:
                    break;
            }

            FileEntryShellItem fileEntryParent = parent as FileEntryShellItem;
            FileEntryShellItem.FileAttributeFlags compressed = FileEntryShellItem.FileAttributeFlags.FILE_ATTRIBUTE_COMPRESSED | FileEntryShellItem.FileAttributeFlags.FILE_ATTRIBUTE_ARCHIVE;

            if (fileEntryParent != null && (fileEntryParent.FileAttributes & compressed) != 0)
            {
                return typeof(CompressedFolderShellItem);
            }

            return null;
        }

        protected IShellItem parent = null;
        protected List<IShellItem> children = new List<IShellItem>();
        protected Dictionary<string, object> fields = new Dictionary<string, object>();
        protected List<IExtensionBlock> extensionBlocks = new List<IExtensionBlock>();
        protected SortedSet<IShellTag> tags = new SortedSet<IShellTag>();
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
