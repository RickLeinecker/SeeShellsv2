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
using SeeShells.UI;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;
using SeeShellsTests.UI.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI.Tests
{
    [TestClass()]
    public class NodeFilterTests
    {

        private int countVisible(List<Node> nodes)
        {
            return (from node in nodes
                    where node.Visibility == System.Windows.Visibility.Visible
                    select node).Count();
        }

        private void resetVisibility(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.Visibility = System.Windows.Visibility.Visible;
            }
        }

        [TestMethod()]
        public void FilterDateRangeTest()
        {
            var eventList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item2", DateTime.MaxValue, null, "Access"),
                new MockEvent("item3", DateTime.MinValue, null, "Access"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };
            var startListSize = testList.Count;

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.


            DateTime startLimit = new DateTime(DateTime.MinValue.Ticks + 10000000000000, DateTimeKind.Utc);
            DateTime endLimit = new DateTime(DateTime.MaxValue.Ticks - 10000, DateTimeKind.Utc);

            //test set startdate and enddate
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new DateRangeFilter(startLimit, endLimit).Apply(ref testListCopy);
            Assert.AreEqual(1, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);


            //test the startdate limit
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new DateRangeFilter(startLimit, null).Apply(ref testListCopy);
            Assert.AreEqual(2, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //test enddate limit
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new DateRangeFilter(null, endLimit).Apply(ref testListCopy);
            Assert.AreEqual(2, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //test full range (aka pointless filter)
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new DateRangeFilter(null, null).Apply(ref testListCopy);
            Assert.AreEqual(3, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

        }

        [TestMethod()]
        public void FilterEventTypeTest()
        {
            var eventList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item2", DateTime.MaxValue, null, "Create"),
                new MockEvent("item3", DateTime.MinValue, null, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };
            var startListSize = testList.Count;

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.


            //basic filter
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventTypeFilter("Access").Apply(ref testListCopy);
            Assert.AreEqual(1, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //test multifilter
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventTypeFilter("Access", "Create").Apply(ref testListCopy);
            Assert.AreEqual(2, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //test no filter
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventTypeFilter("").Apply(ref testListCopy);
            Assert.AreEqual(0, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

        }

        [TestMethod()]
        public void FilterParentTest()
        {
            var parent1 = new MockShellItem();
            var parent2 = new MockShellItem();
            var eventList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, parent1, "Access"),
                new MockEvent("item2", DateTime.MaxValue, parent1, "Create"),
                new MockEvent("item3", DateTime.MinValue, parent2, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };
            var startListSize = testList.Count;

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.


            //check same parent
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventParentFilter(parent1).Apply(ref testListCopy);
            Assert.AreEqual(2, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //check diff parent
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventParentFilter(parent2).Apply(ref testListCopy);
            Assert.AreEqual(1, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //check unknown parent
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventParentFilter(new MockShellItem()).Apply(ref testListCopy);
            Assert.AreEqual(0, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

        }

        [TestMethod()]
        public void FilterNameTest()
        {
            var eventList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item1", DateTime.MaxValue, null, "Create"),
                new MockEvent("item3", DateTime.MinValue, null, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };
            var startListSize = testList.Count;


            List<Node> testListCopy; //reset this list each test because we are removing references from this list.


            //check same name
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventNameFilter("item1").Apply(ref testListCopy);
            Assert.AreEqual(2, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //check no name
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventNameFilter("item2").Apply(ref testListCopy);
            Assert.AreEqual(0, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //check multi names
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new EventNameFilter("item1", "item3").Apply(ref testListCopy);
            Assert.AreEqual(3, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

        }

        [TestMethod()]
        public void FilterAnyStringTest()
        {
            var parent1 = new MockShellItem("item1", 0x02);
            var parent2 = new MockShellItem("item3", 0x02);

            var eventList = new List<MockEvent>()
            {
                new MockEvent(parent1.Name, DateTime.Now, parent1, "Access"),
                new MockEvent(parent1.Name, DateTime.MaxValue, parent1, "Create"),
                new MockEvent(parent2.Name, DateTime.MinValue, parent2, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };
            var startListSize = testList.Count;

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.



            //check for unique attribute to some items
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new AnyStringFilter("item1",false).Apply(ref testListCopy);
            Assert.AreEqual(2, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //check for partial match to all items
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new AnyStringFilter("item", false).Apply(ref testListCopy);
            Assert.AreEqual(3, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //check for regex expression ( .* = match all)
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new AnyStringFilter(".*", true).Apply(ref testListCopy);
            Assert.AreEqual(3, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //check for external attribute matching outside of IEvent (test type byte value)
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new AnyStringFilter("02", false).Apply(ref testListCopy);
            Assert.AreEqual(3, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //test for no results
            resetVisibility(testList);
            testListCopy = new List<Node>(testList);
            new AnyStringFilter("SeeShell", false).Apply(ref testListCopy);
            Assert.AreEqual(0, countVisible(testListCopy));
            Assert.AreEqual(startListSize, testListCopy.Count);

            //test for regex timeout 
            //TODO Find hard enough regex that actually takes time no matter the PC?
        }

        [TestMethod]
        public void FilterUserTest()
        {
            var user1 = "User1";
            var user2 = "User2";

            var parent1 = new MockShellItem();
            parent1.AddProperty("RegistryOwner", user1);

            var parent2 = new MockShellItem();
            parent2.AddProperty("RegistryOwner", user2);

            var eventList = new List<MockEvent>()         
            {
                new MockEvent("item1", DateTime.Now, parent1, "Access"),
                new MockEvent("item2", DateTime.MaxValue, parent1, "Create"),
                new MockEvent("item3", DateTime.MinValue, parent2, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };
            var startListSize = testList.Count;


            //test same name
            resetVisibility(testList);
            new EventUserFilter(user1).Apply(ref testList);
            Assert.AreEqual(2, countVisible(testList));
            Assert.AreEqual(startListSize, testList.Count);

            //test no name
            resetVisibility(testList);
            new EventUserFilter().Apply(ref testList);
            Assert.AreEqual(3, countVisible(testList));
            Assert.AreEqual(startListSize, testList.Count);

            //test multiName
            resetVisibility(testList);
            new EventUserFilter(user1, user2).Apply(ref testList);
            Assert.AreEqual(3, countVisible(testList));
            Assert.AreEqual(startListSize, testList.Count);


        }

    }
}