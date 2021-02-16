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
    public class NetworkShellItemTests
    {
        [TestMethod()]
        public void NetworkShellItemTest()
        {
            NetworkShellItem item = new NetworkShellItem()
            {
                Type = 0x00,
                TypeName = "TestType",
                SubtypeName = "123",
                Description = "Test",
                Size = 10,
                ModifiedDate = new DateTime(90810298),
                AccessedDate = new DateTime(908123802123),
                CreationDate = new DateTime(4865846),
                NetworkFlags = NetworkShellItem.NetworkFlagBits.HasDescription | NetworkShellItem.NetworkFlagBits.HasComments,
                NetworkLocation = "192.168.1.1",
                NetworkDescription = "description",
                NetworkComments = "comments"
            };

            Assert.IsTrue(item.Fields.Count == 12);
            Assert.IsTrue(item.RegistryKey == null);

            Assert.IsTrue(item.Fields.ContainsKey("Type"));
            Assert.IsTrue(item.Fields.ContainsKey("TypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("SubtypeName"));
            Assert.IsTrue(item.Fields.ContainsKey("Description"));
            Assert.IsTrue(item.Fields.ContainsKey("Size"));
            Assert.IsTrue(item.Fields.ContainsKey("ModifiedDate"));
            Assert.IsTrue(item.Fields.ContainsKey("AccessedDate"));
            Assert.IsTrue(item.Fields.ContainsKey("CreationDate"));
            Assert.IsTrue(item.Fields.ContainsKey("NetworkFlags"));
            Assert.IsTrue(item.Fields.ContainsKey("NetworkLocation"));
            Assert.IsTrue(item.Fields.ContainsKey("NetworkDescription"));
            Assert.IsTrue(item.Fields.ContainsKey("NetworkComments"));

            Assert.IsTrue(item.Fields["Type"] as byte? == item.Type);
            Assert.IsTrue(item.Fields["TypeName"] as string == item.TypeName);
            Assert.IsTrue(item.Fields["SubtypeName"] as string == item.SubtypeName);
            Assert.IsTrue(item.Fields["Description"] as string == item.Description);
            Assert.IsTrue(item.Fields["Size"] as ushort? == item.Size);
            Assert.IsTrue(item.Fields["ModifiedDate"] as DateTime? == item.ModifiedDate);
            Assert.IsTrue(item.Fields["AccessedDate"] as DateTime? == item.AccessedDate);
            Assert.IsTrue(item.Fields["CreationDate"] as DateTime? == item.CreationDate);
            Assert.IsTrue((NetworkShellItem.NetworkFlagBits)item.Fields["NetworkFlags"] == item.NetworkFlags);
            Assert.IsTrue(item.Fields["NetworkLocation"] as string == item.NetworkLocation);
            Assert.IsTrue(item.Fields["NetworkDescription"] as string == item.NetworkDescription);
            Assert.IsTrue(item.Fields["NetworkComments"] as string == item.NetworkComments);

            Assert.IsTrue(item.Type == 0x00);
            Assert.IsTrue(item.TypeName == "TestType");
            Assert.IsTrue(item.SubtypeName == "123");
            Assert.IsTrue(item.Description == "Test");
            Assert.IsTrue(item.Size == 10);
            Assert.IsTrue(item.ModifiedDate == new DateTime(90810298));
            Assert.IsTrue(item.AccessedDate == new DateTime(908123802123));
            Assert.IsTrue(item.CreationDate == new DateTime(4865846));
            Assert.IsTrue(item.NetworkFlags == (NetworkShellItem.NetworkFlagBits.HasDescription | NetworkShellItem.NetworkFlagBits.HasComments));
            Assert.IsTrue(item.NetworkLocation == "192.168.1.1");
            Assert.IsTrue(item.NetworkDescription == "description");
            Assert.IsTrue(item.NetworkComments == "comments");
        }

        [TestMethod()]
        public void NetworkShellItemTest1()
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

            NetworkShellItem item = new NetworkShellItem(buf);

            Assert.IsTrue(item.Fields.Count == 9);
            Assert.IsTrue(item.RegistryKey == null);

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

        [TestMethod()]
        public void NetworkShellItemTest2()
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

            IShellItem item = ShellItem.FromByteArray(buf);

            Assert.IsTrue(item is NetworkShellItem);
        }
    }
}