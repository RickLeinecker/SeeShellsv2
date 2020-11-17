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
using SeeShells.UI.Node;
using SeeShells.UI.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.UI.Templates
{
    /// <summary>
    /// Interaction logic for Switch.xaml
    /// </summary>
    public partial class Switch : UserControl
    {
        public Switch()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Home.timelinePage == null)
            {
                timeline.IsEnabled = false;

            }
            else
            {
                string timelinePageKey = "timelinepage";
                if (App.pages.ContainsKey(timelinePageKey))
                {
                    App.NavigationService.Navigate(App.pages[timelinePageKey]);
                }
            }
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            string homepage = "homepage";
            if (App.pages.ContainsKey(homepage))
            {
                App.NavigationService.Navigate(App.pages[homepage]);
            }

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            string helppage = "helppage";
            if (App.pages.ContainsKey(helppage))
            {
                App.NavigationService.Navigate(App.pages[helppage]);
            }
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "CSV File (*.csv)|*.csv",
                ReadOnlyChecked = true
            };

            if (openFileDialog.ShowDialog() != true)
                return;
            var file = openFileDialog.FileName;
            List<IShellItem> csvShelltems = CsvIO.ImportCSVFile(file);
            if (csvShelltems.Count == 0)
            {
                LogAggregator.Instance.ShowIfNotEmpty();
                return;
            }

            if (App.ShellItems != null)
            {
                App.ShellItems.Clear();
            }
            if (App.nodeCollection.nodeList != null)
            {
                App.nodeCollection.nodeList.Clear();
            }

            App.ShellItems = csvShelltems;
            List<IEvent> events = EventParser.GetEvents(App.ShellItems);
            App.nodeCollection.ClearAllFilters();
            App.nodeCollection.nodeList.AddRange(NodeParser.GetNodes(events));

            if (Home.timelinePage == null)
            {
                Home.timelinePage = new TimelinePage();
                App.NavigationService.Navigate(Home.timelinePage);
            }
            else
            {
                Home.timelinePage.RebuildTimeline();
                string timelinePageKey = "timelinepage";
                if (App.pages.ContainsKey(timelinePageKey))
                {
                    App.NavigationService.Navigate(App.pages[timelinePageKey]);
                }
                else
                {
                    App.NavigationService.Navigate(Home.timelinePage);
                }
            }
            LogAggregator.Instance.ShowIfNotEmpty();
        }
    }
}
