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
using System.Windows;
using System.Xaml;
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.UI.EventFilters
{
    /// <summary>
    /// Filters Events by <see cref="IShellItem"/>'s which have a property set specifying the owner of the registry object.
    /// </summary>
    public class EventUserFilter : INodeFilter
    {
        private readonly HashSet<string> acceptableUsers;

        /// <summary>
        /// Filters Events by <see cref="IShellItem"/>'s which have a property set specifying the owner of the registry object.
        /// If multiple Users are specified, returned events are one of the specified types.
        /// </summary>
        /// <param name="users">one or more acceptable user (names, SID, etc.) to filter on.</param>
        public EventUserFilter(params string[] users)
        {
            acceptableUsers = new HashSet<string>(users);
        }

        public void Apply(ref List<Node.Node> nodes)
        {
            if (acceptableUsers.Count == 0)
            {
                return; //dont apply filter if no users to filter on
            }

            //iterate backwards because iterating forwards would be an issue with a list of changing size.
            for (int i = nodes.Count - 1; i >= 0; i--) 
            {
                Node.Node node = nodes[i];
                IShellItem nParent = node.aEvent.Parent;

                var props = nParent.GetAllProperties();
                var keyName = "RegistryOwner";

                //check for the property and if it exists, verify its a user we want
                if (props.ContainsKey(keyName))
                {
                    if (!acceptableUsers.Contains(props[keyName]))
                    {
                        node.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    node.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}