#region copyright
// SeeShells Copyright (c) 2019-2020 Aleksandar Stoyanov, Bridget Woodye, Klayton Killough, 
// Richard Leinecker, Sara Frackiewicz, Yara As-Saidi
// SeeShells is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// SeeShells is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with this program;
// if not, see <https://www.gnu.org/licenses>
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace SeeShellsV2.Data
{
    public static class Block
    {
        public static byte unpack_byte(byte[] buf, int offset)
        {
            return buf[offset];
        }
        /// <summary>
        /// Unpacks a value equal to 2 bytes
        /// </summary>
        public static ushort unpack_word(byte[] buf, int offset)
        {
            return BitConverter.ToUInt16(buf, offset);
        }

        public static string unpack_guid(byte[] buf, int offset)
        {
            return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}-{4:x2}{5:x2}-{6:x2}{7:x2}-{8:x2}{9:x2}-{10:x2}{11:x2}{12:x2}{13:x2}{14:x2}{15:x2}",
                buf[offset + 3], buf[offset + 2], buf[offset + 1], buf[offset],
                buf[offset + 5], buf[offset + 4],
                buf[offset + 7], buf[offset + 6],
                buf[offset + 8], buf[offset + 9],
                buf[offset + 10], buf[offset + 11], buf[offset + 12], buf[offset + 13], buf[offset + 14], buf[offset + 15]);
        }
        /// <summary>
        /// Unpacks a Unicode encoded String of various sizing.
        /// </summary>
        /// <param name="off"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string unpack_wstring(byte[] buf, int offset, int length = 0)
        {
            if (length == 0)
            {
                int end = offset;
                for (int ind = offset; ind + 1 < buf.Length; ind += 2)
                {
                    if (buf[ind] == 0 && buf[ind + 1] == 0)
                    {
                        end = ind;
                        break;
                    }
                }
                length = end - offset;
            }

            while (buf[offset + length - 2] == 0 && buf[offset + length - 1] == 0) length -= 2;

            return Encoding.Unicode.GetString(buf, offset, length);
        }

        public static string unpack_string(byte[] buf, int offset, int length = 0)
        {
            if (length == 0)
            {
                int end = Array.IndexOf(buf, (byte)0, offset);
                length = end - offset;
                if (length == 0) return string.Empty;
            }

            while (buf[offset + length - 1] == 0) --length;

            return Encoding.ASCII.GetString(buf, offset, length);
        }

        /// <summary>
        /// unpacks a equivalent to 4 bytes
        /// </summary>
        /// <param name="off"></param>
        /// <returns></returns>
        public static uint unpack_dword(byte[] buf, int offset)
        {
            return BitConverter.ToUInt32(buf, offset);
        }

        /// <summary>
        /// unpacks a equivalent to 8 bytes
        /// </summary>
        /// <param name="off"></param>
        /// <returns></returns>
        public static ulong UnpackQword(byte[] buf, int offset)
        {
            return BitConverter.ToUInt64(buf, offset);
        }


        public static DateTime unpack_dosdate(byte[] buf, int offset)
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

            return new DateTime(year, month, day, hour, minute, sec);
        }

        /// <summary>
        /// Pulls out a Windows File time from its byte representation
        /// FileTimes are shown to be represented in 8 Bytes(QWord)
        /// <
        /// </summary>
        /// <param name="off"></param>
        /// <returns></returns>
        public static DateTime UnpackFileTime(byte[] buf, int offset)
        {
            return DateTime.FromFileTimeUtc(BitConverter.ToInt64(buf, offset));
        }

        public static int align(int off, int alignment)
        {
            if (off % alignment == 0)
                return off;
            return off + (alignment - off % alignment);
        }
    }
}
