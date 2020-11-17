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
    public class ShellItem0x00 : ShellItem
    {
        public string Guid { get; protected set; }
        public override string Name
        {
            get
            {
                if (Size == 0x20)
                {
                    if (KnownGuids.dict.ContainsKey(Guid))
                    {
                        return string.Format("{{{0}}}", KnownGuids.dict[Guid]);
                    }
                    else
                    {
                        return string.Format("{{{0}}}", Guid);
                    }
                }
                else
                {
                    return "??";
                }
            }
        }
        public ShellItem0x00(byte[] buf) : base(buf)
        {
            if (Size == 0x20)
            {
                Guid = unpack_guid(0x0E);
            }
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, Constants.GUID, Guid);
            return ret;
        }
    }
}