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
        [Flags]
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
            init => fields[nameof(NetworkFlags)] = (int)value;
            get => (NetworkFlagBits)fields.GetStructOrDefault<int>(nameof(NetworkFlags), 0);
        }

        public string NetworkLocation
        {
            init => fields[nameof(NetworkLocation)] = value;
            get => fields.GetClassOrDefault(nameof(NetworkLocation), string.Empty);
        }

        public string NetworkDescription
        {
            init => fields[nameof(NetworkDescription)] = value;
            get => fields.GetClassOrDefault(nameof(NetworkDescription), string.Empty);
        }

        public string NetworkComments
        {
            init => fields[nameof(NetworkComments)] = value;
            get => fields.GetClassOrDefault(nameof(NetworkComments), string.Empty);
        }
    }
}