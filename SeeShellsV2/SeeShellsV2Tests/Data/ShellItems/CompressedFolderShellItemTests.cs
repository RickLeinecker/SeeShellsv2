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
    public class CompressedFolderShellItemTests
    {
        [TestMethod()]
        public void CompressedFolderShellItemTest()
        {
            CompressedFolderShellItem item = new CompressedFolderShellItem()
            {
                Type = 0x00,
                TypeName = "TestType",
                SubtypeName = "123",
                Description = "Test",
                Size = 10,
                FileName = "file.zip",
                PathName = "C:\\",
                ModifiedDate = new DateTime(129874)
            };

            Assert.IsTrue(item.Fields.Count == 8);
            Assert.IsTrue(item.RegistryKey == null);

            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("SubtypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("Description"));
            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields.ContainsKey("FileName"));
            Assert.IsTrue(item.Fields.ContainsKey("PathName"));
            Assert.IsTrue(item.Fields.ContainsKey("ModifiedDate"));

            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.Fields["SubtypeName"] as string == item.SubtypeName);
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);
            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue(item.Fields["FileName"] as string == item.FileName);
            Assert.IsTrue(item.Fields["PathName"] as string == item.PathName);
            Assert.IsTrue(item.Fields["ModifiedDate"] as DateTime? == item.ModifiedDate);

            Assert.IsTrue(item.Type == 0x00);
            Assert.IsTrue(item.TypeName == "TestType");
            Assert.IsTrue(item.SubtypeName == "123");
            Assert.IsTrue(item.Description == "Test");
            Assert.IsTrue(item.Size == 10);
            Assert.IsTrue(item.FileName == "file.zip");
            Assert.IsTrue(item.PathName == "C:\\");
            Assert.IsTrue(item.ModifiedDate == new DateTime(129874));
        }

        [TestMethod()]
        public void CompressedFolderShellItemTest1()
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

            CompressedFolderShellItem item = new CompressedFolderShellItem(buf);

            Assert.IsTrue(item.Fields.Count == 7);

            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("FileName"));
            Assert.IsTrue(item.Fields.ContainsKey("PathName"));
            Assert.IsTrue(item.Fields.ContainsKey("ModifiedDate"));
            Assert.IsTrue(item.Fields.ContainsKey("Description"));

            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.Fields["FileName"] as string == item.FileName);
            Assert.IsTrue(item.Fields["PathName"] as string == item.PathName);
            Assert.IsTrue(item.Fields["ModifiedDate"] as DateTime? == item.ModifiedDate);
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);

            Assert.IsTrue(item.Size == 122);
            Assert.IsTrue(item.Type == 0x7E);
            Assert.IsTrue(item.TypeName == "Compressed Folder");
            Assert.IsTrue(item.FileName == "LAMP");
            Assert.IsTrue(item.PathName == "stacks/");
            Assert.IsTrue(item.ModifiedDate == new DateTime(2019, 12, 11, 14, 23, 06));
            Assert.IsTrue(item.Description == item.FileName);
        }
    }
}