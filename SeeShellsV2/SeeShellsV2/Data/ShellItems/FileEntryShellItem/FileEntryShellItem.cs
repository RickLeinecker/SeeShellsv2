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
    public class FileEntryShellItem : ShellItem, IShellItem, ICreationTimestamp, IAccessedTimestamp, IModifiedTimestamp
    {
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
            init => fields[nameof(FileSize)] = value;
            get => fields.GetStructOrDefault<uint>(nameof(FileSize), 0);
        }

        public FileAttributeFlags FileAttributes
        {
            init => fields[nameof(FileAttributes)] = (int) value;
            get => (FileAttributeFlags) fields.GetStructOrDefault<int>(nameof(FileAttributes), 0);
        }

        public DateTime ModifiedDate
        {
            init => fields[nameof(ModifiedDate)] = value;
            get => fields.GetStructOrDefault(nameof(ModifiedDate), DateTime.MinValue);
        }

        public DateTime AccessedDate
        {
            init => fields[nameof(AccessedDate)] = value;
            get => fields.GetStructOrDefault(nameof(AccessedDate), DateTime.MinValue);
        }

        public DateTime CreationDate
        {
            init => fields[nameof(CreationDate)] = value;
            get => fields.GetStructOrDefault(nameof(CreationDate), DateTime.MinValue);
        }
    }
}