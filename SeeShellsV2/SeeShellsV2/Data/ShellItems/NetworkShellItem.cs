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

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Shell items that document Windows network properties:
    /// 0x41, 0x42, 0x46, 0x47, 0x4C, 0xC3
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#network_location_shell_item
    public class NetworkShellItem : ShellItem, IShellItem
    {
        public static IReadOnlySet<byte> KnownTypes = new HashSet<byte>{
        /*  0x41, 0x42, 0x46, 0x47, 0x4C, 0xC3  */         // libwsi
            0x41, 0x42, 0x43, 0x46, 0x47, 0x4D, 0x4E, 0xC3 // v1 team
        };

        public enum SubtypeFlags
        {
            None = 0x00,
            DomainName = 0x01,
            ServerUNCPath = 0x02,
            ShareUNCPath = 0x03,
            MicrosoftWindowsNetwork = 0x06,
            EntireNetwork = 0x07,
            NetworkPlacesRoot = 0x0D,
            NetworkPlacesServer = 0x0E,
            Unknown = 0x80
        }

        [Flags]
        public enum NetworkFlagBits
        {
            None = 0x00,
            Unknown1 = 0x01,
            Unknown2 = 0x02,
            Unknown4 = 0x04,
            HasComments = 0x40,
            HasDescription = 0x80
        }

        public NetworkFlagBits NetworkFlags
        {
            init => fields["NetworkFlags"] = (int)value;
            get => (NetworkFlagBits)fields.GetStructOrDefault<int>("NetworkFlags", 0);
        }

        public string NetworkLocation
        {
            init => fields["NetworkLocation"] = value;
            get => fields.GetClassOrDefault("NetworkLocation", string.Empty);
        }

        public string NetworkDescription
        {
            init => fields["NetworkDescription"] = value;
            get => fields.GetClassOrDefault("NetworkDescription", string.Empty);
        }

        public string NetworkComments
        {
            init => fields["NetworkComments"] = value;
            get => fields.GetClassOrDefault("NetworkComments", string.Empty);
        }

        public NetworkShellItem() { }

        public NetworkShellItem(byte[] buf)
        {
            try
            {
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Type"] = Block.UnpackByte(buf, 0x02);

                fields["TypeName"] = "Network NetworkLocation";

                if ((Type & 0x0F) == (byte)SubtypeFlags.DomainName)
                    fields["SubtypeName"] = "Domain/WorkGroup Description";
                else if ((Type & 0x0F) == (byte)SubtypeFlags.ServerUNCPath)
                    fields["SubtypeName"] = "Server UNC Path";
                else if ((Type & 0x0F) == (byte)SubtypeFlags.ShareUNCPath)
                    fields["SubtypeName"] = "Share UNC Path";
                else if ((Type & 0x0F) == (byte)SubtypeFlags.MicrosoftWindowsNetwork)
                    fields["SubtypeName"] = "Microsoft Windows Network";
                else if ((Type & 0x0F) == (byte)SubtypeFlags.EntireNetwork)
                    fields["SubtypeName"] = "Entire Network";
                else if ((Type & 0x0F) == (byte)SubtypeFlags.NetworkPlacesRoot)
                    fields["SubtypeName"] = "NetworkPlaces";
                else if ((Type & 0x0F) == (byte)SubtypeFlags.NetworkPlacesServer)
                    fields["SubtypeName"] = "NetworkPlaces";

                fields["NetworkFlags"] = (int)Block.UnpackByte(buf, 0x04);
                fields["NetworkLocation"] = Block.UnpackString(buf, 0x05);

                int off = 0x05;
                off += NetworkLocation.Length + 1;

                if (NetworkFlags.HasFlag(NetworkFlagBits.HasDescription))
                {
                    fields["NetworkDescription"] = Block.UnpackString(buf, off);
                    off += NetworkDescription.Length + 1;
                }

                if (NetworkFlags.HasFlag(NetworkFlagBits.HasComments))
                {
                    fields["NetworkComments"] = Block.UnpackString(buf, off);
                }

                fields["Description"] = fields["NetworkLocation"];
            }
            catch (ShellParserException ex)
            {
                throw new ArgumentException("byte array could not be parsed into NetworkShellItem", ex);
            }
        }
    }
}