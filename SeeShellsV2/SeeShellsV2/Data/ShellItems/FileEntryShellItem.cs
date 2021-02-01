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

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Shell items that document file system entries:
    /// 0x30, 0x31, 0x32, 0x35, 0x36, 0xB1
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#file_entry_shell_item
    public class FileEntryShellItem : ShellItem, IShellItem
    {
        public static IReadOnlySet<byte> KnownTypes = new HashSet<byte> {
        /*  0x30, 0x31, 0x32, 0x35, 0x36, 0xB1  */   // libwsi
            0x30, 0x31, 0x32, 0x34, 0x35, 0x36, 0xB1 // v1 team
        };

        [Flags]
        public enum SubtypeFlags
        {
            None = 0x00,
            IsDirectory = 0x01,
            IsFile = 0x02,
            HasUnicodeStrings = 0x04,
            Unknown = 0x08,
            HasClassIdentifier = 0x80
        }

        [Flags]
        public enum FileAttributeFlags
        {
            FILE_ATTRIBUTE_READONLY            = 0x00000001, // Is read-Only
            FILE_ATTRIBUTE_HIDDEN              = 0x00000002, // Is hidden
            FILE_ATTRIBUTE_SYSTEM              = 0x00000004, // Is a system file or directory
            FILE_ATTRIBUTE_VOLUME              = 0x00000008, // Is a volume label
            FILE_ATTRIBUTE_DIRECTORY           = 0x00000010, // Is a directory
            FILE_ATTRIBUTE_ARCHIVE             = 0x00000020, // Should be archived
            FILE_ATTRIBUTE_DEVICE              = 0x00000040, // Is a device
            FILE_ATTRIBUTE_NORMAL              = 0x00000080, // Is normal; None of the other flags should be set
            FILE_ATTRIBUTE_TEMPORARY           = 0x00000100, // Is temporary
            FILE_ATTRIBUTE_SPARSE_FILE         = 0x00000200, // Is a sparse file
            FILE_ATTRIBUTE_REPARSE_POINT       = 0x00000400, // Is a reparse point or symbolic link
            FILE_ATTRIBUTE_COMPRESSED          = 0x00000800, // Is compressed
            FILE_ATTRIBUTE_OFFLINE             = 0x00001000, // Is offline; The data of the file is stored on an offline storage.
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000, // Do not index content; The content of the file or directory should not be indexed by the indexing service.
            FILE_ATTRIBUTE_ENCRYPTED           = 0x00004000, // Is encrypted
            FILE_ATTRIBUTE_INTEGRITY_STREAM    = 0x00008000, // The directory or user data stream is configured with integrity (only supported on ReFS volumes).
            FILE_ATTRIBUTE_VIRTUAL             = 0x00010000, // Is virtual
            FILE_ATTRIBUTE_NO_SCRUB_DATA       = 0x00020000, // The user data stream not to be read by the background data integrity scanner (AKA scrubber).
        }

        public uint FileSize
        {
            init => fields["FileSize"] = value;
            get => fields.GetStructOrDefault<uint>("FileSize", 0);
        }

        public FileAttributeFlags FileAttributes
        {
            init => fields["FileAttributes"] = (int) value;
            get => (FileAttributeFlags) fields.GetStructOrDefault<int>("FileAttributes", 0);
        }

        public string FilePrimaryName
        {
            init => fields["FilePrimaryName"] = value;
            get => fields.GetClassOrDefault("FilePrimaryName", string.Empty);
        }

        public FileEntryShellItem() { }

        public FileEntryShellItem(byte[] buf)
        {
            try
            {
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Type"] = Block.UnpackByte(buf, 0x02);

                fields["TypeName"] = "File Entry";

                if (((SubtypeFlags)Type & SubtypeFlags.IsDirectory) != SubtypeFlags.None)
                    fields["SubtypeName"] = "Directory";
                else if (((SubtypeFlags)Type & SubtypeFlags.IsFile) != SubtypeFlags.None)
                    fields["SubtypeName"] = "File";

                int offset = 0x04;

                fields["FileSize"] = Block.UnpackDWord(buf, offset);
                offset += 4;
                fields["ModifiedDate"] = Block.UnpackDosDateTime(buf, offset);
                offset += 4;
                fields["FileAttributes"] = (int)Block.UnpackWord(buf, offset);
                offset += 2;

                if (((SubtypeFlags)Type & SubtypeFlags.HasUnicodeStrings) != SubtypeFlags.None)
                    fields["FilePrimaryName"] = Block.UnpackWString(buf, offset);
                else
                    fields["FilePrimaryName"] = Block.UnpackString(buf, offset);

                fields["Description"] = FilePrimaryName;

                // Peek at the end of the shell item to get the offset of the first extension block.
                // if the block exists and has a valid signature, the shell item is >=Windows XP
                // otherwise the shell item is <Windows XP and has no extension blocks
                ushort extensionOffset = Block.UnpackWord(buf, Size - 2);
                if (Block.UnpackDWord(buf, extensionOffset + 4) == 0xBEEF0004)
                {
                    // the shell item contains extension blocks

                    ExtensionBlockBEEF0004 extensionBlock = new ExtensionBlockBEEF0004(buf, extensionOffset);
                    extensionBlocks.Add(extensionBlock);

                    fields["Description"] = extensionBlock.LongName;
                    fields["CreationDate"] = extensionBlock.CreationDate;
                    fields["AccessedDate"] = extensionBlock.AccessedDate;

                    // TODO (Devon): implement additional extension blocks
                    // 0xBEEF0005, 0xBEEF0006, 0xBEEF001A, 0xBEEF0003
                }
            }
            catch (ShellParserException ex)
            {
                throw new ArgumentException("byte array could not be parsed into FileEntryShellItem", ex);
            }
        }
    }
}