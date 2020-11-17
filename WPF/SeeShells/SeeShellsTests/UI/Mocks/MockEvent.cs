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
using SeeShells.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI.Mocks
{
    class MockEvent : IEvent
    {

        public MockEvent(string name, DateTime eventTime, IShellItem parent, string eventType)
        {
            Name = name;
            EventTime = eventTime;
            Parent = parent;
            EventType = eventType;
        }

        public string Name { get; set; }

        public DateTime EventTime { get; set; }

        public IShellItem Parent { get; set; }

        public string EventType { get; set; }

        public TimeZone timeZone { get; }
    }
}
