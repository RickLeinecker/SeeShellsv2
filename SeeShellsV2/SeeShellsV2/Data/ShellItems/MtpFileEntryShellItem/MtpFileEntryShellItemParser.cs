using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class MtpFileEntryShellItemParser : IShellItemParser
    {
        private IReadOnlySet<uint> KnownSignatures = new HashSet<uint> {
            0x07192006
        };

        public Type ShellItemType { get => typeof(MtpFileEntryShellItem); }

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
                string subtypename = "File Entry";

                //DateTime modified = BlockHelper.UnpackFileTime(value, 0x1A);
                //DateTime created = BlockHelper.UnpackFileTime(value, 0x22);

                int offset = 0x4A;
                string foldername = BlockHelper.UnpackWString(value, offset);
                offset += 2 * (foldername.Length + 1);

                if (foldername == string.Empty)
                {
                    foldername = BlockHelper.UnpackWString(value, offset);
                    offset += 2 * (foldername.Length + 1);
                }

                string folderid = BlockHelper.UnpackWString(value, offset);

                Place p = new Folder()
                {
                    Name = foldername,
                    PathName = parent != null ? Path.Join(parent.Place.PathName, parent.Place.Name) : null,
                };

                if (hive.Places.Contains(p))
                    p = hive.Places.First(place => place == p);
                else
                    hive.Places.Add(p);

                MtpFileEntryShellItem item = new MtpFileEntryShellItem()
                {
                    Size = size,
                    Signature = signature,
                    TypeName = typename,
                    SubtypeName = subtypename,
                    //ModifiedDate = modified,
                    //CreationDate = created,
                    Place = p,
                    RegistryHive = hive,
                    Value = value,
                    NodeSlot = keyWrapper?.NodeSlot,
                    SlotModifiedDate = keyWrapper?.SlotModifiedDate,
                    LastRegistryWriteDate = keyWrapper?.LastRegistryWriteDate ?? DateTime.MinValue,
                    FolderId = folderid,
                    Description = foldername,
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
