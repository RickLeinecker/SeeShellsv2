using System;
using System.Collections.Generic;
using System.Text;

namespace SeeShellsV2.Utilities
{
    public class ShellParserException : Exception
    {
        public ShellParserException() { }
        public ShellParserException(string message) : base(message) { }
        public ShellParserException(string message, Exception inner) : base(message, inner) { }
    }

    public static class BlockHelper
    {
        /// <summary>
        /// Unpacks 8-bit unsigned integer
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the byte</param>
        /// <returns>unsigned integer from buffer</returns>
        /// </summary>
        public static byte UnpackByte(byte[] buf, int offset)
        {
            try
            {
                return buf[offset];
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read byte at given offset", ex);
            }
        }

        /// <summary>
        /// Unpacks 16-bit unsigned integer
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the first byte of the word</param>
        /// <returns>unsigned integer from buffer</returns>
        /// </summary>
        public static ushort UnpackWord(byte[] buf, int offset)
        {
            try
            {
                return BitConverter.ToUInt16(buf, offset);
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read word at given offset", ex);
            }
        }

        /// <summary>
        /// Unpacks 16 byte GUID string
        /// </summary>
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the first byte of the GUID string</param>
        /// <returns>GUID string</returns>
        public static string UnpackGuid(byte[] buf, int offset)
        {
            try
            {
                return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}-{4:x2}{5:x2}-{6:x2}{7:x2}-{8:x2}{9:x2}-{10:x2}{11:x2}{12:x2}{13:x2}{14:x2}{15:x2}",
                    buf[offset + 3], buf[offset + 2], buf[offset + 1], buf[offset],
                    buf[offset + 5], buf[offset + 4],
                    buf[offset + 7], buf[offset + 6],
                    buf[offset + 8], buf[offset + 9],
                    buf[offset + 10], buf[offset + 11], buf[offset + 12], buf[offset + 13], buf[offset + 14], buf[offset + 15]);
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read guid at given offset", ex);
            }
        }

        /// <summary>
        /// Unpacks a unicode string. Reads characters until null terminator or end of buffer
        /// </summary>
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the first byte of the unicode string</param>
        /// <returns>stringy string</returns>
        public static string UnpackWString(byte[] buf, int offset)
        {
            try
            {
                return Encoding.Unicode.GetString(buf, offset, buf.Length - offset).Split('\0')[0];
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read unicode string at given offset", ex);
            }
        }

        /// <summary>
        /// Unpacks an ascii string. Reads characters until null terminator or end of buffer
        /// </summary>
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the first byte of the ascii string</param>
        /// <returns>stringy string</returns>
        public static string UnpackString(byte[] buf, int offset)
        {
            try
            {
                return Encoding.ASCII.GetString(buf, offset, buf.Length - offset).Split('\0')[0];
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read ascii string at given offset", ex);
            }
        }

        /// <summary>
        /// unpacks a equivalent to 4 bytes
        /// </summary>
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the first byte of the dword</param>
        /// <returns>unsigned integer from buffer</returns>
        public static uint UnpackDWord(byte[] buf, int offset)
        {
            try
            {
                return BitConverter.ToUInt32(buf, offset);
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read dword at given offset", ex);
            }
        }

        /// <summary>
        /// unpacks a equivalent to 8 bytes
        /// </summary>
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the first byte of the qword</param>
        /// <returns>unsigned integer from buffer</returns>
        public static ulong UnpackQWord(byte[] buf, int offset)
        {
            try
            {
                return BitConverter.ToUInt64(buf, offset);
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read qword at given offset", ex);
            }
        }

        /// <summary>
        /// Unpack a DOS DateTime from the buffer
        /// </summary>
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the first byte of the dos datetime</param>
        /// <returns>unpacked date in UTC</returns>
        public static DateTime UnpackDosDateTime(byte[] buf, int offset)
        {
            try
            {
                ushort dosdate = (ushort)(buf[offset + 1] << 8 | buf[offset]);
                ushort dostime = (ushort)(buf[offset + 3] << 8 | buf[offset + 2]);

                //check if the bytes contained no data
                if ((dosdate == 0 || dosdate == 1) && dostime == 0)
                {
                    return DateTime.MinValue; //same thing as invalid. (minvalue goes below the epoch)
                }

                int day = dosdate & 0x1F;
                int month = (dosdate & 0x1E0) >> 5;
                int year = (dosdate & 0xFE00) >> 9;
                year += 1980;

                int sec = (dostime & 0x1F) * 2;
                int minute = (dostime & 0x7E0) >> 5;
                int hour = (dostime & 0xF800) >> 11;

                return new DateTime(year, month, day, hour, minute, sec, DateTimeKind.Utc);
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read dos datetime at given offset", ex);
            }
        }

        /// <summary>
        /// Pulls out a Windows File time from its byte representation
        /// FileTimes are shown to be represented in 8 Bytes(QWord)
        /// </summary>
        /// <param name="buf">byte array to unpack from</param>
        /// <param name="offset">offset of the first byte of the filetime</param>
        /// <returns>unpacked date in UTC</returns>
        public static DateTime UnpackFileTime(byte[] buf, int offset)
        {
            try
            {
                return DateTime.FromFileTimeUtc(BitConverter.ToInt64(buf, offset));
            }
            catch (Exception ex)
            {
                throw new ShellParserException("could not read filetime at given offset", ex);
            }
        }

        public static int AlignTo(int start, int offset, int alignment)
        {
            return (offset - start) % alignment == 0 ? offset : (offset + (alignment - (offset - start) % alignment));
        }
    }
}
