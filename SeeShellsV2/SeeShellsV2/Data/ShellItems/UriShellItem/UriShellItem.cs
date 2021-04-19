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
            init => fields[nameof(Uri)] = value;
            get => fields.GetClassOrDefault(nameof(Uri), string.Empty);
        }

        public string FTPHostname
        {
            init => fields[nameof(FTPHostname)] = value;
            get => fields.GetClassOrDefault(nameof(FTPHostname), string.Empty);
        }

        public string FTPUsername
        {
            init => fields[nameof(FTPUsername)] = value;
            get => fields.GetClassOrDefault(nameof(FTPUsername), string.Empty);
        }

        public string FTPPassword
        {
            init => fields[nameof(FTPPassword)] = value;
            get => fields.GetClassOrDefault(nameof(FTPPassword), string.Empty);
        }

        public UriFlagBits UriFlags
        {
            init => fields[nameof(UriFlags)] = (int)value;
            get => (UriFlagBits)fields.GetStructOrDefault(nameof(UriFlags), 0);
        }
                        
        public DateTime ConnectedDate
        {
            init => fields[nameof(ConnectedDate)] = value;
            get => fields.GetStructOrDefault(nameof(ConnectedDate), DateTime.MinValue);
        }
    }
}