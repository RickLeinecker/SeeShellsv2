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
    public class RootFolderShellItemTests
    {
        [TestMethod()]
        public void RootFolderShellItemTest()
        {
            RootFolderShellItem item = new RootFolderShellItem()
            {
                Type = 0x00,
                TypeName = "TestType",
                Description = "Test",
                Size = 10,
                RootFolderGuid = "Hello, World!",
                RootFolderName = "Docs",
                SortIndex = 0xFF,
                SortIndexDescription = "123abc"
            };

            Assert.IsTrue(item.Fields.Count == 8);
            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Type == 0x00);
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.TypeName == "TestType");
            Assert.IsTrue(item.Fields.ContainsKey("Description"));
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);
            Assert.IsTrue(item.Description == "Test");
            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue(item.Size == 10);
            Assert.IsTrue(item.Fields.ContainsKey("SortIndex"));
            Assert.IsTrue(item.Fields["SortIndex"] as byte? == item.SortIndex);
            Assert.IsTrue(item.SortIndex == 0xFF);
            Assert.IsTrue(item.Fields.ContainsKey("SortIndexDescription"));
            Assert.IsTrue(item.Fields["SortIndexDescription"] as string == item.SortIndexDescription);
            Assert.IsTrue(item.SortIndexDescription == "123abc");
            Assert.IsTrue(item.RegistryKey == null);
            Assert.IsTrue(item.Fields.ContainsKey("RootFolderGuid"));
            Assert.IsTrue(item.Fields["RootFolderGuid"] as string == item.RootFolderGuid);
            Assert.IsTrue(item.RootFolderGuid == "Hello, World!");
            Assert.IsTrue(item.Fields.ContainsKey("RootFolderName"));
            Assert.IsTrue(item.Fields["RootFolderName"] as string == item.RootFolderName);
            Assert.IsTrue(item.RootFolderName == "Docs");
        }

        [TestMethod()]
        public void RootFolderShellItemTest1()
        {
            byte[] buf = new byte[] {
                0x14, 0x00, 0x1F, 0x50, 0xE0, 0x4F, 0xD0, 0x20,
                0xEA, 0x3A, 0x69, 0x10, 0xA2, 0xD8, 0x08, 0x00,
                0x2B, 0x30, 0x30, 0x9D, 0x00, 0x00
            };

            RootFolderShellItem item = new RootFolderShellItem(buf);

            Assert.IsTrue(item.Fields.Count == 9);
            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Type == 0x1F);
            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue(item.Size == 0x14);
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.TypeName == "Root Folder");
            Assert.IsTrue(item.Fields.ContainsKey("Description"));
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);
            Assert.IsTrue(item.Description == "My Computer");
            Assert.IsTrue(item.Fields.ContainsKey("RootFolderGuid"));
            Assert.IsTrue(item.Fields["RootFolderGuid"] as string == item.RootFolderGuid);
            Assert.IsTrue(item.RootFolderGuid == "20d04fe0-3aea-1069-a2d8-08002b30309d");
            Assert.IsTrue(item.Fields.ContainsKey("RootFolderName"));
            Assert.IsTrue(item.Fields["RootFolderName"] as string == item.RootFolderName);
            Assert.IsTrue(item.RootFolderName == "My Computer");
            Assert.IsTrue(item.Fields.ContainsKey("SortIndex"));
            Assert.IsTrue(item.Fields["SortIndex"] as byte? == item.SortIndex);
            Assert.IsTrue(item.SortIndex == 0x50);
            Assert.IsTrue(item.Fields.ContainsKey("SortIndexDescription"));
            Assert.IsTrue(item.Fields["SortIndexDescription"] as string == item.SortIndexDescription);
            Assert.IsTrue(item.SortIndexDescription == "MY_COMPUTER");
            Assert.IsTrue(item.RegistryKey == null);
        }

        [TestMethod()]
        public void RootFolderShellItemTest2()
        {
            byte[] buf = new byte[] {
                0x14, 0x00, 0x1F, 0x50, 0xE0, 0x4F, 0xD0, 0x20,
                0xEA, 0x3A, 0x69, 0x10, 0xA2, 0xD8, 0x08, 0x00,
                0x2B, 0x30, 0x30, 0x9D, 0x00, 0x00
            };

            IShellItem item = ShellItem.FromByteArray(buf);

            Assert.IsTrue(item is RootFolderShellItem);
        }
    }
}