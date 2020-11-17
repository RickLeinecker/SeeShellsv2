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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;

namespace SeeShellsTests.UI.Mocks
{
    /// <summary>
    /// Filters out all but the specified nodes in the constructor
    /// </summary>
    public class MockNodeFilter : INodeFilter
    {
        private readonly Node[] acceptableNodes;

        public MockNodeFilter(params Node[] acceptableNodes)
        {
            this.acceptableNodes = acceptableNodes;
        }

        public void Apply(ref List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (!acceptableNodes.Contains(node))
                {
                    node.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}