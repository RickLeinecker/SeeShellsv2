using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class FileEntryShellItemParser : IShellItemParser
    {
        private IReadOnlySet<byte> KnownTypes = new HashSet<byte> {
            0x30, 0x31, 0x32, 0x34, 0x35, 0x36, 0x39, 0xB1
        };

        public Type ShellItemType { get => typeof(FileEntryShellItem); }

        public int Priority { get => 2; }

        public bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            try
            {
                byte type = BlockHelper.UnpackByte(value, 0x02);
                return (type & 0x70) == 0x30 && KnownTypes.Contains(type) && !(parent is CompressedFolderShellItem);
            }
            catch
            {
                return false;
            }
        }

        public IShellItem Parse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            if (!CanParse(hive, keyWrapper, value, parent))
                return null;

            try
            {
                ushort size = BlockHelper.UnpackWord(value, 0x00);
                byte type = BlockHelper.UnpackByte(value, 0x02);

                string typename = "File Entry";
                string subtypename = string.Empty;

                if (((FileEntryShellItem.SubtypeFlags)type & FileEntryShellItem.SubtypeFlags.IsDirectory) != FileEntryShellItem.SubtypeFlags.None)
                    subtypename = "Directory";
                else if (((FileEntryShellItem.SubtypeFlags)type & FileEntryShellItem.SubtypeFlags.IsFile) != FileEntryShellItem.SubtypeFlags.None)
                    subtypename = "File";

                int offset = 0x04;

                uint filesize = BlockHelper.UnpackDWord(value, offset);
                offset += 4;
                DateTime modified = BlockHelper.UnpackDosDateTime(value, offset);
                offset += 4;
                int fileattributes = BlockHelper.UnpackWord(value, offset);
                offset += 2;

                string filePrimaryName = string.Empty;
                if (((FileEntryShellItem.SubtypeFlags)type & FileEntryShellItem.SubtypeFlags.HasUnicodeStrings) != FileEntryShellItem.SubtypeFlags.None)
                    filePrimaryName = BlockHelper.UnpackWString(value, offset);
                else
                    filePrimaryName = BlockHelper.UnpackString(value, offset);

                // Peek at the end of the shell item to get the offset of the first extension block.
                // if the block exists and has a valid signature, the shell item is >=Windows XP
                // otherwise the shell item is <Windows XP and has no extension blocks
                ExtensionBlockBEEF0004 extensionBlock = null;
                ushort extensionOffset = BlockHelper.UnpackWord(value, size - 2);
                if (BlockHelper.UnpackDWord(value, extensionOffset + 4) == 0xBEEF0004)
                    extensionBlock = new ExtensionBlockBEEF0004(value, extensionOffset);

                FileEntryShellItem item = new FileEntryShellItem()
                {
                    Size = size,
                    Type = type,
                    TypeName = typename,
                    SubtypeName = subtypename,
                    FileSize = filesize,
                    ModifiedDate = modified,
                    AccessedDate = extensionBlock?.AccessedDate ?? DateTime.MinValue,
                    CreationDate = extensionBlock?.CreationDate ?? DateTime.MinValue,
                    FileAttributes = (FileEntryShellItem.FileAttributeFlags)fileattributes,
                    Place = new Folder()
                    {
                        Name = extensionBlock?.LongName ?? filePrimaryName,
                        PathName = parent != null ? Path.Join(parent.Place.PathName, parent.Place.Name) : null,
                    },
                    RegistryHive = hive,
                    Value = value,
                    NodeSlot = keyWrapper?.NodeSlot,
                    SlotModifiedDate = keyWrapper?.SlotModifiedDate,
                    LastRegistryWriteDate = keyWrapper?.LastRegistryWriteDate ?? DateTime.MinValue,
                    Description = extensionBlock?.LongName ?? filePrimaryName,
                    Parent = parent,
                    ExtensionBlocks = new List<IExtensionBlock>() { extensionBlock }
                };

                parent?.Children.Add(item);
                return item;
            }
            catch (ShellParserException)
            {
                return null;
            }
        }

    }
}
