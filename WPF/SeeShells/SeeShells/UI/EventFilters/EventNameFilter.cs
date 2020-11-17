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

namespace SeeShells.UI.EventFilters
{
    /// <summary>
    /// Filter's <see cref="Node.Node"/>s by a <see cref="IEvent.Name"/> property.
    /// If multiple <see cref="IEvent.Name"/> are specified, returned <see cref="Node.Node"/> are one of the specified types.
    /// </summary>
    public class EventNameFilter : INodeFilter
    {
        private readonly string[] names;

        /// <summary>
        /// Filter's <see cref="IEvent"/>s by their <see cref="IEvent.Name"/> property.
        /// If multiple <see cref="IEvent.Name"/> are specified, returned events are one of the specified types.
        /// </summary>
        /// <param name="names">One or more acceptable <see cref="string"/> to filter on. </param>
        public EventNameFilter(params string[] names)
        {
            this.names = names;
        }
        public void Apply(ref List<Node.Node> nodes)
        {
            if (names.Length == 1 && names[0].Equals(string.Empty))
                return;

            for (int i = nodes.Count-1; i >= 0; i--) //iterate backwards because iterating forwards would be an issue with a list of changing size.
            {
                Node.Node node = nodes[i];
                IEvent nEvent = node.aEvent;

                bool acceptableName = false;
                foreach (string name in names)
                {
                    if (nEvent.Name.Equals(name))
                    {
                        acceptableName = true;
                        break;
                    }
                }

                if (!acceptableName)
                {
                    node.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
    }
}
