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
    public class ShellItemTests
    {
        [TestMethod()]
        public void ShellItemTest()
        {
            IShellItem block = new ShellItem()
            {
                Type = 0x00,
                TypeName = "TestType",
                Name = "Test",
                Size = 10,
                ModifiedDate = new DateTime(90810298),
                AccessedDate = new DateTime(908123802123),
                CreationDate = new DateTime(4865846)
            };

            Assert.IsTrue(block.Fields.Count == 7);
            Assert.IsTrue(block.Fields.ContainsKey("Type"));
            Assert.IsTrue(block.Fields["Type"] as byte? == block.Type);
            Assert.IsTrue(block.Type == 0x00);
            Assert.IsTrue(block.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(block.Fields["TypeName"] as string == block.TypeName);
            Assert.IsTrue(block.TypeName == "TestType");
            Assert.IsTrue(block.Fields.ContainsKey("Name"));
            Assert.IsTrue(block.Fields["Name"] as string == block.Name);
            Assert.IsTrue(block.Name == "Test");
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == 10);
            Assert.IsTrue(block.Fields.ContainsKey("ModifiedDate"));
            Assert.IsTrue(block.Fields["ModifiedDate"] as DateTime? == block.ModifiedDate);
            Assert.IsTrue(block.ModifiedDate == new DateTime(90810298));
            Assert.IsTrue(block.Fields.ContainsKey("AccessedDate"));
            Assert.IsTrue(block.Fields["AccessedDate"] as DateTime? == block.AccessedDate);
            Assert.IsTrue(block.AccessedDate == new DateTime(908123802123));
            Assert.IsTrue(block.Fields.ContainsKey("CreationDate"));
            Assert.IsTrue(block.Fields["CreationDate"] as DateTime? == block.CreationDate);
            Assert.IsTrue(block.CreationDate == new DateTime(4865846));
        }

        [TestMethod()]
        public void ShellItemTest1()
        {
            byte[] buf = new byte[] { 0x11, 0x01, 0x99 };

            IShellItem block = new ShellItem(buf);

            Assert.IsTrue(block.Fields.Count == 2);
            Assert.IsTrue(block.Fields.ContainsKey("Type"));
            Assert.IsTrue(block.Fields["Type"] as byte? == block.Type);
            Assert.IsTrue(block.Type == 0x99);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == 273);
            Assert.IsTrue(block.TypeName == "Unknown");
            Assert.IsTrue(block.Name == "??");
            Assert.IsTrue(block.ModifiedDate == DateTime.MinValue);
            Assert.IsTrue(block.AccessedDate == DateTime.MinValue);
            Assert.IsTrue(block.CreationDate == DateTime.MinValue);
        }
    }
}