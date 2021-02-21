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
        public static IReadOnlySet<uint> KnownSignatures = new HashSet<uint> {
            0x08312006
        };

        public string DeviceName
        {
            init => fields["DeviceName"] = value;
            get => fields.GetClassOrDefault("DeviceName", string.Empty);
        }

        public string DevicePath
        {
            init => fields["DevicePath"] = value;
            get => fields.GetClassOrDefault("DevicePath", string.Empty);
        }

        public MtpDeviceShellItem() { }

        public MtpDeviceShellItem(byte[] buf)
        {
            try
            {
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Signature"] = Block.UnpackDWord(buf, 0x06);

                fields["TypeName"] = "Media Transfer Protocol";
                fields["SubtypeName"] = "Device";

                int offset = 40;
                fields["DeviceName"] = Block.UnpackWString(buf, offset);
                offset += 2 * (DeviceName.Length + 1);
                fields["DevicePath"] = Block.UnpackWString(buf, offset);

                fields["Description"] = DeviceName;
            }
            catch (ShellParserException ex)
            {
                throw new ArgumentException("byte array could not be parsed into MtpDeviceShellItem", ex);
            }
        }
    }
}
