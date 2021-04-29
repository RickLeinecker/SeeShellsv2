using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Shell item that documents Media Transfer Protocol volumes:
    /// 0x10312005
    /// </summary>
    /// https://digital-forensics.sans.org/summit-archives/dfir14/USB_Devices_and_Media_Transfer_Protocol_Nicole_Ibrahim.pdf
    public class MtpVolumeShellItem : ShellItem, IShellItem
    {
        public string StorageId
        {
            init => fields[nameof(StorageId)] = value;
            get => fields.GetClassOrDefault(nameof(StorageId), string.Empty);
        }

        public string FileSystemName
        {
            init => fields[nameof(FileSystemName)] = value;
            get => fields.GetClassOrDefault(nameof(FileSystemName), string.Empty);
        }
    }
}
