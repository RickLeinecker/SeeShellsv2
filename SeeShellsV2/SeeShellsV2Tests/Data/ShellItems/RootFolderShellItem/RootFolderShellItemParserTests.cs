using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShellsV2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Repositories;

namespace SeeShellsV2.Data.Tests
{
    [TestClass()]
    public class RootFolderShellItemParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            byte[] buf = new byte[] {
                0x14, 0x00, 0x1F, 0x50, 0xE0, 0x4F, 0xD0, 0x20,
                0xEA, 0x3A, 0x69, 0x10, 0xA2, 0xD8, 0x08, 0x00,
                0x2B, 0x30, 0x30, 0x9D, 0x00, 0x00
            };

            RootFolderShellItemParser parser = new RootFolderShellItemParser(new Config { KnownGuids = new Dictionary<string, string>() { { "20d04fe0-3aea-1069-a2d8-08002b30309d", "My Computer" } } });

            RootFolderShellItem item = parser.Parse(null, null, buf) as RootFolderShellItem;

            Assert.IsTrue(item.Fields.Count == 15);
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
            Assert.IsTrue(item.Fields.ContainsKey("Place"));
            Assert.IsTrue(item.Fields["Place"] as Place == item.Place);
            Assert.IsTrue(item.Place.Name == "My Computer");
            Assert.IsTrue(item.Fields.ContainsKey("SortIndex"));
            Assert.IsTrue(item.Fields["SortIndex"] as byte? == item.SortIndex);
            Assert.IsTrue(item.SortIndex == 0x50);
            Assert.IsTrue(item.Fields.ContainsKey("SortIndexDescription"));
            Assert.IsTrue(item.Fields["SortIndexDescription"] as string == item.SortIndexDescription);
            Assert.IsTrue(item.SortIndexDescription == "MY_COMPUTER");
        }
    }
}