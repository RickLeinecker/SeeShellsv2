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
using SeeShells.ShellParser.ShellItems.ExtensionBlocks;
using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Indicates that this Shell Type can have 0 or more <see cref="IExtensionBlock"/> types included.
    /// </summary>
    public class ShellItemWithExtensions : ShellItem
    {
        public List<IExtensionBlock> ExtensionBlocks { get; private set; }
        
        public ShellItemWithExtensions(byte[] buf) : base(buf)
        {
            ExtensionBlocks = new List<IExtensionBlock>();
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret =  base.GetAllProperties();
            foreach (IExtensionBlock block in ExtensionBlocks)
            {
                var props = block.GetAllProperties();
                //TODO how do we populate Extension Blocks in addition to the shell Item Key-Value Pairs?
            }
            AddPairIfNotNull(ret, "ExtensionBlockCount", ExtensionBlocks.Count); //TODO REMOVE ME WHEN THE ABOVE IS ANSWERED.
            return ret;
        }
    }
}