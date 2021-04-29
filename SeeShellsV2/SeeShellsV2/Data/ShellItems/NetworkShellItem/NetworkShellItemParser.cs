using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class NetworkShellItemParser : IShellItemParser
    {
        private IReadOnlySet<byte> KnownTypes = new HashSet<byte>{
            0x41, 0x42, 0x43, 0x46, 0x47, 0x4D, 0x4E, 0xC3
        };

        public Type ShellItemType { get => typeof(NetworkShellItem); }

        public int Priority { get => 2; }

        public bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            try
            {
                byte type = BlockHelper.UnpackByte(value, 0x02);
                return (type & 0x70) == 0x40 && KnownTypes.Contains(type);
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

                string typename = "Network NetworkLocation";
                string subtypename = string.Empty;

                if ((type & 0x0F) == (byte)NetworkShellItem.SubtypeFlags.DomainName)
                    subtypename = "Domain/WorkGroup Description";
                else if ((type & 0x0F) == (byte)NetworkShellItem.SubtypeFlags.ServerUNCPath)
                    subtypename = "Server UNC Path";
                else if ((type & 0x0F) == (byte)NetworkShellItem.SubtypeFlags.ShareUNCPath)
                    subtypename = "Share UNC Path";
                else if ((type & 0x0F) == (byte)NetworkShellItem.SubtypeFlags.MicrosoftWindowsNetwork)
                    subtypename = "Microsoft Windows Network";
                else if ((type & 0x0F) == (byte)NetworkShellItem.SubtypeFlags.EntireNetwork)
                    subtypename = "Entire Network";
                else if ((type & 0x0F) == (byte)NetworkShellItem.SubtypeFlags.NetworkPlacesRoot)
                    subtypename = "NetworkPlaces";
                else if ((type & 0x0F) == (byte)NetworkShellItem.SubtypeFlags.NetworkPlacesServer)
                    subtypename = "NetworkPlaces";

                NetworkShellItem.NetworkFlagBits networkflags = (NetworkShellItem.NetworkFlagBits)BlockHelper.UnpackByte(value, 0x04);
                string networklocation = BlockHelper.UnpackString(value, 0x05);

                int off = 0x05;
                off += networklocation.Length + 1;

                string networkdescription = string.Empty;
                if (networkflags.HasFlag(NetworkShellItem.NetworkFlagBits.HasDescription))
                {
                    networkdescription = BlockHelper.UnpackString(value, off);
                    off += networkdescription.Length + 1;
                }

                string networkcomments = string.Empty;
                if (networkflags.HasFlag(NetworkShellItem.NetworkFlagBits.HasComments))
                {
                    networkcomments = BlockHelper.UnpackString(value, off);
                }

                string name = networklocation
                    .Split(Path.DirectorySeparatorChar).Last();

                string pathname = networklocation
                    .Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                    .SkipLast(1)
                    .Aggregate("\\", (string a, string b) => a + "\\" + b);

                NetworkShellItem item = new NetworkShellItem()
                {
                    Size = size,
                    Type = type,
                    TypeName = typename,
                    SubtypeName = subtypename,
                    NetworkFlags = networkflags,
                    NetworkLocation = networklocation,
                    NetworkDescription = networkdescription,
                    NetworkComments = networkcomments,
                    Place = new NetworkLocation()
                    {
                        Name = networklocation,
                        PathName = string.Empty,
                    },
                    RegistryHive = hive,
                    Value = value,
                    NodeSlot = keyWrapper?.NodeSlot,
                    SlotModifiedDate = keyWrapper?.SlotModifiedDate,
                    LastRegistryWriteDate = keyWrapper?.LastRegistryWriteDate ?? DateTime.MinValue,
                    Description = networklocation,
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
