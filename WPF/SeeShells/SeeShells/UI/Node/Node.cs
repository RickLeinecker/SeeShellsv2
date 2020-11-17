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
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace SeeShells.UI.Node
{
    public class Node : ToggleButton
    {
        public IEvent aEvent;
        public InfoBlock block;
        public TextBlock alignmentBlock { get; set; }

        /// <summary>
        /// The Node is an object that stores event data and has graphical objects to be displayed on a timeline.
        /// </summary>
        /// <param name="aEvent">object that stores event/shellbag data</param>
        /// <param name="block">object to display event details on a timeline</param>
        public Node(IEvent aEvent, InfoBlock block)
        {
            this.aEvent = aEvent;
            this.block = block;
        }

        public DateTime GetBlockTime()
        {
            return aEvent.EventTime;
        }

        /// <summary>
        /// This is used to hide and show the block of information connected to each dot of information on the timeline.
        /// </summary>
        public void ToggleBlock()
        {
            if (this.block.Visibility == Visibility.Collapsed)
            {
                this.alignmentBlock.Visibility = Visibility.Hidden;
                this.block.Visibility = Visibility.Visible;
            }
            else if (this.block.Visibility == Visibility.Visible)
            {
                this.alignmentBlock.Visibility = Visibility.Collapsed;
                this.block.Visibility = Visibility.Collapsed;
            }
        }
    }
}
