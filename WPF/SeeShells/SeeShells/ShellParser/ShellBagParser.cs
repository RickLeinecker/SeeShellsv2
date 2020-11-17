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
using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using SeeShells.ShellParser.Registry;

namespace SeeShells.ShellParser
{
    /// <summary>
    /// This class is used to obtain and identify ShellBag items. 
    /// </summary>
    public static class ShellBagParser
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Identifies and gathers ShellBag items from raw binary registry data.
        /// </summary>
        /// <returns>a list of different variety ShellBag items</returns>
        public static List<IShellItem> GetShellItems(IRegistryReader registryReader)
        {
            List<IShellItem> shellItems = new List<IShellItem>();
            Dictionary<RegistryKeyWrapper, IShellItem> keyShellMappings = new Dictionary<RegistryKeyWrapper, IShellItem>(); 
            foreach (RegistryKeyWrapper keyWrapper in registryReader.GetRegistryKeys())
            {
                if(keyWrapper.Value != null) // Some Registry Keys are null
                {
                    ShellItemList shellItemList = new ShellItemList(keyWrapper.Value);
                    foreach (IShellItem shellItem in shellItemList.Items())
                    {

                        IShellItem parentShellItem = null;
                        //obtain the parent shellitem from the parent registry key (if it exists)
                        if (keyWrapper.Parent != null)
                        {
                            if (keyShellMappings.TryGetValue(keyWrapper.Parent, out IShellItem pShellItem))
                            {
                                parentShellItem = pShellItem;
                            }
                        }

                        RegistryShellItemDecorator decoratedShellItem = new RegistryShellItemDecorator(shellItem, keyWrapper, parentShellItem);
                        try
                        {
                            keyShellMappings.Add(keyWrapper, decoratedShellItem);
                        }
                        catch (ArgumentException ex)
                        {
                            //*should* never happen, if it does Absolute Path finding need to be reworked. Potentially should be a fatal exception
                            // as now the shellbags involved are misleading. (contain incomplete data) 
                            logger.Error(ex, $"Registry Item {keyWrapper.RegistryPath} already had an associated Shellbag ({keyShellMappings[keyWrapper].Name}), Absolute Path's are no longer accurate.");
                        }

                        shellItems.Add(decoratedShellItem);
                    }
                }
            }
            return shellItems;
        }
    }
}
