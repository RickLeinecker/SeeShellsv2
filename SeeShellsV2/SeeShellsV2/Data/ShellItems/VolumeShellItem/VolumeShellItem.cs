using System;
using System.Collections.Generic;

namespace SeeShellsV2.Data
{
    /// <summary>
    /// Shell items that document Windows Volumes:
    /// 0x23, 0x25, 0x29, 0x2A, 0x2E, 0x2F
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#volume_shell_item
    public class VolumeShellItem : ShellItem, IShellItem
    {
        public enum TypeFlags
        {
            None = 0x00,
            SystemFolder = 0x0e,
            LocalDisk = 0x0f,
        }
    }
}