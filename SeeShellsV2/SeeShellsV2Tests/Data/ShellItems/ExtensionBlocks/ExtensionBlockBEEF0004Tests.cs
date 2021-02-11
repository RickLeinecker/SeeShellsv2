using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SeeShellsV2.Data.Tests
{
    /// <summary>
    /// test class for <see cref="ExtensionBlockBEEF0004"/>
    /// </summary>
    [TestClass()]
    public class ExtensionBlockBEEF0004Tests
    {
        private readonly DateTime dateDefault = DateTime.MinValue;
        private readonly int numericDefault = 0;
        private readonly string stringDefault = string.Empty;

        private readonly int testOffset = 5;

        private readonly byte[] testBufferVersion3 = {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // offset 5
            0x27, 0x00,                   // Size: 39
            0x03, 0x00,                   // ExtensionVersion: 0x0003
            0x04, 0x00, 0xEF, 0xBE,       // Signature: 0xBEEF0004
            0xAA, 0x00, 0xAA, 0xAA,       // CreationDate: 09 APR 1992 00:05:22 Local
            0x89, 0x18, 0xAB, 0x00,       // AccessedDate: 16 DEC 2003 00:05:22 Local
            0x00, 0x00,                   // unknown
            0x04, 0x00,                   // LongNameSize: 4
            0x42, 0x00, 0x45, 0x00,       // LongName (Unicode): BE
            0x45, 0x00, 0x46, 0x00,       // LongName (Unicode): EF
            0x00, 0x00,                   // LongName (Unicode): END
            0x42, 0x41, 0x44, 0x00        // LocalizedName (ASCII): BAD\0
        };

        private readonly byte[] testBufferVersion7 = {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF,                   // offset 5
            0x3D, 0x00,                                     // Size: 61
            0x07, 0x00,                                     // ExtensionVersion: 0x0007
            0x04, 0x00, 0xEF, 0xBE,                         // Signature: 0xBEEF0004
            0xAA, 0x00, 0xAA, 0xAA,                         // CreationDate: 09 APR 1992 00:05:22 Local
            0x89, 0x18, 0xAB, 0x00,                         // AccessedDate: 16 DEC 2003 00:05:22 Local
            0x00, 0x00,                                     // unknown
            0x00, 0x00,                                     // unknown
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // fileref
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00 ,0x00, 0x00, // unknown
            0x04, 0x00,                                     // LongNameSize: 4
            0x42, 0x00, 0x45, 0x00,                         // LongName (Unicode): BE
            0x45, 0x00, 0x46, 0x00,                         // LongName (Unicode): EF
            0x00, 0x00,                                     // LongName (Unicode): END
            0x42, 0x00, 0x41, 0x00, 0x44, 0x00, 0x00, 0x00  // LocalizedName (ASCII): BAD\0
        };

        private readonly byte[] testBufferVersion8 = {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF,                   // offset 5
            0x41, 0x00,                                     // Size: 65
            0x08, 0x00,                                     // ExtensionVersion: 0x0008
            0x04, 0x00, 0xEF, 0xBE,                         // Signature: 0xBEEF0004
            0xAA, 0x00, 0xAA, 0xAA,                         // CreationDate: 09 APR 1992 00:05:22 Local
            0x89, 0x18, 0xAB, 0x00,                         // AccessedDate: 16 DEC 2003 00:05:22 Local
            0x00, 0x00,                                     // unknown
            0x00, 0x00,                                     // unknown
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // fileref
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00 ,0x00, 0x00, // unknown
            0x04, 0x00,                                     // LongNameSize: 4
            0x00, 0x00, 0x00, 0x00,                         // unknown
            0x42, 0x00, 0x45, 0x00,                         // LongName (Unicode): BE
            0x45, 0x00, 0x46, 0x00,                         // LongName (Unicode): EF
            0x00, 0x00,                                     // LongName (Unicode): END
            0x42, 0x00, 0x41, 0x00, 0x44, 0x00, 0x00, 0x00  // LocalizedName (ASCII): BAD\0
        };

        private readonly byte[] testBufferVersion9 = {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF,                   // offset 5
            0x45, 0x00,                                     // Size: 69
            0x09, 0x00,                                     // ExtensionVersion: 0x0007
            0x04, 0x00, 0xEF, 0xBE,                         // Signature: 0xBEEF0004
            0xAA, 0x00, 0xAA, 0xAA,                         // CreationDate: 09 APR 1992 00:05:22 Local
            0x89, 0x18, 0xAB, 0x00,                         // AccessedDate: 16 DEC 2003 00:05:22 Local
            0x00, 0x00,                                     // unknown
            0x00, 0x00,                                     // unknown
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // fileref
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00 ,0x00, 0x00, // unknown
            0x04, 0x00,                                     // LongNameSize: 4
            0x00, 0x00, 0x00, 0x00,                         // unknown
            0x00, 0x00, 0x00, 0x00,                         // unknown
            0x42, 0x00, 0x45, 0x00,                         // LongName (Unicode): BE
            0x45, 0x00, 0x46, 0x00,                         // LongName (Unicode): EF
            0x00, 0x00,                                     // LongName (Unicode): END
            0x42, 0x00, 0x41, 0x00, 0x44, 0x00, 0x00, 0x00  // LocalizedName (ASCII): BAD\0
        };

        /// <summary>
        /// test <see cref="ExtensionBlockBEEF0004()"/> with custom properties
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockBEEF0004Test()
        {
            ExtensionBlockBEEF0004 block = new ExtensionBlockBEEF0004()
            {
                CreationDate = new DateTime(1000200020),
                AccessedDate = new DateTime(1289012842900),
                LongName = "EXTNS",
                LocalizedName = "Hello, World!",
                Size = 42,
                ExtensionVersion = 51,
                Signature = 1000
            };

            Assert.IsTrue(block.Fields.Count == 7);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == 42);
            Assert.IsTrue(block.Fields.ContainsKey("ExtensionVersion"));
            Assert.IsTrue(block.Fields["ExtensionVersion"] as ushort? == block.ExtensionVersion);
            Assert.IsTrue(block.ExtensionVersion == 51);
            Assert.IsTrue(block.Fields.ContainsKey("Signature"));
            Assert.IsTrue(block.Fields["Signature"] as uint? == block.Signature);
            Assert.IsTrue(block.Signature == 1000);
            Assert.IsTrue(block.Fields.ContainsKey("CreationDate"));
            Assert.IsTrue(block.Fields["CreationDate"] as DateTime? == block.CreationDate);
            Assert.IsTrue(block.CreationDate == new DateTime(1000200020));
            Assert.IsTrue(block.Fields.ContainsKey("AccessedDate"));
            Assert.IsTrue(block.Fields["AccessedDate"] as DateTime? == block.AccessedDate);
            Assert.IsTrue(block.AccessedDate == new DateTime(1289012842900));
            Assert.IsTrue(block.Fields.ContainsKey("LongName"));
            Assert.IsTrue(block.Fields["LongName"] as string == block.LongName);
            Assert.IsTrue(block.LongName == "EXTNS");
            Assert.IsTrue(block.Fields.ContainsKey("LocalizedName"));
            Assert.IsTrue(block.Fields["LocalizedName"] as string == block.LocalizedName);
            Assert.IsTrue(block.LocalizedName == "Hello, World!");
        }

        /// <summary>
        /// test <see cref="ExtensionBlockBEEF0004()"/> with default properties
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockBEEF0004Test1()
        {
            ExtensionBlockBEEF0004 block = new ExtensionBlockBEEF0004();

            Assert.IsTrue(block.Fields.Count == 0);
            Assert.IsTrue(block.Size == numericDefault);
            Assert.IsTrue(block.ExtensionVersion == numericDefault);
            Assert.IsTrue(block.Signature == numericDefault);
            Assert.IsTrue(block.CreationDate == dateDefault);
            Assert.IsTrue(block.AccessedDate == dateDefault);
            Assert.IsTrue(block.LongName == stringDefault);
            Assert.IsTrue(block.LocalizedName == stringDefault);
        }

        /// <summary>
        /// test <see cref="ExtensionBlockBEEF0004(byte[], int)"/> with ver3 buffer
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockBEEF0004Test2()
        {
            ExtensionBlockBEEF0004 block = new ExtensionBlockBEEF0004(testBufferVersion3, testOffset);

            Assert.IsTrue(block.Fields.Count == 7);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == testBufferVersion3.Length);
            Assert.IsTrue(block.Fields.ContainsKey("ExtensionVersion"));
            Assert.IsTrue(block.Fields["ExtensionVersion"] as ushort? == block.ExtensionVersion);
            Assert.IsTrue(block.ExtensionVersion == 3);
            Assert.IsTrue(block.Fields.ContainsKey("Signature"));
            Assert.IsTrue(block.Fields["Signature"] as uint? == block.Signature);
            Assert.IsTrue(block.Signature == 0xBEEF0004);
            Assert.IsTrue(block.Fields.ContainsKey("CreationDate"));
            Assert.IsTrue(block.Fields["CreationDate"] as DateTime? == block.CreationDate);
            Assert.IsTrue(block.CreationDate == new DateTime(1980, 5, 10, 21, 21, 20));
            Assert.IsTrue(block.Fields.ContainsKey("AccessedDate"));
            Assert.IsTrue(block.Fields["AccessedDate"] as DateTime? == block.AccessedDate);
            Assert.IsTrue(block.AccessedDate == new DateTime(1992, 4, 9, 0, 5, 22));
            Assert.IsTrue(block.Fields.ContainsKey("LongName"));
            Assert.IsTrue(block.Fields["LongName"] as string == block.LongName);
            Assert.IsTrue(block.LongName == "BEEF");
            Assert.IsTrue(block.Fields.ContainsKey("LocalizedName"));
            Assert.IsTrue(block.Fields["LocalizedName"] as string == block.LocalizedName);
            Assert.IsTrue(block.LocalizedName == "BAD");
        }

        /// <summary>
        /// test <see cref="ExtensionBlockBEEF0004(byte[], int)"/> with ver7 buffer
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockBEEF0004Test3()
        {
            ExtensionBlockBEEF0004 block = new ExtensionBlockBEEF0004(testBufferVersion7, testOffset);

            Assert.IsTrue(block.Fields.Count == 7);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == testBufferVersion7.Length);
            Assert.IsTrue(block.Fields.ContainsKey("ExtensionVersion"));
            Assert.IsTrue(block.Fields["ExtensionVersion"] as ushort? == block.ExtensionVersion);
            Assert.IsTrue(block.ExtensionVersion == 7);
            Assert.IsTrue(block.Fields.ContainsKey("Signature"));
            Assert.IsTrue(block.Fields["Signature"] as uint? == block.Signature);
            Assert.IsTrue(block.Signature == 0xBEEF0004);
            Assert.IsTrue(block.Fields.ContainsKey("CreationDate"));
            Assert.IsTrue(block.Fields["CreationDate"] as DateTime? == block.CreationDate);
            Assert.IsTrue(block.CreationDate == new DateTime(1980, 5, 10, 21, 21, 20));
            Assert.IsTrue(block.Fields.ContainsKey("AccessedDate"));
            Assert.IsTrue(block.Fields["AccessedDate"] as DateTime? == block.AccessedDate);
            Assert.IsTrue(block.AccessedDate == new DateTime(1992, 4, 9, 0, 5, 22));
            Assert.IsTrue(block.Fields.ContainsKey("LongName"));
            Assert.IsTrue(block.Fields["LongName"] as string == block.LongName);
            Assert.IsTrue(block.LongName == "BEEF");
            Assert.IsTrue(block.Fields.ContainsKey("LocalizedName"));
            Assert.IsTrue(block.Fields["LocalizedName"] as string == block.LocalizedName);
            Assert.IsTrue(block.LocalizedName == "BAD");
        }

        /// <summary>
        /// test <see cref="ExtensionBlockBEEF0004(byte[], int)"/> with ver8 buffer
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockBEEF0004Test4()
        {
            ExtensionBlockBEEF0004 block = new ExtensionBlockBEEF0004(testBufferVersion8, testOffset);

            Assert.IsTrue(block.Fields.Count == 7);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == testBufferVersion8.Length);
            Assert.IsTrue(block.Fields.ContainsKey("ExtensionVersion"));
            Assert.IsTrue(block.Fields["ExtensionVersion"] as ushort? == block.ExtensionVersion);
            Assert.IsTrue(block.ExtensionVersion == 8);
            Assert.IsTrue(block.Fields.ContainsKey("Signature"));
            Assert.IsTrue(block.Fields["Signature"] as uint? == block.Signature);
            Assert.IsTrue(block.Signature == 0xBEEF0004);
            Assert.IsTrue(block.Fields.ContainsKey("CreationDate"));
            Assert.IsTrue(block.Fields["CreationDate"] as DateTime? == block.CreationDate);
            Assert.IsTrue(block.CreationDate == new DateTime(1980, 5, 10, 21, 21, 20));
            Assert.IsTrue(block.Fields.ContainsKey("AccessedDate"));
            Assert.IsTrue(block.Fields["AccessedDate"] as DateTime? == block.AccessedDate);
            Assert.IsTrue(block.AccessedDate == new DateTime(1992, 4, 9, 0, 5, 22));
            Assert.IsTrue(block.Fields.ContainsKey("LongName"));
            Assert.IsTrue(block.Fields["LongName"] as string == block.LongName);
            Assert.IsTrue(block.LongName == "BEEF");
            Assert.IsTrue(block.Fields.ContainsKey("LocalizedName"));
            Assert.IsTrue(block.Fields["LocalizedName"] as string == block.LocalizedName);
            Assert.IsTrue(block.LocalizedName == "BAD");
        }

        /// <summary>
        /// test <see cref="ExtensionBlockBEEF0004(byte[], int)"/> with ver9 buffer
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockBEEF0004Test5()
        {
            ExtensionBlockBEEF0004 block = new ExtensionBlockBEEF0004(testBufferVersion9, testOffset);

            Assert.IsTrue(block.Fields.Count == 7);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == testBufferVersion9.Length);
            Assert.IsTrue(block.Fields.ContainsKey("ExtensionVersion"));
            Assert.IsTrue(block.Fields["ExtensionVersion"] as ushort? == block.ExtensionVersion);
            Assert.IsTrue(block.ExtensionVersion == 9);
            Assert.IsTrue(block.Fields.ContainsKey("Signature"));
            Assert.IsTrue(block.Fields["Signature"] as uint? == block.Signature);
            Assert.IsTrue(block.Signature == 0xBEEF0004);
            Assert.IsTrue(block.Fields.ContainsKey("CreationDate"));
            Assert.IsTrue(block.Fields["CreationDate"] as DateTime? == block.CreationDate);
            Assert.IsTrue(block.CreationDate == new DateTime(1980, 5, 10, 21, 21, 20));
            Assert.IsTrue(block.Fields.ContainsKey("AccessedDate"));
            Assert.IsTrue(block.Fields["AccessedDate"] as DateTime? == block.AccessedDate);
            Assert.IsTrue(block.AccessedDate == new DateTime(1992, 4, 9, 0, 5, 22));
            Assert.IsTrue(block.Fields.ContainsKey("LongName"));
            Assert.IsTrue(block.Fields["LongName"] as string == block.LongName);
            Assert.IsTrue(block.LongName == "BEEF");
            Assert.IsTrue(block.Fields.ContainsKey("LocalizedName"));
            Assert.IsTrue(block.Fields["LocalizedName"] as string == block.LocalizedName);
            Assert.IsTrue(block.LocalizedName == "BAD");
        }
    }
}