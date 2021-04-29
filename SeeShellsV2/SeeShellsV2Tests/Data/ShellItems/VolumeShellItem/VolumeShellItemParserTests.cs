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
    public class VolumeShellItemParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            byte[] buf = new byte[] {
                0x19, 0x00, 0x2F, 0x46, 0x3A, 0x5C, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00
            };

            VolumeShellItemParser parser = new VolumeShellItemParser(new Config());
            VolumeShellItem item = parser.Parse(null, null, buf) as VolumeShellItem;

            Assert.IsTrue(item.Fields.Count == 11);
            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Type == 0x2F);
            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue(item.Size == 0x19);
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.TypeName == "Volume");
            Assert.IsTrue(item.Fields.ContainsKey("SubtypeName"));
            Assert.IsTrue(item.Fields["SubtypeName"] as string == item.SubtypeName);
            Assert.IsTrue(item.SubtypeName == "Local Disk");
            Assert.IsTrue(item.Fields.ContainsKey("Description"));
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);
            Assert.IsTrue(item.Description == "F:\\");
            Assert.IsTrue(item.Fields.ContainsKey("Place"));
            Assert.IsTrue(item.Fields["Place"] as Place == item.Place);
            Assert.IsTrue(item.Place.Name == "F:\\");
        }
    }
}