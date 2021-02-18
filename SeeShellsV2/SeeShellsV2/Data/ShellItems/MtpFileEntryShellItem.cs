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
    public class MtpFileEntryShellItem : ShellItem, IShellItem
    {
        public static IReadOnlySet<uint> KnownSignatures = new HashSet<uint> {
            0x07192006
        };

        public string FolderName
        {
            init => fields["FolderName"] = value;
            get => fields.GetClassOrDefault("FolderName", string.Empty);
        }
        
        public string FolderId
        {
            init => fields["FolderId"] = value;
            get => fields.GetClassOrDefault("FolderId", string.Empty);
        }

        public DateTime ModifiedDate
        {
            init => fields["ModifiedDate"] = value;
            get => fields.GetStructOrDefault("ModifiedDate", DateTime.MinValue);
        }

        public DateTime CreationDate
        {
            init => fields["CreationDate"] = value;
            get => fields.GetStructOrDefault("CreationDate", DateTime.MinValue);
        }

        public MtpFileEntryShellItem() { }

        public MtpFileEntryShellItem(byte[] buf)
        {
            try
            {
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Signature"] = Block.UnpackDWord(buf, 0x06);

                fields["TypeName"] = "Media Transfer Protocol";
                fields["SubtypeName"] = "File Entry";

                fields["ModifiedDate"] = Block.UnpackFileTime(buf, 0x1A);
                fields["CreationDate"] = Block.UnpackFileTime(buf, 0x22);

                int offset = 0x4A;
                fields["FolderName"] = Block.UnpackWString(buf, offset);
                offset += 2 * (FolderName.Length + 1);

                if (FolderName == string.Empty)
                {
                    fields["FolderName"] = Block.UnpackWString(buf, offset);
                    offset += 2 * (FolderName.Length + 1);
                }

                fields["FolderId"] = Block.UnpackWString(buf, offset);

                fields["Description"] = FolderName;
            }
            catch (ShellParserException ex)
            {
                throw new ArgumentException("byte array could not be parsed into MtpFileEntryShellItem", ex);
            }
        }
    }
}
