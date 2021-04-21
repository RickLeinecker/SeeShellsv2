using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Shell items that document compressed folders:
    /// 0x52
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#36-compressed-folder-shell-item
    public class CompressedFolderShellItem : ShellItem, IShellItem, IModifiedTimestamp
    {
        public DateTime ModifiedDate
        {
            init => fields[nameof(ModifiedDate)] = value;
            get => fields.GetStructOrDefault(nameof(ModifiedDate), DateTime.MinValue);
        }
    }
}
