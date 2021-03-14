using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShellsV2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Registry;
using Registry.Abstractions;

namespace SeeShellsV2.Data.Tests
{
    [TestClass()]
    public class RegistryKeyWrapperTests
    {
        [TestMethod()]
        public void RegistryKeyWrapperTest()
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

            RegistryKeyWrapper rkey = new RegistryKeyWrapper(buf);

            Assert.IsTrue(rkey.Value == buf);
        }

        [TestMethod()]
        public void RegistryKeyWrapperTest1()
        {
            byte[] buf = new byte[] {
                0x14, 0x00, 0x1F, 0x50, 0xE0, 0x4F, 0xD0, 0x20,
                0xEA, 0x3A, 0x69, 0x10, 0xA2, 0xD8, 0x08, 0x00,
                0x2B, 0x30, 0x30, 0x9D, 0x00, 0x00
            };

            RegistryKeyWrapper rkey = new RegistryKeyWrapper(buf, "user", "@\\ntuser.dat");

            Assert.IsTrue(rkey.Value == buf);
            Assert.IsTrue(rkey.RegistryUser == "user");
            Assert.IsTrue(rkey.RegistryPath == "@\\ntuser.dat");
        }

        [TestMethod()]
        public void RegistryKeyWrapperTest2()
        {
            string key = Microsoft.Win32.Registry.Users.GetSubKeyNames().First();

            Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.Users.OpenSubKey(key); // might not work on github workflow
            byte[] value = (byte[])Microsoft.Win32.Registry.Users.GetValue(key);

            RegistryKeyWrapper rkWrapper = new RegistryKeyWrapper(rkey, value);

            Assert.IsTrue(rkWrapper.LastRegistryWriteDate != DateTime.MinValue);
            Assert.IsTrue(rkWrapper.RegistryPath != string.Empty);
            Assert.IsTrue(rkWrapper.RegistryUser != string.Empty);
            Assert.IsTrue(rkWrapper.RegistrySID != string.Empty);
        }

        [TestMethod()]
        public void RegistryKeyWrapperTest3()
        {
            RegistryHiveOnDemand hive = new RegistryHiveOnDemand("Resources\\UsrClass.dat");

            RegistryKey key = hive.GetKey("Local Settings\\Software\\Microsoft\\Windows\\Shell\\BagMRU\\0\\0");

            RegistryKeyWrapper rkWrapper = new RegistryKeyWrapper(key, key.Values.First().ValueDataRaw, hive, null);

            Assert.IsTrue(rkWrapper.LastRegistryWriteDate == new DateTime(637300198697461019, DateTimeKind.Utc).ToLocalTime());
            Assert.IsTrue(rkWrapper.RegistryPath == "S-1-5-21-1023748813-111960264-4059772771-1001_Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell\\BagMRU\\0\\0");
            Assert.IsTrue(rkWrapper.SlotModifiedDate == new DateTime(637300198698554500, DateTimeKind.Utc).ToLocalTime());
            Assert.IsTrue(rkWrapper.RegistryUser == "S-1-5-21-1023748813-111960264-4059772771-1001");
        }
    }
}