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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.UI.Node;

namespace SeeShells.UI.EventFilters {
    /// <summary>
    /// Filter's a list of <see cref="Node.Node"/>s by a specific <see cref="IEvent.EventTime"/> criteria.
    /// </summary>
    public class EventTypeFilter : INodeFilter
    {
        private readonly string[] eventTypes;

        /// <summary>
        /// Filter's a list of <see cref="Node.Node"/>s by a specific <see cref="IEvent.EventTime"/> criteria.
        /// </summary>
        /// <param name="eventTypes">One or more acceptable types to filter on. </param>
        public EventTypeFilter(params string[] eventTypes)
        {
            this.eventTypes = eventTypes;
        }
        public void Apply(ref List<Node.Node> nodes)
        {
            if (eventTypes.Length == 0)
            {
                return; //dont apply filter if no filters
            }

            for (int i = nodes.Count-1; i >= 0; i--)//iterate backwards because iterating forwards would be an issue with a list of changing size.
            {
                Node.Node node = nodes[i];
                IEvent nEvent = node.aEvent;

                bool acceptableType = false;
                foreach (string type in eventTypes)
                {
                    if (nEvent.EventType.Trim().Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        acceptableType = true;
                        break;
                    }
                }

                if (!acceptableType)
                {
                    node.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
    }

}
