using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class UriShellItemParser : IShellItemParser
    {
        private IReadOnlySet<byte> KnownTypes = new HashSet<byte> { 0x61 };

        public Type ShellItemType { get => typeof(UriShellItem); }

        public int Priority { get => 2; }

        public bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            try
            {
                byte type = BlockHelper.UnpackByte(value, 0x02);
                return (type & 0x70) == 0x60 && KnownTypes.Contains(type);
            }
            catch { return false; }
        }

        public IShellItem Parse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            if (!CanParse(hive, keyWrapper, value, parent))
                return null;

            try
            {
                ushort size = BlockHelper.UnpackWord(value, 0x00);
                byte type = BlockHelper.UnpackByte(value, 0x02);

                string typename = "URI";

                int off = 0x03;

                UriShellItem.UriFlagBits uriflags = (UriShellItem.UriFlagBits)BlockHelper.UnpackByte(value, off);
                off += 1;

                DateTime connection = DateTime.MinValue;
                string ftphostname = null;
                string ftpusername = null;
                string ftppassword = null;
                string uri = null;

                ushort dataSize = BlockHelper.UnpackWord(value, off); //0 if no data, does not include 2 bytes of the normal size indicator
                if (dataSize != 0)
                {
                    off += 2; //move past Size of Data
                    off += 4; //move past unknown
                    off += 4; //move past unknown
                    if (off < size)
                    {
                        connection = BlockHelper.UnpackFileTime(value, off); //timestamp in "FILETIME" format (location: 0x0E)
                        off += 8; //move past ConnectionTime
                    }
                    off += 4; //move past unknown 0000 or FFFF
                    off += 12; //move past unknown empty section
                    off += 4; //unknown
                    if (off < size)
                    {
                        off = BlockHelper.AlignTo(2, off, 4);
                        uint length = BlockHelper.UnpackDWord(value, off);
                        off += 4; //move past size
                        off = BlockHelper.AlignTo(2, off, 4);
                        if ((type & (byte)UriShellItem.UriFlagBits.HasUnicodeStrings) != 0)
                        {
                            ftphostname = BlockHelper.UnpackWString(value, off);
                        }
                        else
                        {
                            ftphostname = BlockHelper.UnpackString(value, off);
                        }
                        off += (int)length; //move past string
                    }
                    if (off < size)
                    {
                        off = BlockHelper.AlignTo(2, off, 4);
                        uint length = BlockHelper.UnpackDWord(value, off);
                        off += 4; //move past size
                        off = BlockHelper.AlignTo(2, off, 4);
                        if ((type & (byte)UriShellItem.UriFlagBits.HasUnicodeStrings) != 0)
                        {
                            ftpusername = BlockHelper.UnpackWString(value, off);
                        }
                        else
                        {
                            ftpusername = BlockHelper.UnpackString(value, off);
                        }
                        off += (int)length; //move past string
                    }
                    if (off < size)
                    {
                        off = BlockHelper.AlignTo(2, off, 4);
                        uint length = BlockHelper.UnpackDWord(value, off);
                        off += 4; //move past size
                        off = BlockHelper.AlignTo(2, off, 4);
                        if ((type & (byte)UriShellItem.UriFlagBits.HasUnicodeStrings) != 0)
                        {
                            ftppassword = BlockHelper.UnpackWString(value, off);
                        }
                        else
                        {
                            ftppassword = BlockHelper.UnpackString(value, off);
                        }
                        off += (int)length; //move past string
                    }
                    if (off < size) //immediately afterwards is a common Uri
                    {
                        off = BlockHelper.AlignTo(2, off, 4);
                        if ((type & (byte)UriShellItem.UriFlagBits.HasUnicodeStrings) != 0)
                        {
                            uri = BlockHelper.UnpackWString(value, off);
                        }
                        else
                        {
                            uri = BlockHelper.UnpackString(value, off);
                        }
                    }
                }

                UriShellItem item = new UriShellItem()
                {
                    Size = size,
                    Type = type,
                    TypeName = typename,
                    UriFlags = uriflags,
                    ConnectedDate = connection,
                    FTPHostname = ftphostname,
                    FTPUsername = ftpusername,
                    FTPPassword = ftppassword,
                    Uri = uri,
                    Place = new NetworkLocation()
                    {
                        Name = ftphostname ?? uri ?? string.Empty,
                        PathName = null,
                    },
                    RegistryHive = hive,
                    Value = value,
                    NodeSlot = keyWrapper?.NodeSlot,
                    SlotModifiedDate = keyWrapper?.SlotModifiedDate,
                    LastRegistryWriteDate = keyWrapper?.LastRegistryWriteDate ?? DateTime.MinValue,
                    Description = ftphostname ?? uri ?? string.Empty,
                    Parent = parent
                };

                parent?.Children.Add(item);
                return item;
            }
            catch (ShellParserException)
            {
                return null;
            }
        }
    }
}
