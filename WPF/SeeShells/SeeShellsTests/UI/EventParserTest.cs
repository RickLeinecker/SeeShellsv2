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
using SeeShells.UI;
using SeeShellsTests.UI.Mocks;
using SeeShells.ShellParser;
using SeeShells.ShellParser.ShellItems;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.IO;

namespace SeeShellsTests.UI
{
    [TestClass()]
    public class EventParserTest
    {
        [TestMethod()]
        public void EventListOutput()
        {
            List<IShellItem> shellItems = new List<IShellItem>();
            Dictionary<string, string> shellItemProperties = new Dictionary<string, string>();
            shellItemProperties.Add("Size", "0");
            shellItemProperties.Add("Type", "31");
            shellItemProperties.Add("TypeName", "Some Type Name");
            shellItemProperties.Add("Name", "Some Name");
            shellItemProperties.Add("ModifiedDate", "1/1/2010 12:00:00 AM");
            shellItemProperties.Add("AccessedDate", "2/1/2000 12:00:00 AM");
            shellItemProperties.Add("CreationDate", "1/1/2019 12:00:00 AM");
            shellItemProperties.Add("LastAccessedDate", "1/1/2016 12:00:00 AM");
            CsvParsedShellItem ShellItem = new CsvParsedShellItem(shellItemProperties);
            shellItems.Add(ShellItem);
            List<IEvent> newList = EventParser.GetEvents(shellItems);
            Assert.IsNotNull(newList);
            foreach(var el in newList)
            {
                Assert.AreSame(el.Parent, ShellItem);
            }

            
        }
    }
}
