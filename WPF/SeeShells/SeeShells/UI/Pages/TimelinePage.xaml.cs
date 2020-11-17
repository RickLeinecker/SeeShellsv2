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
using Microsoft.Win32;
using SeeShells.IO;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for TimelinePage.xaml
    /// </summary>
    public partial class TimelinePage : Page
    {
        private const string EVENT_PARENT_IDENTIFER = "EventParent";
        private HashSet<string> eventTypeList = new HashSet<string>();
        private HashSet<string> eventUserList = new HashSet<string>();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private int maxStackedNodes = 0;
        private TimeSpan maxRealTimeSpan = new TimeSpan(0, 0, 1, 0); // Max time in one timeline (1 min).

        private static TimelinePage timelinePage;
        private static NodeNavigation nodeNavigation;
        private static List<Object> allNodes;

        private static List<ToggleButton> toggledNodes = new List<ToggleButton>();
        private Boolean AutoScroll = true;

        public TimelinePage()
        {
            InitializeComponent();
            BuildTimeline();
            timelinePage = this;
        }

        private void AllStringFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RegexCheckBox != null) //null check to stop initalization NPEs
            {
                TextBox emitter = (TextBox)sender;
                bool useRegex = RegexCheckBox.IsChecked.GetValueOrDefault(false);
                UpdateFilter("AnyString", new AnyStringFilter(emitter.Text, useRegex));
            }
        }

        private void ClearEventContentFilter_Click(object sender, RoutedEventArgs e)
        {
            AllStringFilterTextBlock.Clear();
            RegexCheckBox.IsChecked = false;
        }

        private void UpdateDateFilter(object sender, SelectionChangedEventArgs e)
        {
            //set the end date's time to the end of the day
            DateTime? endTime = endDatePicker.SelectedDate?.AddHours(23).AddMinutes(59).AddSeconds(59) ??
                                endDatePicker.SelectedDate;
            UpdateFilter("DateFilter", new DateRangeFilter(startDatePicker.SelectedDate, endTime));

        }

        private void ClearDateFilter_Click(object sender, RoutedEventArgs e)
        {
            startDatePicker.SelectedDate = null;
            endDatePicker.SelectedDate = null;
        }

        private static void UpdateFilter(string filterIdentifer, INodeFilter newFilter)
        {
            //remove the current filter that exists
            App.nodeCollection.RemoveEventFilter(filterIdentifer);

            //add a new filter with our date restrictions
            App.nodeCollection.AddEventFilter(filterIdentifer, newFilter);

            //rebuild the timeline according to the new filters
            timelinePage.RebuildTimeline();
        }

        /// <summary>
        /// Updates the list of <seealso cref="EventFilters.EventTypeFilter"/>s when a change occurs
        /// </summary>
        private void EventTypeFilter_OnItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            CheckComboBox emitter = (CheckComboBox)sender;

            string[] items = emitter.SelectedItems.Cast<string>().ToArray();
            UpdateFilter("EventType", new EventTypeFilter(items));

        }


        private void EventTypeFilter_DropDownOpened(object sender, EventArgs e)
        {
            CheckComboBox emitter = (CheckComboBox)sender;

            if (eventTypeList.Count == 0)
            {
                foreach (Node.Node node in App.nodeCollection.nodeList)
                {
                    eventTypeList.Add(node.aEvent.EventType);
                }
            }

            emitter.ItemsSource = eventTypeList;
            emitter.Items.Refresh();

        }

        private void ClearEventTypeFilter_Click(object sender, RoutedEventArgs e)
        {
            EventTypeFilter.SelectedItems.Clear();
        }

        private void EventUserFilter_OnItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            CheckComboBox emitter = (CheckComboBox)sender;

            string[] items = emitter.SelectedItems.Cast<string>().ToArray();
            UpdateFilter("EventUser", new EventUserFilter(items));
        }

        private void EventUserFilter_DropDownOpened(object sender, EventArgs e)
        {
            CheckComboBox emitter = (CheckComboBox)sender;
            if (eventUserList.Count == 0)
            {
                //check if the owner property is on the node, then pull the user
                foreach (Node.Node node in App.nodeCollection.nodeList)
                {
                    string userID;
                    if (node.aEvent.Parent.GetAllProperties().TryGetValue("RegistryOwner", out userID))
                    {
                        eventUserList.Add(userID);
                    }
                }
            }

            emitter.ItemsSource = eventUserList;
            emitter.Items.Refresh();
        }

        private void ClearUserFilter_Click(object sender, RoutedEventArgs e)
        {
            EventUserFilter.SelectedItems.Clear();
        }

        public static void EventParentContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            IShellItem parent = (IShellItem)menuItem.Tag;
            timelinePage.EventParentTextBox.Text = "Filtering by: " + parent.Name;
            UpdateFilter(EVENT_PARENT_IDENTIFER, new EventParentFilter(parent));
        }

        private void EventParentClearButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateFilter(EVENT_PARENT_IDENTIFER, new EventParentFilter());
            EventParentTextBox.Text = string.Empty;
        }

        private void RegexCheckBox_Click(object sender, RoutedEventArgs e)
        {
            //force fire a text changed event for the textbox so that the filter gets updated
            AllStringFilter_TextChanged(AllStringFilterTextBlock, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None));
        }

        private void EventNameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox emitter = (TextBox)sender;
            UpdateFilter("EventName", new EventNameFilter(emitter.Text));

        }

        private void ClearEventNameFilter_Click(object sender, RoutedEventArgs e)
        {
            EventNameFilter.Clear();
        }

        private void NextNode_Click(object sender, RoutedEventArgs e)
        {
            nodeNavigation.GoToNextNode();
        }

        private void PrevNode_Click(object sender, RoutedEventArgs e)
        {
            nodeNavigation.GoToPreviousNode();
        }

        /// <summary>
        /// Builds a timeline dynamically. Creates one timeline for each cluster of events.
        /// </summary>
        private void BuildTimeline()
        {
            try
            {
                if(App.nodeCollection.nodeList.Count == 0)
                {
                    logger.Info("No nodes to draw on the timeline.");
                    return;
                }

                List<Node.Node> nodeList = new List<Node.Node>();
                DateTime eventTime = App.nodeCollection.nodeList[0].aEvent.EventTime;
                maxStackedNodes = 0;
                int currentMaxStackedNodes = 0;
                foreach (Node.Node node in App.nodeCollection.nodeList)
                {
                    if(node.aEvent.EventTime.Equals(eventTime))
                    {
                        currentMaxStackedNodes++;
                    }
                    else
                    {
                        if(currentMaxStackedNodes > maxStackedNodes)
                            maxStackedNodes = currentMaxStackedNodes;

                        currentMaxStackedNodes = 1;

                        eventTime = node.aEvent.EventTime;
                    }

                    node.Style = (Style)Resources["Node"];
                    if (node.Visibility == Visibility.Visible)
                        nodeList.Add(node);
                    node.block.Visibility = Visibility.Collapsed;
                }

                // if the last nodes were all the same time, we still need to check if they are the new highest maxStackedNodes
                if (currentMaxStackedNodes > maxStackedNodes)
                    maxStackedNodes = currentMaxStackedNodes;

                if (nodeList.Count == 0)
                {
                    EmptyTimeline.Visibility = Visibility.Visible;
                    logger.Info("All nodes are filtered out, no nodes to draw on the timeline.");
                    return;
                }
                else
                {
                    EmptyTimeline.Visibility = Visibility.Collapsed;
                }

                allNodes = new List<Object>();
                List<Node.Node> nodesCluster = new List<Node.Node>(); // Holds events for one timeline at a time.
                nodesCluster.Add(nodeList[0]);
                DateTime previousDate = nodeList[0].aEvent.EventTime;
                DateTime realTimeStart = DateTimeRoundDown(previousDate, maxRealTimeSpan);
                int nodeListSize = nodeList.Count;
                for (int i = 1; i < nodeListSize; i++)
                {
                    // If the event belongs to the timeline
                    if (TimeSpan.Compare(nodeList[i].aEvent.EventTime.Subtract(realTimeStart), maxRealTimeSpan) == -1) // Compare returns -1 if the first argument is less than the second
                    {
                        nodesCluster.Add(nodeList[i]);
                    }
                    else
                    {
                        AddTimeline(nodesCluster);
                        nodesCluster.Clear();

                        nodesCluster.Add(nodeList[i]);
                        previousDate = nodeList[i].aEvent.EventTime;
                        realTimeStart = DateTimeRoundDown(previousDate, maxRealTimeSpan);
                        if (i == nodeListSize - 1) // If it's the last event of nodeList.
                        {
                            AddTimeline(nodesCluster);
                            nodesCluster.Clear();
                        }
                    }
                }
                if (nodesCluster.Count != 0) // If all events belong to the same timeline.
                {
                    AddTimeline(nodesCluster);
                }
                InitializeNodeNavigation();
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, "Null nodeList" + "\n" + ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Creates a timeline with the given events and adds it to the UI.
        /// </summary>
        /// <param name="nodesCluster">list of events that belong in 1 timeline</param>
        private void AddTimeline(List<Node.Node> nodesCluster)
        {
            DateTime beginDate = DateTimeRoundDown(nodesCluster[0].aEvent.EventTime, maxRealTimeSpan);
            DateTime endDate = beginDate.AddMinutes(1);

            TimelinePanel timelinePanel = MakeTimelinePanel(beginDate, endDate);
            TimelinePanel blockPanel = MakeBlockPanel(beginDate, endDate);

            List<StackedNodes> stackedNodesList = GetStackedNodes(nodesCluster);

            // Add all nodes that stack onto a timeline
            foreach (StackedNodes stackedNode in stackedNodesList)
            {
                stackedNode.Click += DotPress;
                stackedNode.MouseEnter += HoverStackedNodes;
                stackedNode.MouseLeave += HoverStackedNodes;
                stackedNode.Style = (Style)Resources["StackedNode"];
                TimelinePanel.SetDate(stackedNode, stackedNode.events[0].EventTime);
                stackedNode.Content = stackedNode.events.Count.ToString();
                allNodes.Add(stackedNode);
                timelinePanel.Children.Add(stackedNode);
                ConnectNodeToTimeline(timelinePanel, stackedNode.events[0].EventTime);

                // Adds invisible blocks as padding for a nice vertical allignment.
                int numBlocksNeeded = maxStackedNodes - stackedNode.nodes.Count;
                TextBlock alignmentBlock = new TextBlock
                    {
                        Style = (Style)Resources["TimelineBlock"],
                        Visibility = Visibility.Collapsed,
                        Height = numBlocksNeeded * 70
                    };

                stackedNode.alignmentBlock = alignmentBlock;
                TimelinePanel.SetDate(stackedNode.alignmentBlock, stackedNode.nodes[0].GetBlockTime());
                blockPanel.Children.Add(stackedNode.alignmentBlock);

                // Adds the actual node blocks
                foreach (Node.Node node in stackedNode.nodes)
                {
                    node.IsChecked = false;
                    node.block.Style = (Style)Resources["TimelineBlock"];
                    TimelinePanel.SetDate(node.block, node.GetBlockTime());
                    blockPanel.Children.Add(node.block);
                }

            }
            // Add all other nodes onto a timeline
            foreach (Node.Node node in nodesCluster)
            {
                node.MouseEnter -= HoverNode;
                node.MouseLeave -= HoverNode;
                node.MouseEnter += HoverNode;
                node.MouseLeave += HoverNode;
                TimelinePanel.SetDate(node, node.aEvent.EventTime);
                allNodes.Add(node);
                timelinePanel.Children.Add(node);
                ConnectNodeToTimeline(timelinePanel, node.aEvent.EventTime);

                // Adds invisible block as padding for a nice vertical allignment.
                TextBlock alignmentBlock = new TextBlock
                    {
                        Style = (Style)Resources["TimelineBlock"],
                        Visibility = Visibility.Collapsed,
                        Height = (maxStackedNodes - 1) * 70
                    };

                node.alignmentBlock = alignmentBlock;
                TimelinePanel.SetDate(node.alignmentBlock, node.GetBlockTime());
                blockPanel.Children.Add(node.alignmentBlock);
                

                // Adds the actual node blocks
                node.IsChecked = false;
                node.block.Style = (Style)Resources["TimelineBlock"];
                TimelinePanel.SetDate(node.block, node.GetBlockTime());
                blockPanel.Children.Add(node.block);

            }

            Timelines.Children.Add(timelinePanel);

            Blocks.Children.Add(blockPanel);
            Line separationLine = MakeTimelineSeparatingLine();
            Line blockSeperation = MakeBlockPanelSeparation();
            Blocks.Children.Add(blockSeperation);
            Timelines.Children.Add(separationLine);
            AddTicks(beginDate, endDate);
            AddTimeStamp(beginDate, endDate);
        }

        /// <summary>
        /// Creates a TimelinePanel
        /// </summary>
        /// <param name="beginDate">begin date of timeline</param>
        /// <param name="endDate">end date of timeline</param>
        /// <returns>TimelinePanel that can space graphical objects according to time</returns>
        private TimelinePanel MakeTimelinePanel(DateTime beginDate, DateTime endDate)
        {
            TimelinePanel timelinePanel = new TimelinePanel
            {
                UnitTimeSpan = new TimeSpan(0, 0, 0, 1),
                UnitSize = App.nodeCollection.nodeList[0].Width,
                BeginDate = beginDate,
                EndDate = endDate,
                KeepOriginalOrderForOverlap = true
            };

            return timelinePanel;
        }

        /// <summary>
        /// Creates a TimelinePanel for Blocks
        /// </summary>
        /// <param name="beginDate">begin date of timeline</param>
        /// <param name="endDate">end date of timeline</param>
        /// <returns>TimelinePanel that can space Blocks according to time</returns>
        private TimelinePanel MakeBlockPanel(DateTime beginDate, DateTime endDate)
        {
            TimelinePanel timelinePanel = new TimelinePanel
            {
                UnitTimeSpan = new TimeSpan(0, 0, 0, 10),
                UnitSize = 200,
                BeginDate = beginDate,
                EndDate = endDate,
                KeepOriginalOrderForOverlap = true
            };

            return timelinePanel;
        }

        /// <summary>
        /// Gets all nodes that have the same EventTime out of a list that gets passed into the method and returns them in a list of StackedNodes.
        /// The list that gets passed in has the nodes that would stack deleted from it.
        /// </summary>
        /// <param name="nodesCluster">a list of nodes</param>
        /// <returns>list of StackedNodes and modifies the list of nodes that gets passed in</returns>
        private List<StackedNodes> GetStackedNodes(List<Node.Node> nodesCluster)
        {
            List<StackedNodes> stackedNodesList = new List<StackedNodes>();
            Node.Node previousNode = nodesCluster[0];
            int i = 1;
            while (i < nodesCluster.Count)
            {
                if (previousNode.aEvent.EventTime.Equals(nodesCluster[i].aEvent.EventTime))
                {
                    StackedNodes stackedNodes = new StackedNodes();
                    stackedNodes.events.Add(previousNode.aEvent);
                    stackedNodes.blocks.Add(previousNode.block);
                    while (i < nodesCluster.Count && previousNode.aEvent.EventTime.Equals(nodesCluster[i].aEvent.EventTime))
                    {
                        stackedNodes.events.Add(nodesCluster[i].aEvent);
                        stackedNodes.blocks.Add(nodesCluster[i].block);
                        previousNode = nodesCluster[i];
                        stackedNodes.nodes.Add(nodesCluster.ElementAt(i - 1));
                        nodesCluster.RemoveAt(i - 1);

                    }
                    stackedNodesList.Add(stackedNodes);

                    if (i < nodesCluster.Count) // If haven't reached the end of the list.
                    {
                        previousNode = nodesCluster[i];
                        stackedNodes.nodes.Add(nodesCluster.ElementAt(i - 1));
                        nodesCluster.RemoveAt(i - 1);
                    }
                    else
                    {
                        stackedNodes.nodes.Add(nodesCluster.ElementAt(i - 1));
                        nodesCluster.RemoveAt(i - 1); 
                    }
                }
                else
                {
                    previousNode = nodesCluster[i];
                    i++;
                }
            }
            return stackedNodesList;
        }

        /// <summary>
        /// Draws a line to connect a node to a timeline
        /// </summary>
        /// <param name="timelinePanel">timeline panel to hold and position connection lines</param>
        /// <param name="eventTime">time used as position for where to draw a connecting line</param>
        private void ConnectNodeToTimeline(TimelinePanel timelinePanel, DateTime eventTime)
        {
            Line connectorLine = new Line();
            connectorLine.Stroke = Brushes.LightSteelBlue;
            connectorLine.X1 = 10;
            connectorLine.X2 = 10;
            connectorLine.Y1 = 0;
            connectorLine.Y2 = 15;
            connectorLine.StrokeThickness = 1;

            TimelinePanel.SetDate(connectorLine, eventTime);
            timelinePanel.Children.Add(connectorLine);
        }

        /// <summary>
        /// Draws ticks below the nodes to represent the seconds of each timeline interval
        /// </summary>
        /// <param name="beginDate">begin date of a timeline period</param>
        /// <param name="endDate"> end date of a timeline period</param>
        private void AddTicks(DateTime beginDate, DateTime endDate)
        {
            TimelinePanel timelinePanel = MakeTimelinePanel(beginDate, endDate);

            AddTicksBar(beginDate, endDate);
            int timePeriod = (int)endDate.Subtract(beginDate).TotalSeconds;
            for (int i = 0; i < timePeriod; i++)
            {
                Line tick = new Line();
                tick.Stroke = Brushes.Black;
                tick.X1 = 10;
                tick.X2 = 10;
                tick.Y1 = 0;
                tick.Y2 = 20;
                tick.StrokeThickness = 1;

                TimelinePanel.SetDate(tick, beginDate);
                timelinePanel.Children.Add(tick);
                beginDate = beginDate.AddSeconds(1);
            }
            Ticks.Children.Add(timelinePanel);

            Line separationLine = MakeTimelineSeparatingLine();
            separationLine.Visibility = Visibility.Hidden; // Hidden since this line is added only for proper spacing
            Ticks.Children.Add(separationLine);
        }

        /// <summary>
        /// Adds a rectangular bar behind the timeline ticks.
        /// </summary>
        /// <param name="beginDate">begin date of the time interval used to decide the size of the bar</param>
        /// <param name="endDate">end date of the time interval used to decide the size of the bar</param>
        private void AddTicksBar(DateTime beginDate, DateTime endDate)
        {
            Rectangle bar = new Rectangle();
            bar.Height = 20;
            bar.Width = (endDate.Subtract(beginDate).TotalSeconds * App.nodeCollection.nodeList[0].Width) + 12; // The added integer is to compensate for the margins and thickness of the line that separates the timelines. 
            bar.Fill = Brushes.LightSteelBlue;
            bar.Margin = new Thickness(0, 0, -1, 0);

            TicksBar.Children.Add(bar);
        }

        /// <summary>
        /// Adds time stamp that tells the time period of a timeline
        /// </summary>
        /// <param name="beginDate">the begin date of the timeline</param>
        /// <param name="endDate">the end date of the timeline</param>
        private void AddTimeStamp(DateTime beginDate, DateTime endDate)
        {
            TextBlock timeStamp = new TextBlock();
            timeStamp.Text = beginDate.ToString() + " - " + endDate.ToString();
            timeStamp.Foreground = Brushes.White;
            timeStamp.Height = 20;
            timeStamp.Width = (endDate.Subtract(beginDate).TotalSeconds * App.nodeCollection.nodeList[0].Width) + 12;
            timeStamp.Margin = new Thickness(0, 0, -1, 0);

            TimeStamps.Children.Add(timeStamp);
        }

        /// <summary>
        /// Creates a line to be used as separation between timelines
        /// </summary>
        /// <returns>a line to visually separate timelines</returns>
        private Line MakeTimelineSeparatingLine()
        {
            Line separatingLine = new Line();
            separatingLine.Stroke = Brushes.LightSteelBlue;
            separatingLine.X1 = 0;
            separatingLine.X2 = 0;
            separatingLine.Y1 = 43;
            separatingLine.Y2 = 150;
            separatingLine.StrokeThickness = 2;
            separatingLine.HorizontalAlignment = HorizontalAlignment.Left;
            separatingLine.VerticalAlignment = VerticalAlignment.Center;
            separatingLine.Margin = new Thickness(5, 0, 5, 0);

            return separatingLine;
        }

        /// <summary>
        /// Creates a space to seperate the multiple timelines
        /// </summary>
        /// <returns>A appropriate space to separate the block panels on the timelines</returns>
        private Line MakeBlockPanelSeparation()
        {
            Line separatingSpace = new Line();
            separatingSpace.X1 = 0;
            separatingSpace.X2 = 1;
            separatingSpace.Y1 = 43;
            separatingSpace.Y2 = 150;
            separatingSpace.StrokeThickness = 2;
            separatingSpace.HorizontalAlignment = HorizontalAlignment.Left;
            separatingSpace.VerticalAlignment = VerticalAlignment.Center;
            separatingSpace.Margin = new Thickness(5, 0, 5, 0);

            return separatingSpace;
        }

        /// <summary>
        /// Rounds down a DateTime to the nearest TimeSpan
        /// </summary>
        /// <param name="date">date to round down</param>
        /// <param name="roundingFactor">TimeSpan to round down to</param>
        /// <returns>a rounded down DateTime</returns>
        private DateTime DateTimeRoundDown(DateTime date, TimeSpan roundingFactor)
        {
            long ticks = date.Ticks / roundingFactor.Ticks;
            return new DateTime(ticks * roundingFactor.Ticks);
        }

        /// <summary>
        /// Clears all children of UI objects and rebuilds the timeline.
        /// </summary>
        public void RebuildTimeline()
        {
            toggledNodes.Clear();

            foreach (Object child in Timelines.Children)
            {
                if(child is TimelinePanel) // Only TimelinePanel objects since Timelines also contains separating lines
                {
                    TimelinePanel timeline = (TimelinePanel)child;
                    timeline.Children.Clear();
                }
            }
            foreach (Object child in Blocks.Children)
            {
                if (child is TimelinePanel) // Only TimelinePanel objects since Timelines also contains separating lines
                {
                    TimelinePanel blockPanel = (TimelinePanel)child;
                    blockPanel.Children.Clear();
                }
            }
            Timelines.Children.Clear();
            Ticks.Children.Clear();
            TicksBar.Children.Clear();
            TimeStamps.Children.Clear();
            Blocks.Children.Clear();
            eventTypeList.Clear();
            eventUserList.Clear();
            BuildTimeline();
        }

        /// <summary>
        /// This checks when the download button is hit, whether the HTML and/or the CSV checkbox is checked or not and calls the creation of HtmlOutput.
        /// </summary>
        private void DownloadClick(object sender, RoutedEventArgs e)
        {
            if (htmlCheckBox.IsChecked ?? false)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = ".html";
                saveFileDialog1.Filter = "Html File (*.html)| *.html";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.ShowDialog();
                string name = saveFileDialog1.FileName;
                if (name != string.Empty)
                {
                    List<Node.Node> visibleNodes = App.nodeCollection.nodeList
                        .Where(node => node.Visibility == Visibility.Visible).ToList();
                    HtmlIO.OutputHtmlFile(visibleNodes, name);
                }
            }
            if (csvCheckBox.IsChecked ?? false)
            {
                SaveFileDialog saveFileDialog2 = new SaveFileDialog();
                saveFileDialog2.DefaultExt = ".csv";
                saveFileDialog2.Filter = "CSV File (*.csv)| *.csv";
                saveFileDialog2.AddExtension = true;
                saveFileDialog2.ShowDialog();
                string name2 = saveFileDialog2.FileName;
                if (name2 != string.Empty)
                {
                    var file = new FileInfo(name2);
                    if (file.Exists)
                    {
                        try // Check if the file is locked
                        {
                            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                stream.Close();
                            }
                        }
                        catch (IOException ex)
                        {
                            System.Windows.MessageBox.Show("The file: \"" + name2 + "\" is being used by another process.\n" +
                                "Select a different file name or close the process to save the CSV.", "Cannot save file", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            logger.Info(ex, "The file: \"" + name2 + "\" is being used by another process.\n" + ex.ToString());
                            return;
                        }
                    }
                    CsvIO.OutputCSVFile(App.ShellItems, name2);
                }
            }
        }

        /// <summary>
        /// This activates the toggle_block method built into the Node object. 
        /// </summary>
        public static void DotPress(object sender, EventArgs e)
        {
            try
            {
                nodeNavigation.SetNewStartingPoint(sender);
                if (sender.GetType() == typeof(Node.Node))
                {
                    unToggleEventsThatAreTooClose(((Node.Node)sender).GetBlockTime());
                    ((Node.Node)sender).ToggleBlock();

                    if (toggledNodes.Contains((Node.Node)sender))
                        toggledNodes.Remove((Node.Node)sender);
                    else
                        toggledNodes.Add((Node.Node)sender);
                }
                else if (sender.GetType() == typeof(StackedNodes))
                {
                    unToggleEventsThatAreTooClose(((StackedNodes)sender).GetBlockTime());
                    ((StackedNodes)sender).ToggleBlock();

                    if (toggledNodes.Contains((StackedNodes)sender))
                        toggledNodes.Remove((StackedNodes)sender);
                    else
                        toggledNodes.Add((StackedNodes)sender);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error with the toggled nodes.");
            }
            

        }

        private static void unToggleEventsThatAreTooClose(DateTime time)
        {
            for (int i=0; i<toggledNodes.Count; i++)
            {
                ToggleButton button = toggledNodes[i];

                if (button.GetType() == typeof(Node.Node))
                {
                    Node.Node node = (Node.Node)button;
                    DateTime nodesTime = node.GetBlockTime();
                    if (EventIsTooClose(nodesTime, time))
                    {
                        node.ToggleBlock();
                        toggledNodes.Remove(node);
                        i--;
                    }
                }
                else if (button.GetType() == typeof(StackedNodes))
                {
                    StackedNodes node = (StackedNodes)button;
                    DateTime nodesTime = node.GetBlockTime();
                    if (EventIsTooClose(nodesTime, time))
                    {
                        node.ToggleBlock();
                        toggledNodes.Remove(node);
                        i--;
                    }
                }
            }
        }

        private static bool EventIsTooClose(DateTime eventTime, DateTime comparison)
        {
            if ((eventTime.Ticks < comparison.Ticks) && (eventTime.Ticks >= (comparison.Ticks - TimeSpan.TicksPerSecond * 11)))
                return true;

            if ((eventTime.Ticks > comparison.Ticks) && (eventTime.Ticks <= (comparison.Ticks + TimeSpan.TicksPerSecond * 11)))
                return true;

           return false;
        }

        /// <summary>
        /// This activates the block of text to expand and show more information. 
        /// </summary>
        public static void HoverBlock(object sender, EventArgs e)
        {
            ((InfoBlock)sender).ToggleInfo();
        }

        /// <summary>
        /// This pops up context menu for a block.
        /// </summary>
        public static void ClickBlock(object sender, EventArgs e)
        {
            ((InfoBlock)sender).ContextMenu.PlacementTarget = sender as Button;
            ((InfoBlock)sender).ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// This pops up a new window with information from the block (selection in context menu).
        /// </summary>
        public static void popOut(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            InfoBlock block = (InfoBlock)menuItem.Tag;
            block.PopOutInfo();
        }

        /// <summary>
        /// This pops up a new window with information from the block.
        /// </summary>
        public static void LeftPopOut(object sender, EventArgs e)
        {
            ((InfoBlock)sender).PopOutInfo();
        }

        /// <summary>
        /// This is the Node version of this function to change the color of the blocks whose node is hovered over. 
        /// </summary>
        public void HoverNode(Object sender, EventArgs e)
        {
            if (((Node.Node)sender).IsChecked == true)
            {
                if (((Node.Node)sender).block.Style == (Style)Resources["TimelineBlock"])
                {
                    // change color
                    ((Node.Node)sender).block.Style = (Style)Resources["LitUpBlock"];
                }
                else 
                {
                    ((Node.Node)sender).block.Style = (Style)Resources["TimelineBlock"];
                }
            }
        }

        /// <summary>
        /// This is the StackedNode version of this function to change the color of the blocks whose StackedNode is hovered over. 
        /// </summary>
        public void HoverStackedNodes(Object sender, EventArgs e)
        {
            if (((StackedNodes)sender).IsChecked == true)
            {
                foreach(InfoBlock block in ((StackedNodes)sender).blocks)
                {
                    if (block.Style == (Style)Resources["TimelineBlock"])
                    {
                        // change color
                        block.Style = (Style)Resources["LitUpBlock"];
                    }
                    else
                    {
                        block.Style = (Style)Resources["TimelineBlock"];
                    }
                }
            }
        }


        // from: https://stackoverflow.com/questions/2984803/how-to-automatically-scroll-scrollviewer-only-if-the-user-did-not-change-scrol
        private void TimelineScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (TimelineScroll.VerticalOffset == TimelineScroll.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    AutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    AutoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (AutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                TimelineScroll.ScrollToVerticalOffset(TimelineScroll.ExtentHeight);
            }
        }

        /// <summary>
        /// Initializes the object used for single node navigation.
        /// </summary>
        private void InitializeNodeNavigation()
        {
            nodeNavigation = new NodeNavigation(allNodes);
        }
    }
}