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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SeeShells.UI.Windows;

namespace SeeShells.UI.Node
{
    public class InfoBlock : TextBlock
    {
        public IEvent aEvent;

        /// <summary>
        /// The InfoBlock displays the information held in an IEvent graphically on the timeline.
        /// </summary>
        public InfoBlock()
        {
            this.aEvent = null;
        }

        /// <summary>
        /// The InfoBlock displays the information held in an IEvent graphically on the timeline.
        /// </summary>
        /// <param name="aEvent">object that stores shellbag data and current event information</param>
        public InfoBlock(IEvent aEvent)
        {
            this.aEvent = aEvent;
        }

        private string GetInfo()
        {
            string text = "";
            
            foreach (KeyValuePair<string, string> property in this.aEvent.Parent.GetAllProperties())
            {
                TimeZoneInfo time = TimeZoneInfo.Local;
                if (property.Key.Contains("Date"))
                {
                    DateTime timeChange = Convert.ToDateTime(property.Value);
                    timeChange = TimeZoneInfo.ConvertTimeFromUtc(timeChange, time);
                    string timeFix = timeChange.ToString();
                    text += AddSpacesToCamelCase(property.Key) + ": " + timeFix;
                    text += "\n";
                }
                else
                {
                    text += AddSpacesToCamelCase(property.Key) + ": " + property.Value;
                    text += "\n";

                }
            };

            return text;
        }

        private string AddSpacesToCamelCase(string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value, "[A-Z]", " $0");
        }

        /// <summary>
        /// This enlarges the textblock so more information can be seen about the shellItem.
        /// </summary>
        public void ToggleInfo()
        {
            if(this.Width >= 250)
            {
                this.Text = "";
                this.Width = 200;
                this.Height = 70;
                this.Text += this.aEvent.Name + "\n";
                this.Text += this.aEvent.EventTime + "\n";
                this.Text += this.aEvent.EventType + "\n";
                this.TextAlignment = TextAlignment.Center;
            }
            else
            {
                this.Width = 350;
                this.Height = Double.NaN;
                this.TextAlignment = TextAlignment.Left;
                this.Text = GetInfo();
            }
        }

        /// <summary>
        /// This creates a window of the event information so that it can be moved around on the screen.
        /// </summary>
        public void PopOutInfo()
        {
            EventInformationWindow info = new EventInformationWindow()
            {
                EventTitle = this.aEvent.Name,
                EventBody = GetInfo()
            };

            info.Show();
        }
    }
}
