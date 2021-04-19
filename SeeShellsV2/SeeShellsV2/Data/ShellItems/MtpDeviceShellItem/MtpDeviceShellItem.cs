using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Shell item that documents Media Transfer Protocol Devices:
    /// 0x08312006
    /// </summary>
    /// https://digital-forensics.sans.org/summit-archives/dfir14/USB_Devices_and_Media_Transfer_Protocol_Nicole_Ibrahim.pdf
    public class MtpDeviceShellItem : ShellItem, IShellItem
    {
        public string InternalDevicePath
        {
            init => fields[nameof(InternalDevicePath)] = value;
            get => fields.GetClassOrDefault(nameof(InternalDevicePath), string.Empty);
        }
    }
}
