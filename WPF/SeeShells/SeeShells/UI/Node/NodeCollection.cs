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
using SeeShells.UI.EventFilters;
using System.Collections.Generic;
using System.Windows;

namespace SeeShells.UI.Node
{
    public class NodeCollection
    {
        /// <summary>
        /// Creates a global list of Nodes to be accessed through an Instance of the class created in App.xaml.cs
        /// </summary>
        public List<Node> nodeList = new List<Node>();

        Dictionary<string, INodeFilter> filterList = new Dictionary<string, INodeFilter>();


        /// <summary>
        ///  applies a filter to the nodeList 
        /// recalculates the <see cref="nodeList"/> upon adding a filter
        /// </summary>
        /// <param name="identifier">A unique identifer that will be used to identify the filtering instance</param>
        /// <param name="filter">a filter that will be applied to remove nodes from a collection. </param>

        public void AddEventFilter(string identifier, INodeFilter filter)
        {
            filterList.Add(identifier, filter);
            filter.Apply(ref nodeList);
        }

        /// <summary>
        /// Removes a previously added <see cref="INodeFilter"/>
        /// recalculates the <see cref="nodeList"/> upon removing a filter
        /// </summary>
        /// <param name="identifier">A unique identifer that will be used to identify the filtering instance</param>
        public void RemoveEventFilter(string filterIdentifer)
        {
            bool didRemove = filterList.Remove(filterIdentifer);

            if (didRemove)
            {
                //restore visibility of previously filtered events
                foreach (var node in nodeList)
                {
                    node.Visibility = System.Windows.Visibility.Visible;
                }
                //reapply all remaining filters
                foreach (INodeFilter filter in filterList.Values)
                {
                    filter.Apply(ref nodeList);
                }
            }
        }

        public void ClearAllFilters()
        {
            filterList.Clear();
            foreach (var node in nodeList)
            {
                node.Visibility = Visibility.Visible;
            }
        }

    }
}
