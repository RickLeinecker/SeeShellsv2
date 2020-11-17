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
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using SeeShellsTests.UI.Mocks;

namespace SeeShellsTests.UI
{
    [TestClass()]
    public class NodeCollectionTest
    {
        private int CountVisible(IEnumerable<Node> nodes)
        {
            return (from node in nodes
                where node.Visibility == System.Windows.Visibility.Visible
                select node).Count();
        }

        /// <summary>
        /// Tests that a list of Nodes is created and saved in the NodeCollection object in App.xml.cs
        /// </summary>
        [TestMethod()]
        public void NodeCollectionCreationTest()
        {
            List<IEvent> eventList = new List<IEvent>();
            IEvent aEvent = new MockEvent("item1", DateTime.Now, null, "Access");
            eventList.Add(aEvent);
            App.nodeCollection.nodeList = NodeParser.GetNodes(eventList);
            Assert.AreNotEqual(App.nodeCollection.nodeList.Count, 0);
        }

        [TestMethod()]
        public void NodeCollectionClearAllTest()
        {
            IEvent event1 = new MockEvent("item1", DateTime.Now, null, "Access");
            IEvent event2 = new MockEvent("item2", DateTime.Now, null, "Creation");
            Node node1 = new MockNode(event1);
            Node node2 = new MockNode(event2);

            var nodeCollection = new NodeCollection {nodeList = new List<Node> {node1, node2}};

            Assert.AreEqual(2, CountVisible(nodeCollection.nodeList));
            nodeCollection.AddEventFilter("mockFilter", new MockNodeFilter(node1));
            Assert.AreEqual(1, CountVisible(nodeCollection.nodeList));
            nodeCollection.ClearAllFilters();
            Assert.AreEqual(2, CountVisible(nodeCollection.nodeList));

        }

    }
}
