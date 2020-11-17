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
    /// <summary>
    /// Supposedly corresponds to ShellItem 0x23 (as originally given)- but 
    /// a conflicting signature appears here: https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#33-volume-shell-item
    /// </summary>
    public class SHITEM_UNKNOWNENTRY2 : ShellItem
    {
        public string Guid { get; protected set; }
        public byte Flags { get; protected set; }
        public override string Name
        {
            get
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
        }
        public SHITEM_UNKNOWNENTRY2(byte[] buf, int offset, object parent)
            : base(buf, offset)
        {
            Flags = unpack_byte(0x03);
            Guid = unpack_guid(0x04);
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, Constants.FLAGS, Flags);
            AddPairIfNotNull(ret, Constants.GUID, Guid);
            return ret;
        }

    }
}