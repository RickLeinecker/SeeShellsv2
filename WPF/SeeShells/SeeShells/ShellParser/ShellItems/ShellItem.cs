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
using System.Reflection;
using System.Text;


namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem : Block, IShellItem
    {
        public virtual ushort Size { get; protected set; }
        public virtual byte Type { get; protected set; }

        public virtual string TypeName
        {
            get
            {
                return "Unknown";
            }
            protected set
            {

            }
        }

        public virtual string Name
        {
            get
            {
                return "??";
            }
            protected set
            {

            }
        }
        public virtual DateTime ModifiedDate
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        public virtual DateTime AccessedDate
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        public virtual DateTime CreationDate
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        [System.Obsolete("Deprecated. Use 1-parameter Constructor. Offset parameter has no effect.")]
        public ShellItem(byte[] buf, int offset)
            : base(buf, offset)
        {
            Type = unpack_byte(0x02);
            Size = unpack_word(0x00);
        }

        public ShellItem(byte[] buf) : base(buf, 0) {
            Type = unpack_byte(0x02);
            Size = unpack_word(0x00);
        }

        public virtual IDictionary<string, string> GetAllProperties()
        {
            SortedDictionary<string, string> properties = new SortedDictionary<string, string>();
            AddPairIfNotNull(properties, Constants.SIZE, Size.ToString("X2")); //hexidecimal with 2 numerical places (aka a byte)
            AddPairIfNotNull(properties, Constants.TYPE, Type.ToString("X2"));
            AddPairIfNotNull(properties, Constants.TYPENAME, TypeName);
            AddPairIfNotNull(properties, Constants.NAME, Name);
            AddPairIfNotNull(properties, Constants.MODIFIED_DATE, ModifiedDate);
            AddPairIfNotNull(properties, Constants.ACCESSED_DATE, AccessedDate);
            AddPairIfNotNull(properties, Constants.CREATION_DATE, CreationDate);
            return properties;
        }

    }
}
