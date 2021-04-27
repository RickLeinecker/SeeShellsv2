using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class MtpDeviceShellItemParser : IShellItemParser
    {
        private IReadOnlySet<uint> KnownSignatures = new HashSet<uint> {
            0x08312006
        };

        public Type ShellItemType { get => typeof(MtpDeviceShellItem); }

        public int Priority { get => 2; }

        public bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            try
            {
                uint signature = BlockHelper.UnpackDWord(value, 0x06);
                return KnownSignatures.Contains(signature);
            }
            catch
            {
                return false;
            }
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
                string subtypename = "Device";

                int offset = 40;
                string devicename = BlockHelper.UnpackWString(value, offset);
                offset += 2 * (devicename.Length + 1);
                string devicepath = BlockHelper.UnpackWString(value, offset);

                MtpDeviceShellItem item = new MtpDeviceShellItem()
                {
                    Size = size,
                    Signature = signature,
                    TypeName = typename,
                    SubtypeName = subtypename,
                    InternalDevicePath = devicepath,
                    Place = new RemovableDevice()
                    {
                        Name = devicename,
                        PathName = parent != null ? Path.Combine(parent.Place.PathName ?? string.Empty, parent.Place.Name) : null,
                    },
                    RegistryHive = hive,
                    Value = value,
                    NodeSlot = keyWrapper?.NodeSlot,
                    SlotModifiedDate = keyWrapper?.SlotModifiedDate,
                    LastRegistryWriteDate = keyWrapper?.LastRegistryWriteDate ?? DateTime.MinValue,
                    Description = devicename,
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
