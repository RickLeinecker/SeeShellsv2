using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace SeeShellsV2.Utilities.Tests
{
    [TestClass()]
    public class BlockHelperTests
    {
        [TestMethod()]
        public void UnpackByteTest()
        {
            var rand = new Random();

            byte[] b = new byte[2048];
            byte[] buf = new byte[b.Length];

            for (var i = 0; i < b.Length; i++)
                b[i] = (byte)rand.Next(byte.MinValue, byte.MaxValue);

            for (var i = 0; i < buf.Length; i++)
                buf[i] = b[i];

            for (var i = 0; i < b.Length; i++)
                Assert.IsTrue(BlockHelper.UnpackByte(buf, i) == b[i]);
        }

        [TestMethod()]
        public void UnpackWordTest()
        {
            var rand = new Random();

            ushort[] b = new ushort[2048];
            byte[] buf = new byte[b.Length * 2];

            for (var i = 0; i < b.Length; i++)
                b[i] = (ushort)rand.Next(ushort.MinValue, ushort.MaxValue);

            for (var i = 0; i < buf.Length; i++)
                buf[i] = (byte) ((b[i/2] >> ((i % 2) * 8)) & byte.MaxValue);

            for (var i = 0; i < b.Length; i++)
                Assert.IsTrue(BlockHelper.UnpackWord(buf, i*2) == b[i]);
        }

        [TestMethod()]
        public void UnpackGuidTest()
        {
            byte[] buf = new byte[] {
                0xE0, 0x4F, 0xD0, 0x20, 0xEA, 0x3A, 0x69, 0x10,
                0xA2, 0xD8, 0x08, 0x00, 0x2B, 0x30, 0x30, 0x9D,
                0x00, 0x00
            };

            Assert.IsTrue(BlockHelper.UnpackGuid(buf, 0) == "20d04fe0-3aea-1069-a2d8-08002b30309d");
        }

        [TestMethod()]
        public void UnpackWStringTest()
        {
            var builder = new StringBuilder();
            var rand = new Random();

            string[] s = new string[99];
            byte[] buf = new byte[2 * 4950 + 1]; // triangle number T(99) = 4950

            var offset = 0;
            for (var i = 0; i < s.Length; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    builder.Append((char)rand.Next(char.MinValue+1, char.MaxValue));

                    if (builder[builder.Length - 1] >= 0xD800 && builder[builder.Length - 1] <= 0xDFFF)
                    {
                        builder.Remove(builder.Length - 1, 1);
                        j--;
                    }
                }

                s[i] = builder.ToString();
                builder.Clear();
                Encoding.Unicode.GetBytes(s[i]).CopyTo(buf, offset);
                buf[offset + 2 * i] = 0x00;
                buf[offset + 2 * (i + 1)] = 0x00;
                offset += 2 * (i + 1);
            }

            offset = 0;
            for (var i = 0; i < s.Length; i++)
            {
                Assert.IsTrue(s[i].Length == i);
                Assert.IsTrue(BlockHelper.UnpackWString(buf, offset) == s[i]);
                offset += 2 * (s[i].Length + 1);
            }
        }

        [TestMethod()]
        public void UnpackStringTest()
        {
            var builder = new StringBuilder();
            var rand = new Random();

            string[] s = new string[99];
            byte[] buf = new byte[4950]; // triangle number T(99) = 4950

            var offset = 0;
            for (var i = 0; i < s.Length; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    builder.Append(Encoding.ASCII.GetString(new[] { (byte)rand.Next(1, 255) }));
                }

                s[i] = builder.ToString();
                builder.Clear();
                Encoding.ASCII.GetBytes(s[i]).CopyTo(buf, offset);
                buf[offset + i] = 0x00;
                offset += i + 1;
            }

            offset = 0;
            for (var i = 0; i < s.Length; i++)
            {
                Assert.IsTrue(s[i].Length == i);
                Assert.IsTrue(BlockHelper.UnpackString(buf, offset) == s[i]);
                offset += s[i].Length + 1;
            }
        }

        [TestMethod()]
        public void UnpackDWordTest()
        {
            var rand = new Random();

            uint[] b = new uint[2048];
            byte[] buf = new byte[b.Length * 4];

            for (var i = 0; i < b.Length; i++)
                b[i] = (uint) rand.Next(int.MinValue, int.MaxValue);

            for (var i = 0; i < buf.Length; i++)
                buf[i] = (byte)((b[i / 4] >> ((i % 4) * 8)) & byte.MaxValue);

            for (var i = 0; i < b.Length; i++)
                Assert.IsTrue(BlockHelper.UnpackDWord(buf, i * 4) == b[i]);
        }

        [TestMethod()]
        public void UnpackQwordTest()
        {
            var rand = new Random();

            ulong[] b = new ulong[2048];
            byte[] buf = new byte[b.Length * 8];

            for (var i = 0; i < b.Length; i++)
                b[i] = (ulong)rand.Next(int.MinValue, int.MaxValue);

            for (var i = 0; i < buf.Length; i++)
                buf[i] = (byte)((b[i / 8] >> ((i % 8) * 8)) & byte.MaxValue);

            for (var i = 0; i < b.Length; i++)
                Assert.IsTrue(BlockHelper.UnpackQWord(buf, i * 8) == b[i]);
        }

        [TestMethod()]
        public void UnpackDosDateTimeTest()
        {
            byte[] buf = new byte[] { 0xAA, 0x00, 0xAA, 0xAA };
            Assert.IsTrue(BlockHelper.UnpackDosDateTime(buf, 0) == new DateTime(1980, 5, 10, 21, 21, 20));
        }

        [TestMethod()]
        public void UnpackFileTimeTest()
        {
            byte[] buf = new byte[] { 0x00, 0x75, 0xA1, 0x4A, 0xDE, 0x1C, 0x71, 0xD2, 0x01 };
            Assert.IsTrue(BlockHelper.UnpackFileTime(buf, 1) == new DateTime(636202939949621621, DateTimeKind.Utc));
        }
    }
}