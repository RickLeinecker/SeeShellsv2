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
        public static IReadOnlySet<uint> KnownSignatures = new HashSet<uint> {
            0x10312005
        };

        public string StorageName
        {
            init => fields["StorageName"] = value;
            get => fields.GetClassOrDefault("StorageName", string.Empty);
        }

        public string StorageId
        {
            init => fields["StorageId"] = value;
            get => fields.GetClassOrDefault("StorageId", string.Empty);
        }

        public string FileSystemName
        {
            init => fields["FileSystemName"] = value;
            get => fields.GetClassOrDefault("FileSystemName", string.Empty);
        }

        public MtpVolumeShellItem() { }

        public MtpVolumeShellItem(byte[] buf)
        {
            try
            {
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Signature"] = Block.UnpackDWord(buf, 0x06);

                fields["TypeName"] = "Media Transfer Protocol";
                fields["SubtypeName"] = "Volume";

                int offset = 54;
                fields["StorageName"] = Block.UnpackWString(buf, offset);
                offset += 2 * (StorageName.Length + 1);
                fields["StorageId"] = Block.UnpackWString(buf, offset);
                offset += 2 * (StorageId.Length + 1);
                fields["FileSystemName"] = Block.UnpackWString(buf, offset);

                fields["Description"] = StorageName;
            }
            catch (ShellParserException ex)
            {
                throw new ArgumentException("byte array could not be parsed into MtpVolumeShellItem", ex);
            }
        }
    }
}
