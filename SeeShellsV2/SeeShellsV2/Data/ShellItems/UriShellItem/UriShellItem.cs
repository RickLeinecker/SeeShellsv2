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
    /// Shell items that document URIs:
    /// 0x61
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#37-uri-shell-item
    public class UriShellItem : ShellItem, IShellItem, IConnectedTimestamp
    {
        [Flags]
        public enum UriFlagBits
        {
            None = 0x00,
            HasUnicodeStrings = 0x80
        }

        public string Uri
        {
            init => fields["Uri"] = value;
            get => fields.GetClassOrDefault("Uri", string.Empty);
        }

        public string FTPHostname
        {
            init => fields["FTPHostname"] = value;
            get => fields.GetClassOrDefault("FTPHostname", string.Empty);
        }

        public string FTPUsername
        {
            init => fields["FTPUsername"] = value;
            get => fields.GetClassOrDefault("FTPUsername", string.Empty);
        }

        public string FTPPassword
        {
            init => fields["FTPPassword"] = value;
            get => fields.GetClassOrDefault("FTPPassword", string.Empty);
        }

        public UriFlagBits UriFlags
        {
            init => fields["UriFlags"] = (int)value;
            get => (UriFlagBits)fields.GetStructOrDefault("UriFlags", 0);
        }
                        
        public DateTime ConnectedDate
        {
            init => fields["ConnectedDate"] = value;
            get => fields.GetStructOrDefault("ConnectedDate", DateTime.MinValue);
        }
    }
}