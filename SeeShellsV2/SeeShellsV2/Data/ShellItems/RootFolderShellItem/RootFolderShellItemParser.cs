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
    public class RootFolderShellItemParser : IShellItemParser
    {
        private IReadOnlySet<byte> KnownTypes = new HashSet<byte> {
            0x1F
        };

        private IConfig Config { get; set; }

        public RootFolderShellItemParser([Dependency] IConfig config)
        {
            Config = config;
        }

        public Type ShellItemType { get => typeof(RootFolderShellItem); }

        public int Priority { get => 2; }

        public bool CanParse(RegistryHive hive, RegistryKeyWrapper keyWrapper, byte[] value, IShellItem parent = null)
        {
            try
            {
                byte type = BlockHelper.UnpackByte(value, 0x02);
                return (type & 0x70) == 0x10 && KnownTypes.Contains(type);
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

                string typename = "Root Folder";

                string guid = BlockHelper.UnpackGuid(value, 0x04);
                string maybeSearchFolderGuid = (size > 113 + 16) ? BlockHelper.UnpackGuid(value, 113) : "n/a";

                string subtypename = string.Empty;
                string rootfolderguid = string.Empty;
                string rootfoldername = string.Empty;
                string description = string.Empty;
                uint signature = 0;
                if (Config.KnownGuids.ContainsKey(guid))
                {
                    subtypename = "GUID";
                    rootfolderguid = guid;
                    description = rootfoldername = Config.KnownGuids[rootfolderguid];
                }
                else if (BlockHelper.UnpackDWord(value, 0x06) == 0xf5a6b710 && BlockHelper.UnpackWord(value, 0x0A) > 0)
                {
                    subtypename = "Removable Drive";
                    signature = 0xf5a6b710;
                    description = rootfoldername = BlockHelper.UnpackString(value, 0x0D);
                }
                else if (BlockHelper.UnpackDWord(value, 0x06) == 0x23a3dfd5 && Config.KnownGuids.ContainsKey(maybeSearchFolderGuid) && Config.KnownGuids[maybeSearchFolderGuid] == "Search Folder")
                {
                    /* TODO
                    fields["Subtype"] = "Search Folder";
                    fields["Signature"] = 0x23a3dfd5;

                    byte[] internalvalue = new byte[Size - 167];
                    Array.Copy(value, 167, internalvalue, 0, internalvalue.Length);

                    IShellItem a = ShellItem.FromByteArray(internalvalue);
                    */

                    throw new ArgumentException("byte array could not be parsed into RootFolderShellItem", new NotImplementedException("Search Folder shell item not implemented"));
                }
                else
                {
                    throw new ArgumentException("byte array could not be parsed into RootFolderShellItem", new ArgumentException(string.Format("Unknown Root Folder GUID {0}", guid)));
                }

                byte sortindex = BlockHelper.UnpackByte(value, 0x03);
                string sortindexdescription = string.Empty;

                if (subtypename != "Removable Drive")
                {
                    switch(sortindex)
                    {
                        case 0x00:
                            sortindexdescription = "INTERNET_EXPLORER";
                            break;
                        case 0x42:
                            sortindexdescription = "LIBRARIES";
                            break;
                        case 0x44:
                            sortindexdescription = "USERS";
                            break;
                        case 0x48:
                            sortindexdescription = "MY_DOCUMENTS";
                            break;
                        case 0x50:
                            sortindexdescription = "MY_COMPUTER";
                            break;
                        case 0x58:
                            sortindexdescription = "NETWORK";
                            break;
                        case 0x60:
                            sortindexdescription = "RECYCLE_BIN";
                            break;
                        case 0x68:
                            sortindexdescription = "INTERNET_EXPLORER";
                            break;
                        case 0x70:
                            sortindexdescription = "UNKNOWN";
                            break;
                        case 0x80:
                            sortindexdescription = "MY_GAMES";
                            break;
                        default:
                            break;
                    }
                }

                RootFolderShellItem item = new RootFolderShellItem()
                {
                    Size = size,
                    Type = type,
                    Signature = signature,
                    TypeName = typename,
                    SubtypeName = subtypename,
                    RootFolderGuid = rootfolderguid,
                    SortIndex = sortindex,
                    SortIndexDescription = sortindexdescription,
                    Place = subtypename == "Removable Drive" ?
                        new RemovableDrive()
                        {
                            Name = rootfoldername,
                            PathName = parent != null ? Path.Join(parent.Place.PathName, parent.Place.Name) : null,
                        }
                        :
                        new SystemFolder()
                        {
                            Name = rootfoldername,
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
            catch (ArgumentException)
            {
                return null;
            }
            catch (ShellParserException)
            {
                return null;
            }
        }
    }
}
