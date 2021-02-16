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
    public class CompressedFolderShellItem : ShellItem, IShellItem
    {
        public string FileName
        {
            init => fields["FileName"] = value;
            get => fields.GetClassOrDefault("FileName", string.Empty);
        }

        public string PathName
        {
            init => fields["PathName"] = value;
            get => fields.GetClassOrDefault("PathName", string.Empty);
        }

        public CompressedFolderShellItem() { }

        public CompressedFolderShellItem(byte[] buf)
        {
            try
            {
                // based soley on two examples found in Windows 10 (seen in unit tests for this file)
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Type"] = Block.UnpackByte(buf, 0x02);

                fields["TypeName"] = "Compressed Folder";

                int offset = 36; // unknown fields

                var date = Block.UnpackWString(buf, offset);
                fields["ModifiedDate"] = date == "N/A" ? DateTime.MinValue : DateTime.Parse(date);
                offset += 42; // date string size

                offset = Block.AlignTo(0, offset, 4);
                offset += 12;

                fields["FileName"] = Block.UnpackWString(buf, offset);
                offset += 2 * (FileName.Length + 1); // file name string size

                fields["PathName"] = Block.UnpackWString(buf, offset);
                offset += 2 * (PathName.Length + 1); // path name string size

                fields["Description"] = PathName + FileName;
            }
            catch (Exception ex) when (ex is ShellParserException || ex is FormatException)
            {
                throw new ArgumentException("byte array could not be parsed into CompressedFolderShellItem", ex);
            }
        }
    }
}
