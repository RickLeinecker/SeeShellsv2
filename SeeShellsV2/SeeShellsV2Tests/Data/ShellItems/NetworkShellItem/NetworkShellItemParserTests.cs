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
    public class NetworkShellItemParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            byte[] buf = new byte[] {
                0x31, 0x00, 0xC3, 0x01, 0xC1, 0x5C, 0x5C, 0x31,
                0x39, 0x32, 0x2E, 0x31, 0x36, 0x38, 0x2E, 0x38,
                0x30, 0x2E, 0x31, 0x32, 0x39, 0x5C, 0x55, 0x73,
                0x65, 0x72, 0x73, 0x00, 0x4D, 0x69, 0x63, 0x72,
                0x6F, 0x73, 0x6F, 0x66, 0x74, 0x20, 0x4E, 0x65,
                0x74, 0x77, 0x6F, 0x72, 0x6B, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00
            };

            NetworkShellItemParser parser = new NetworkShellItemParser();
            NetworkShellItem item = parser.Parse(null, null, buf) as NetworkShellItem;

            Assert.IsTrue(item.Fields.Count == 15);

            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("SubtypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("Description"));
            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields.ContainsKey("NetworkFlags"));
            Assert.IsTrue(item.Fields.ContainsKey("NetworkLocation"));
            Assert.IsTrue(item.Fields.ContainsKey("NetworkDescription"));
            Assert.IsTrue(item.Fields.ContainsKey("NetworkComments"));

            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.Fields["SubtypeName"] as string == item.SubtypeName);
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);
            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue((NetworkShellItem.NetworkFlagBits)item.Fields["NetworkFlags"] == item.NetworkFlags);
            Assert.IsTrue(item.Fields["NetworkDescription"] as string == item.NetworkDescription);
            Assert.IsTrue(item.Fields["NetworkComments"] as string == item.NetworkComments);

            Assert.IsTrue(item.Type == 0xC3);
            Assert.IsTrue(item.TypeName == "Network NetworkLocation");
            Assert.IsTrue(item.SubtypeName == "Share UNC Path");
            Assert.IsTrue(item.Description == "\\\\192.168.80.129\\Users");
            Assert.IsTrue(item.Size == 0x31);
            Assert.IsTrue(item.NetworkFlags == (NetworkShellItem.NetworkFlagBits.Unknown1 | NetworkShellItem.NetworkFlagBits.HasDescription | NetworkShellItem.NetworkFlagBits.HasComments));
            Assert.IsTrue(item.NetworkLocation == "\\\\192.168.80.129\\Users");
            Assert.IsTrue(item.NetworkDescription == "Microsoft Network");
            Assert.IsTrue(item.NetworkComments == string.Empty);
        }
    }
}