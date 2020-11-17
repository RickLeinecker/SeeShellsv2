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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SeeShells.UI.Node
{
    public class StackedNodes : ToggleButton
    {
        public List<IEvent> events = new List<IEvent>();
        public List<InfoBlock> blocks = new List<InfoBlock>();
        public List<Node> nodes = new List<Node>();
        public TextBlock alignmentBlock { get; set; }

        public StackedNodes()
        {
            this.Width = 20;
            this.Height = 20;
            this.FontSize = 10;
            this.FontWeight = FontWeights.Bold;
        }

        public DateTime GetBlockTime()
        {
            return events[0].EventTime;
        }

        public void ToggleBlock()
        {
            foreach(InfoBlock block in this.blocks)
            {
                if (block.Visibility == Visibility.Collapsed)
                {
                    block.Visibility = Visibility.Visible;
                }
                else if (block.Visibility == Visibility.Visible)
                {
                    block.Visibility = Visibility.Collapsed;
                }
            }

            if (alignmentBlock.Visibility == Visibility.Collapsed)
                alignmentBlock.Visibility = Visibility.Hidden;
            else
                alignmentBlock.Visibility = Visibility.Collapsed;
        }
    }
}
