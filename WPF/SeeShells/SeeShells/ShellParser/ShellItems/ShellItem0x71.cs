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
    /// Control Panel Shell Item, with a constant GUID for identification.
    /// </summary>
    public class ShellItem0x71 : ShellItem
    {
        public string Guid { get; protected set; }
        public byte Flags { get; protected set; }
        public override string TypeName { get => "Control Panel";}
        public override string Name
        {
            get
            {
                if (KnownGuids.dict.ContainsKey(Guid))
                {
                    return string.Format("{{CONTROL PANEL: {0}}}", KnownGuids.dict[Guid]);
                }
                else
                {
                    return string.Format("{{CONTROL PANEL: {0}}}", Guid);
                }
            }
        }
        public ShellItem0x71(byte[] buf)
            : base(buf)
        {
            Guid = unpack_guid(0x0E);
            Flags = unpack_byte(0x03);
        }
        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, Constants.GUID, Guid);
            AddPairIfNotNull(ret, Constants.FLAGS, Flags);
            return ret;
        }
    }
}