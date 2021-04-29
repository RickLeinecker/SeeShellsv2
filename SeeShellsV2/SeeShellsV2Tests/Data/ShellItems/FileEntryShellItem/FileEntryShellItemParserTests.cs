using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShellsV2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data.Tests
{
    [TestClass()]
    public class FileEntryShellItemParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            byte[] buf = new byte[] {
                0x88, 0x00, 0x32, 0x00, 0xC3, 0x52, 0x7D, 0x00,
                0x85, 0x47, 0x1A, 0xA7, 0x20, 0x00, 0x41, 0x54,
                0x4B, 0x5F, 0x50, 0x41, 0x7E, 0x31, 0x2E, 0x5A,
                0x49, 0x50, 0x00, 0x00, 0x6C, 0x00, 0x08, 0x00,
                0x04, 0x00, 0xEF, 0xBE, 0x98, 0x47, 0x94, 0xB9,
                0x98, 0x47, 0x94, 0xB9, 0x2A, 0x00, 0x00, 0x00,
                0x64, 0xE9, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x41, 0x00,
                0x54, 0x00, 0x4B, 0x00, 0x5F, 0x00, 0x50, 0x00,
                0x61, 0x00, 0x63, 0x00, 0x6B, 0x00, 0x61, 0x00,
                0x67, 0x00, 0x65, 0x00, 0x5F, 0x00, 0x77, 0x00,
                0x69, 0x00, 0x6E, 0x00, 0x37, 0x00, 0x5F, 0x00,
                0x33, 0x00, 0x32, 0x00, 0x5F, 0x00, 0x5A, 0x00,
                0x31, 0x00, 0x30, 0x00, 0x30, 0x00, 0x30, 0x00,
                0x30, 0x00, 0x38, 0x00, 0x2E, 0x00, 0x7A, 0x00,
                0x69, 0x00, 0x70, 0x00, 0x00, 0x00, 0x1C, 0x00,
                0x00, 0x00
            };

            FileEntryShellItemParser parser = new FileEntryShellItemParser();

            FileEntryShellItem item = parser.Parse(null, null, buf) as FileEntryShellItem;

            Assert.IsTrue(item.Fields.Count == 16);

            Assert.IsTrue(item.ExtensionBlocks.Count == 1);
            Assert.IsTrue(item.ExtensionBlocks.First().Signature == 0xBEEF0004);

            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("SubtypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("Description"));
            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields.ContainsKey("ModifiedDate"));
            Assert.IsTrue(item.Fields.ContainsKey("AccessedDate"));
            Assert.IsTrue(item.Fields.ContainsKey("CreationDate"));
            Assert.IsTrue(item.Fields.ContainsKey("FileSize"));
            Assert.IsTrue(item.Fields.ContainsKey("FileAttributes"));
            Assert.IsTrue(item.Fields.ContainsKey("Place"));

            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.Fields["SubtypeName"] as string == item.SubtypeName);
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);
            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue(item.Fields["ModifiedDate"] as DateTime? == item.ModifiedDate);
            Assert.IsTrue(item.Fields["AccessedDate"] as DateTime? == item.AccessedDate);
            Assert.IsTrue(item.Fields["CreationDate"] as DateTime? == item.CreationDate);
            Assert.IsTrue(item.Fields["FileSize"] as uint? == item.FileSize);
            Assert.IsTrue((FileEntryShellItem.FileAttributeFlags)item.Fields["FileAttributes"] == item.FileAttributes);
            Assert.IsTrue(item.Fields["Place"] as Place == item.Place);

            Assert.IsTrue(item.Type == 0x32);
            Assert.IsTrue(item.TypeName == "File Entry");
            Assert.IsTrue(item.SubtypeName == "File");
            Assert.IsTrue(item.Description == "ATK_Package_win7_32_Z100008.zip");
            Assert.IsTrue(item.Size == 136);
            Assert.IsTrue(item.ModifiedDate == new DateTime(2015, 12, 5, 20, 56, 52));
            Assert.IsTrue(item.AccessedDate == new DateTime(2015, 12, 24, 23, 12, 40));
            Assert.IsTrue(item.CreationDate == new DateTime(2015, 12, 24, 23, 12, 40));
            Assert.IsTrue(item.FileSize == 8213187);
            Assert.IsTrue(item.FileAttributes == FileEntryShellItem.FileAttributeFlags.FILE_ATTRIBUTE_ARCHIVE);
            Assert.IsTrue(item.Place.Name == "ATK_Package_win7_32_Z100008.zip");
        }
    }
}