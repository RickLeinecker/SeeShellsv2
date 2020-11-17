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

namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x4D : ShellItem0x40
    {
        //this existed in the code before, no mention of a GUID for type 0x4D has been seen otherwise
        public string GUID { get; protected set; }

        public override string TypeName { get => "Network Location - NetworkPlaces"; }
        public override string Name
        {
            get
            {
                if ((Type & 0x0F) == 0x0D)
                {
                    if (KnownGuids.dict.ContainsKey(GUID))
                    {
                        return string.Format("{{{0}}}", KnownGuids.dict[GUID]);
                    }
                    else
                    {
                        return string.Format("{{{0}}}", GUID);
                    }
                }
                else
                {
                    return Location;
                }
            }
        }

        public ShellItem0x4D(byte[] buf) : base(buf)
        {
            GUID = unpack_guid(0x04);
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, Constants.GUID, GUID);
            return ret;
        }
    }
}