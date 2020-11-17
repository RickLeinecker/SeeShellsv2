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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI
{
    public class Event : IEvent
    {
        /// <summary>
        /// Constructor for the Event class that takes in the parameters 
        /// listed below in order to create the elements of an event object. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventTime"></param>
        /// <param name="parent"></param>
        /// <param name="eventType"></param>
        public Event(string name, DateTime eventTime, IShellItem parent, string eventType)
        {
            Name = name;
            EventTime = eventTime;
            Parent = parent;
            EventType = eventType;
        }
        /// <summary>
        /// Identifier for the entity which was modified at this particular <see cref="EventTime"/>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Details the time in which an event took place.
        /// </summary>
        public DateTime EventTime { get; set; }
        /// <summary>
        /// The ShellItem which this event was derived from.
        /// Provides optional extra information to the formulation of this event.
        /// </summary>
        public IShellItem Parent { get; set; }
        /// <summary>
        /// Categorizes the action which was preformed.
        /// </summary>
        public string EventType { get; set; }

        public TimeZone timeZone { get
            {
                TimeZone curTimeZone = TimeZone.CurrentTimeZone;
                return curTimeZone;
            }
        }
    }
}
