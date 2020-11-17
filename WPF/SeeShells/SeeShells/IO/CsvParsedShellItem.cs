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
using NLog;

namespace SeeShells.IO
{
    public class CsvParsedShellItem : IShellItem
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public virtual ushort Size { get => Convert.ToUInt16(allProperties[Constants.SIZE], 16); }
        public virtual byte Type { get => Convert.ToByte(allProperties[Constants.TYPE], 16); }
        public virtual string TypeName { get => allProperties[Constants.TYPENAME]; }
        public virtual string Name { get => allProperties[Constants.NAME]; }
        public virtual DateTime ModifiedDate { get => DateTime.Parse(allProperties[Constants.MODIFIED_DATE]); }
        public virtual DateTime AccessedDate { get => DateTime.Parse(allProperties[Constants.ACCESSED_DATE]); }
        public virtual DateTime CreationDate { get => DateTime.Parse(allProperties[Constants.CREATION_DATE]); }

        IDictionary<string, string> allProperties;

        public CsvParsedShellItem(IDictionary<string, string> allProperties)
        {
            this.allProperties = allProperties;
            
            //check to make sure all the required minimum fields exist within the CSV file
            //required because of C# properties lazy evaluation may see the issue only during first use.
            string[] constants = {
                Constants.SIZE, Constants.TYPE, Constants.TYPENAME, Constants.NAME, Constants.MODIFIED_DATE,
                Constants.ACCESSED_DATE, Constants.CREATION_DATE
            };
            foreach (string constant in constants)
            {
                try
                {
                    string unused = allProperties[constant];
                }
                catch (KeyNotFoundException ex)
                {
                    logger.Error(ex, $"Invalid Shellbag CSV Record. Missing required column {constant}");
                    throw; //unusable shellItem, dont create the object.
                }
            }
        }

        public virtual IDictionary<string, string> GetAllProperties()
        {
            return allProperties;
        }
    }
}
