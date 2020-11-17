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
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.ShellParser.Registry
{
    /// <summary>
    /// Wraps a IShellItem with the relevant Registry Metadata from which it was produced.
    /// Adds registry related properties to the <seealso cref="IShellItem.GetAllProperties"/> method return
    /// </summary>
    public class RegistryShellItemDecorator : IShellItem
    {
        private const string AbsolutePathIdentifier = "AbsolutePath";
        protected IShellItem BaseShellItem { get; }
        protected RegistryKeyWrapper RegKey { get; }

        /// <summary>
        /// Wraps a IShellItem with the relevant Registry Metadata from which it was produced.
        /// Adds registry related properties to the <seealso cref="IShellItem.GetAllProperties"/> method return
        /// </summary>
        /// <param name="shellItem">The Shellitem to be encapsulated. Cannot be null</param>
        /// <param name="regKey">The Wrapper containing the information to be added to the <see cref="IShellItem"/> Can't be Null.</param>
        /// <param name="parentShellItem">The Shellitem which when hierarchically organized, contains the Shellitem. Can be null </param>
        public RegistryShellItemDecorator(IShellItem shellItem, RegistryKeyWrapper regKey, IShellItem parentShellItem = null)
        {
            BaseShellItem = shellItem ?? throw new ArgumentNullException(nameof(shellItem));
            RegKey = regKey ?? throw new ArgumentNullException(nameof(regKey));
            AbsolutePath = SetAbsolutePath(parentShellItem);
        }

        public ushort Size => BaseShellItem.Size;
        public byte Type => BaseShellItem.Type;
        public string TypeName => BaseShellItem.TypeName ?? string.Empty;
        public string Name => BaseShellItem.Name ?? string.Empty;
        public DateTime ModifiedDate => BaseShellItem.ModifiedDate;
        public DateTime AccessedDate => BaseShellItem.AccessedDate;
        public DateTime CreationDate => BaseShellItem.CreationDate;

        //RegistryDecorator Specific properties
        public string AbsolutePath { get; }


        public IDictionary<string, string> GetAllProperties()
        {
            IDictionary<string, string> baseDict = BaseShellItem.GetAllProperties();
            
            baseDict[AbsolutePathIdentifier] = AbsolutePath;

            //add all registry key values
            if (RegKey.RegistryUser != string.Empty)
                baseDict[Constants.REGISTRY_OWNER] =  RegKey.RegistryUser;
            if (RegKey.RegistryUser != string.Empty)
                baseDict[Constants.REGISTRY_SID] = RegKey.RegistrySID;
            if (RegKey.RegistryPath != string.Empty)
                baseDict[Constants.REGISTRY_PATH] = RegKey.RegistryPath;
            if (RegKey.ShellbagPath != string.Empty)
                baseDict[Constants.SHELLBAG_PATH] = RegKey.ShellbagPath;
            if (RegKey.LastRegistryWriteDate != DateTime.MinValue)
                baseDict[Constants.LAST_REG_WRITE] = RegKey.LastRegistryWriteDate.ToString();
            if (RegKey.SlotModifiedDate != DateTime.MinValue)
                baseDict[Constants.SLOT_MODIFIED_DATE] = RegKey.SlotModifiedDate.ToString();


            return baseDict;
        }

        /// <summary>
        /// Reconstructs the Absolute File Path to find the Item represented by this ShellItem, by obtaining the names of all parents <br/>
        /// (i.e. "C:\Users\User\Desktop" when the ShellItem is the "Desktop" ShellItem Type)
        /// </summary>
        /// <param name="parentShellItem">The ShellItem which represents the hiearchical parent to the information in this ShellItem</param>
        /// <returns>A filepath that should contain enough information to find the original item and related parent Shellbag items</returns>
        protected string SetAbsolutePath(IShellItem parentShellItem)
        {
            if (parentShellItem == null) 
                return Name;
            
            IDictionary<string, string> parentProperties = parentShellItem.GetAllProperties();
            if (parentProperties.TryGetValue(AbsolutePathIdentifier, out string parentPath))
            {
                //replace instances of \\\ because thats cascading, \\ can exist in network paths and \ is normal.
                return $@"{parentPath}\{Name}".Replace("\\\\\\", "\\");
            }

            return Name;
        }
    }
}