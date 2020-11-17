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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells;
using SeeShells.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI
{
    [TestClass()]
    class EventCollectionTest
    {
        /// <summary>
        /// Unit test to ensure that the list of IEvents created in the EventCollection class
        /// can be accessed through an instance of the class created in App.xaml.cs 
        /// </summary>
        [TestMethod()]
        public void EventCollectionTests()
        {
            List<IEvent> eventList = new List<IEvent>();
            Event aEvent = new Event("item1", DateTime.Now, null, "Access");
            eventList.Add(aEvent);
            App.eventCollection.eventList = eventList;
            Assert.AreNotEqual(App.eventCollection.eventList.Count, 0);

        }
    }
}
