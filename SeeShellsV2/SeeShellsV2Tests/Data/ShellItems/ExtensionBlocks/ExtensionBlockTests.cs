using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeeShellsV2.Data.Tests
{
    /// <summary>
    /// test class for <see cref="ExtensionBlock"/>
    /// </summary>
    [TestClass()]
    public class ExtensionBlockTests
    {
        private readonly ushort sizeDefaultValue = 0;
        private readonly ushort extensionVersionDefaultValue = 0;
        private readonly uint signatureDefaultValue = 0;

        private readonly int testOffset = 3;
        private readonly byte[] testBuffer = {
            0x00, 0x00, 0x00, 0x32,
            0x42, 0x00, 0x10, 0x00,
            0x0A, 0xFF, 0x2D, 0x00
        };

        /// <summary>
        /// test <see cref="ExtensionBlock()"/> with all custom properties
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockTest()
        {
            IExtensionBlock block = new ExtensionBlock()
            {
                Size = 42,
                ExtensionVersion = 51,
                Signature = 1000
            };

            Assert.IsTrue(block.Fields.Count == 3);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == 42);
            Assert.IsTrue(block.Fields.ContainsKey("ExtensionVersion"));
            Assert.IsTrue(block.Fields["ExtensionVersion"] as ushort? == block.ExtensionVersion);
            Assert.IsTrue(block.ExtensionVersion == 51);
            Assert.IsTrue(block.Fields.ContainsKey("Signature"));
            Assert.IsTrue(block.Fields["Signature"] as uint? == block.Signature);
            Assert.IsTrue(block.Signature == 1000);
        }

        /// <summary>
        /// test <see cref="ExtensionBlock()"/> with default properties
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockTest1()
        {
            IExtensionBlock block = new ExtensionBlock()
            {
                Size = 126
            };

            Assert.IsTrue(block.Fields.Count == 1);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == 126);
            Assert.IsTrue(block.ExtensionVersion == extensionVersionDefaultValue);
            Assert.IsTrue(block.Signature == signatureDefaultValue);
        }

        /// <summary>
        /// test <see cref="ExtensionBlock()"/> with mix of custom and default properties
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockTest2()
        {
            IExtensionBlock block = new ExtensionBlock();

            Assert.IsTrue(block.Fields.Count == 0);
            Assert.IsTrue(block.Size == sizeDefaultValue);
            Assert.IsTrue(block.ExtensionVersion == extensionVersionDefaultValue);
            Assert.IsTrue(block.Signature == signatureDefaultValue);
        }

        /// <summary>
        /// test <see cref="ExtensionBlock(byte[], int)"/> with custom buffer
        /// </summary>
        [TestMethod()]
        public void ExtensionBlockTest3()
        {
            IExtensionBlock block = new ExtensionBlock(testBuffer, testOffset);

            Assert.IsTrue(block.Fields.Count == 3);
            Assert.IsTrue(block.Fields.ContainsKey("Size"));
            Assert.IsTrue(block.Fields["Size"] as ushort? == block.Size);
            Assert.IsTrue(block.Size == 0x4232);
            Assert.IsTrue(block.Fields.ContainsKey("ExtensionVersion"));
            Assert.IsTrue(block.Fields["ExtensionVersion"] as ushort? == block.ExtensionVersion);
            Assert.IsTrue(block.ExtensionVersion == 0x1000);
            Assert.IsTrue(block.Fields.ContainsKey("Signature"));
            Assert.IsTrue(block.Fields["Signature"] as uint? == block.Signature);
            Assert.IsTrue(block.Signature == 0x2DFF0A00);
        }
    }
}