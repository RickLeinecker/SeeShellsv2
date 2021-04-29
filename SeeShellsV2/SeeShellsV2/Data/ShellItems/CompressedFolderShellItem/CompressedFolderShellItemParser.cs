using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class CompressedFolderShellItemParser : IShellItemParser
    {
        public Type ShellItemType { get => typeof(CompressedFolderShellItem); }

        public int Priority { get => 10; }

        public bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            FileEntryShellItem fileEntryParent = parent as FileEntryShellItem;
            FileEntryShellItem.FileAttributeFlags compressed = FileEntryShellItem.FileAttributeFlags.FILE_ATTRIBUTE_COMPRESSED | FileEntryShellItem.FileAttributeFlags.FILE_ATTRIBUTE_ARCHIVE;
            return parent is CompressedFolderShellItem || (fileEntryParent != null && (fileEntryParent.FileAttributes & compressed) != 0);
        }

        public IShellItem Parse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            if (!CanParse(hive, keyWrapper, value, parent))
                return null;

            try
            {
                // based soley on two examples found in Windows 10 (seen in unit tests for this file)
                ushort size = BlockHelper.UnpackWord(value, 0x00);
                byte type = BlockHelper.UnpackByte(value, 0x02);

                string typename = "Compressed Folder";

                int offset = 36; // unknown fields

                var date = BlockHelper.UnpackWString(value, offset);
                DateTime modified = date == "N/A" ? DateTime.MinValue : DateTime.Parse(date);
                offset += 42; // date string size

                offset = BlockHelper.AlignTo(0, offset, 4);
                offset += 12;

                string filename = BlockHelper.UnpackWString(value, offset);
                offset += 2 * (filename.Length + 1); // file name string size

                string pathname = BlockHelper.UnpackWString(value, offset);
                offset += 2 * (pathname.Length + 1); // path name string size

                IShellItem retval = new CompressedFolderShellItem()
                {
                    Size = size,
                    Type = type,
                    TypeName = typename,
                    ModifiedDate = modified,
                    Place = new CompressedFolder()
                    {
                        Name = filename,
                        PathName = parent != null ? Path.Join(parent.Place?.PathName ?? string.Empty, parent.Place?.Name ?? string.Empty) : null,
                    },
                    RegistryHive = hive,
                    Value = value,
                    NodeSlot = keyWrapper?.NodeSlot,
                    SlotModifiedDate = keyWrapper?.SlotModifiedDate,
                    LastRegistryWriteDate = keyWrapper?.LastRegistryWriteDate ?? DateTime.MinValue,
                    Description = filename,
                    Parent = parent
                };

                parent?.Children.Add(retval);
                return retval;
            }
            catch (Exception ex) when (ex is ShellParserException || ex is FormatException)
            {
                return null;
            }
        }
    }
}
