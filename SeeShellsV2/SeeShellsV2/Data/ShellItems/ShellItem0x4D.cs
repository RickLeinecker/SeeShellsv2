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
using System.Collections.Generic;

namespace SeeShellsV2.Data
{
    public class ShellItem0x4D : ShellItem0x40, IShellItem
    {
        // TODO (Devon): investigate whether this field is used
        // "this existed in the code before, no mention of a GUID for type 0x4D has been seen otherwise" - v1 team
        public string Guid
        {
            init => fields["Guid"] = value;
            get => fields.GetClassOrDefault("Guid", string.Empty);
        }

        public ShellItem0x4D() { }

        public ShellItem0x4D(byte[] buf) : base(buf)
        {
            fields["TypeName"] = "Network Location - NetworkPlaces";
            fields["Guid"] = Block.UnpackGuid(buf, 0x04); // documentation lists this as flags

            if (KnownGuids.dict.ContainsKey(Guid))
                fields["Name"] = string.Format("{{{0}}}", KnownGuids.dict[Guid]);
            else
                fields["Name"] = string.Format("{{{0}}}", Guid);
        }
    }
}