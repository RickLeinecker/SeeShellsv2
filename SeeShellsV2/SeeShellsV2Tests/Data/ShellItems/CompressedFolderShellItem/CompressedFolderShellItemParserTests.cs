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
    public class CompressedFolderShellItemParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            byte[] buf = new byte[] {
                0x7A, 0x00, 0x7E, 0x00, 0x4D, 0x00, 0x49, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x01, 0x00, 0x31, 0x00, 0x32, 0x00,
                0x2F, 0x00, 0x31, 0x00, 0x31, 0x00, 0x2F, 0x00,
                0x32, 0x00, 0x30, 0x00, 0x31, 0x00, 0x39, 0x00,
                0x20, 0x00, 0x20, 0x00, 0x31, 0x00, 0x34, 0x00,
                0x3A, 0x00, 0x32, 0x00, 0x33, 0x00, 0x3A, 0x00,
                0x30, 0x00, 0x36, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
                0x07, 0x00, 0x00, 0x00, 0x4C, 0x00, 0x41, 0x00,
                0x4D, 0x00, 0x50, 0x00, 0x00, 0x00, 0x73, 0x00,
                0x74, 0x00, 0x61, 0x00, 0x63, 0x00, 0x6B, 0x00,
                0x73, 0x00, 0x2F, 0x00, 0x00, 0x00, 0x62, 0x65,
                0x72, 0x3A, 0x00, 0x00
            };

            FileEntryShellItem parent = new FileEntryShellItem()
            {
                Place = new Place() { Name = "C:\\", PathName = "" },
                FileAttributes = FileEntryShellItem.FileAttributeFlags.FILE_ATTRIBUTE_ARCHIVE
            };

            CompressedFolderShellItemParser parser = new CompressedFolderShellItemParser();
            CompressedFolderShellItem item = parser.Parse(null, null, buf, parent) as CompressedFolderShellItem;

            Assert.IsTrue(item.Fields.Count == 11);

            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("Place"));
            Assert.IsTrue(item.Fields.ContainsKey("ModifiedDate"));
            Assert.IsTrue(item.Fields.ContainsKey("Description"));
            Assert.IsTrue(item.Fields.ContainsKey("Value"));

            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.Fields["Place"] as Place == item.Place);
            Assert.IsTrue(item.Fields["ModifiedDate"] as DateTime? == item.ModifiedDate);
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);
            Assert.IsTrue(item.Fields["Value"] as byte[] == item.Value);

            Assert.IsTrue(item.Size == 122);
            Assert.IsTrue(item.Type == 0x7E);
            Assert.IsTrue(item.TypeName == "Compressed Folder");
            Assert.IsTrue(item.Place.Name == "LAMP");
            Assert.IsTrue(item.Place.PathName == "C:\\");
            Assert.IsTrue(item.ModifiedDate == new DateTime(2019, 12, 11, 14, 23, 06));
            Assert.IsTrue(item.Description == "LAMP");
            Assert.IsTrue(item.Value == buf);
        }
    }
}