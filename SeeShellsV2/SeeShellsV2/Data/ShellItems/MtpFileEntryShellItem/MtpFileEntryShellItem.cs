using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Shell item that documents Media Transfer Protocol file entries:
    /// 0x07192006
    /// </summary>
    /// https://digital-forensics.sans.org/summit-archives/dfir14/USB_Devices_and_Media_Transfer_Protocol_Nicole_Ibrahim.pdf
    public class MtpFileEntryShellItem : ShellItem, IShellItem //, ICreationTimestamp, IModifiedTimestamp
    {
        public string FolderId
        {
            init => fields[nameof(FolderId)] = value;
            get => fields.GetClassOrDefault(nameof(FolderId), string.Empty);
        }
    }
}
