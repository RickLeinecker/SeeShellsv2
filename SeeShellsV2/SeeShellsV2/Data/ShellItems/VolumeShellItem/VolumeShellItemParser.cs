using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;

using SeeShellsV2.Repositories;
using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class VolumeShellItemParser : IShellItemParser
    {
        private IReadOnlySet<byte> KnownTypes = new HashSet<byte>{
            0x20, 0x21, 0x23, 0x25, 0x28, 0x29, 0x2A, 0x2E, 0x2F
        };

        private IConfig Config { get; set; }

        public VolumeShellItemParser([Dependency] IConfig config)
        {
            Config = config;
        }

        public Type ShellItemType { get => typeof(VolumeShellItem); }

        public int Priority { get => 2; }

        public bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            try
            {
                byte type = BlockHelper.UnpackByte(value, 0x02);
                return (type & 0x70) == 0x20 && KnownTypes.Contains(type);
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

                string typename = "Volume";

                string subtypename = string.Empty;
                string volumename = string.Empty;
                string description = string.Empty;

                if ((type & 0x8F) == (byte) VolumeShellItem.TypeFlags.LocalDisk)
                {
                    subtypename = "Local Disk";
                    volumename = BlockHelper.UnpackString(value, 0x03);
                    description = volumename;
                }
                else if ((type & 0x8F) == (byte)VolumeShellItem.TypeFlags.SystemFolder)
                {
                    subtypename = "System Folder";
                    string guid = BlockHelper.UnpackGuid(value, 0x04);
                    if (Config.KnownGuids.ContainsKey(guid))
                    {
                        description = volumename = Config.KnownGuids[guid];
                    }
                    else
                    {
                        description = volumename = guid;
                    }
                }

                VolumeShellItem item = new VolumeShellItem()
                {
                    Size = size,
                    Type = type,
                    TypeName = typename,
                    SubtypeName = subtypename,
                    Place = subtypename == "Local Disk" ?
                        new Drive()
                        {
                            Name = volumename,
                            PathName = parent != null ? Path.Join(parent.Place.PathName, parent.Place.Name) : null,
                        }
                        :
                        new SystemFolder()
                        {
                            Name = volumename,
                            PathName = parent != null ? Path.Join(parent.Place.PathName, parent.Place.Name) : null,
                        },
                    RegistryHive = hive,
                    Value = value,
                    NodeSlot = keyWrapper?.NodeSlot,
                    SlotModifiedDate = keyWrapper?.SlotModifiedDate,
                    LastRegistryWriteDate = keyWrapper?.LastRegistryWriteDate ?? DateTime.MinValue,
                    Description = description,
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
