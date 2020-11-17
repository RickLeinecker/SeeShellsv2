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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI.Node;

namespace SeeShells.UI.EventFilters
{
    /// <summary>
    /// Filter's <see cref="Node.Node"/>s by a property in the <see cref="IEvent.Parent"/>'s <see cref="IShellItem.GetAllProperties"/> property.
    /// This generic filter allows for finding <see cref="Node.Node"/>'s that share a property that arent supported via a combination of other filter methods.
    /// 
    /// Example: to filter on IEvent's whos parent has the <see cref="IShellItem.TypeName"/> of "Volume" (aka 0x20 shellbags) the following would be performed:
    /// <code> var filteredList = FilterStringProperty(events, "Volume", false)</code>
    /// Because this uses <see cref="IShellItem.GetAllProperties"/> if a <see cref="IEvent.Name"/> also contained "Volume" it would also be returned by the filter.
    /// 
    /// This Filter accepts partial matches by default. based on the previous example, items following values would return if they existed: "Volume" & "Volume - Named"
    /// Regex searches also check for partial matches due the high variability in possible results. 
    /// Regex searches will time out after <paramref name="regexTimeoutInMilliseconds"/> milliseocnds of searching and return no results. (15 seconds by default)
    /// 
    /// On a large list of <see cref="Node.Node"/>'s or <see cref="IEvent.Parent"/>s, this operation can take a signficant amount of time.
    /// Caching results is HIGHLY recommened. 
    /// </summary>
    public class AnyStringFilter : INodeFilter
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly string value;
        private readonly bool useRegex;
        private readonly long regexTimeoutInMilliseconds;

        /// <summary>
        /// Filter's <see cref="Node.Node"/>s by a property in the <see cref="IEvent.Parent"/>'s <see cref="IShellItem.GetAllProperties"/> property.
        /// This generic filter allows for finding <see cref="Node.Node"/>'s that share a property that arent supported via a combination of other filter methods.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="useRegex">True if <paramref name="value"/> is a regex pattern to be matched. False for a standard <see cref="String.Contains(string)"/></param>
        /// <param name="regexTimeoutInMilliseconds">Timeout value in milliseconds where a Regex search will stop and return no value. Ignored if <paramref name="useRegex"/> is false.</param>
        public AnyStringFilter(string value, bool useRegex, long regexTimeoutInMilliseconds = 6000)
        {
            this.value = value;
            this.useRegex = useRegex;
            this.regexTimeoutInMilliseconds = regexTimeoutInMilliseconds;
        }
        public void Apply(ref List<Node.Node> nodes)
        {
            Regex regex = null;
            //if we've seen the parent, mark true if a previous search found the value - false otherwise.
            Dictionary<IShellItem, bool> seenParents = new Dictionary<IShellItem, bool>();

            //obtain all string values from the IEvent's Parent objects
            for (int i = nodes.Count - 1; i >= 0; i--) //iterate backwards because iterating forwards would be an issue with a list of changing size.
            {
                Node.Node node = nodes[i];
                IEvent nEvent = node.aEvent;

                //only check unique parent values 
                if (seenParents.ContainsKey(nEvent.Parent))
                {
                    //check if we need to remove this event becuase it DOES NOT has the property we've searched for
                    if (!seenParents[nEvent.Parent]) //aka !foundMatch
                    {
                        //nodes.Remove(node);
                        node.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                else
                { //grab all values from the parent's properties
                    //add all properties names and values to the searchable strings
                    var keyValues = nEvent.Parent.GetAllProperties();
                    bool foundMatch = false;
                    foreach (var key in keyValues)
                    {
                        //begin search 
                        if (useRegex)
                        {
                            try
                            {
                                regex = regex ?? new Regex(value, RegexOptions.None, TimeSpan.FromMilliseconds(regexTimeoutInMilliseconds));
                            }
                            catch (Exception ex) when (ex is ArgumentException || ex is RegexMatchTimeoutException) //catch parsing error and timeout
                            {
                                logger.Warn(ex);
                                nodes.ForEach(n => n.Visibility = System.Windows.Visibility.Collapsed); //no results if regex is broken.
                                return;
                            }

                            if (regex.IsMatch(key.Key) || (regex.IsMatch(key.Value)))
                            {
                                foundMatch = true;
                                break;
                            }
                        }
                        else
                        {
                            if (key.Key.Contains(value) || key.Value.Contains(value))
                            {
                                foundMatch = true;
                                break;
                            }
                        }
                    }

                    //if we found a match add result
                    if (!foundMatch)
                    {
                        node.Visibility = System.Windows.Visibility.Collapsed;

                    }
                    seenParents.Add(nEvent.Parent, foundMatch);

                }
            }
        }
    }
}
