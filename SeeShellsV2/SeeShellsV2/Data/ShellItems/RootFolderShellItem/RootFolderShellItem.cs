using System;
using System.Collections.Generic;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Shell items that document windows root folders:
    /// 0x1F
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#32-root-folder-shell-item
    public class RootFolderShellItem : ShellItem, IShellItem
    {
        /// <summary>
        /// Windows GUID string that corresponds to a particular root folder
        /// </summary>
        public string RootFolderGuid
        {
            init => fields[nameof(RootFolderGuid)] = value;
            get => fields.GetClassOrDefault(nameof(RootFolderGuid), string.Empty);
        }

        /// <summary>
        /// Index presumably used for sorting objects in File Exporer
        /// Provides additional information about the shell item
        /// </summary>
        public byte SortIndex
        {
            init => fields[nameof(SortIndex)] = value;
            get => fields.GetStructOrDefault<byte>(nameof(SortIndex), 0xFF);
        }

        /// <summary>
        /// Human readable version of <see cref=SortIndex/>
        /// </summary>
        public string SortIndexDescription
        {
            init => fields[nameof(SortIndexDescription)] = value;
            get => fields.GetClassOrDefault(nameof(SortIndexDescription), "Unknown");
        }
    }
}