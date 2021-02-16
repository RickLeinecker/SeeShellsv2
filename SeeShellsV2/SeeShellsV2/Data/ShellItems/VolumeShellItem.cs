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
    /// Shell items that document Windows Volumes:
    /// 0x23, 0x25, 0x29, 0x2A, 0x2E, 0x2F
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/main/documentation/Windows%20Shell%20Item%20format.asciidoc#volume_shell_item
    public class VolumeShellItem : ShellItem, IShellItem
    {
        public static IReadOnlySet<byte> KnownTypes = new HashSet<byte>{
        /*  0x23, 0x25, 0x29, 0x2A, 0x2E, 0x2F  */               //libwsi
            0x20, 0x21, 0x23, 0x25, 0x28, 0x29, 0x2A, 0x2E, 0x2F // v1 team
        };

        [Flags]
        public enum TypeFlags
        {
            None = 0x00,
            HasName = 0x01,
            Unknown1 = 0x02,
            Unknown2 = 0x04,
            IsRemovable = 0x08
        }

        public string VolumeName
        {
            init => fields["VolumeName"] = value;
            get => fields.GetClassOrDefault("VolumeName", string.Empty);
        }

        public VolumeShellItem() { }

        public VolumeShellItem(byte[] buf)
        {
            try
            {
                fields["Size"] = Block.UnpackWord(buf, 0x00);
                fields["Type"] = Block.UnpackByte(buf, 0x02);

                fields["TypeName"] = "Volume";

                // if HasName bit is true
                if (((TypeFlags)Type & TypeFlags.HasName) != TypeFlags.None)
                {
                    fields["SubtypeName"] = "Named";
                    fields["VolumeName"] = Block.UnpackString(buf, 0x03);
                    fields["Description"] = VolumeName;
                }
                else
                {
                    string guid = Block.UnpackGuid(buf, 0x04);
                    if (KnownGuids.dict.ContainsKey(guid))
                    {
                        fields["Description"] = fields["VolumeName"] = KnownGuids.dict[guid];
                    }
                    else
                    {
                        fields["Description"] = fields["VolumeName"] = guid;
                    }
                }

                // if IsRemovable bit is true
                if (((TypeFlags)Type & TypeFlags.IsRemovable) != TypeFlags.None)
                {
                    fields["SubtypeName"] = "Removable Media";
                }
            }
            catch (ShellParserException ex)
            {
                throw new ArgumentException("byte array could not be parsed into VolumeShellItem", ex);
            }
        }
    }
}