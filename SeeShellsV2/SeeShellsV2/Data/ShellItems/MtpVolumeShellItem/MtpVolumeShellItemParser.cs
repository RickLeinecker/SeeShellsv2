using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class MtpVolumeShellItemParser : IShellItemParser
    {
        private readonly IReadOnlySet<uint> KnownSignatures = new HashSet<uint> {
            0x10312005
        };

        public Type ShellItemType { get => typeof(MtpVolumeShellItem); }

        public int Priority { get => 2; }

        public bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            try
            {
                uint signature = BlockHelper.UnpackDWord(value, 0x06);
                return KnownSignatures.Contains(signature);
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
                uint signature = BlockHelper.UnpackDWord(value, 0x06);

                string typename = "Media Transfer Protocol";
                string subtypename = "Volume";

                int offset = 54;
                string storagename = BlockHelper.UnpackWString(value, offset);
                offset += 2 * (storagename.Length + 1);
                string storageid = BlockHelper.UnpackWString(value, offset);
                offset += 2 * (storageid.Length + 1);
                string filesystemname = BlockHelper.UnpackWString(value, offset);

                MtpVolumeShellItem item = new MtpVolumeShellItem()
                {
                    Size = size,
                    Signature = signature,
                    TypeName = typename,
                    SubtypeName = subtypename,
                    StorageId = storageid,
                    FileSystemName = filesystemname,
                    Place = new SystemFolder()
                    {
                        Name = storagename,
                        PathName = parent != null ? Path.Join(parent.Place.PathName, parent.Place.Name) : null,
                    },
                    RegistryHive = hive,
                    Value = value,
                    NodeSlot = keyWrapper?.NodeSlot,
                    SlotModifiedDate = keyWrapper?.SlotModifiedDate,
                    LastRegistryWriteDate = keyWrapper?.LastRegistryWriteDate ?? DateTime.MinValue,
                    Description = storagename,
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
