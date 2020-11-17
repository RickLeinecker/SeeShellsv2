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
using System.Windows.Navigation;
using SeeShells.IO;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI;
using SeeShells.UI.Node;
using SeeShells.UI.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Reflection;
using System.Windows.Resources;
using SeeShells.UI.Pages;
using System.Windows.Input;
using NLog;

namespace SeeShells
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates an instance of the EventCollection class so that the whole program can access the list of events.
        /// </summary>
        public static EventCollection eventCollection = new EventCollection();

        /// <summary>
        /// Creates an instance of the NodeCollection class so that the whole program can access the list of nodes.
        /// </summary>
        public static NodeCollection nodeCollection = new NodeCollection();

        /// <summary>
        /// Collection of <see cref="ShellItem"/> which is populated after a parsing operation.
        /// </summary>
        public static List<IShellItem> ShellItems { get; set; }

        public static Dictionary<string, Page> pages = new Dictionary<string, Page>();


        public static NavigationService NavigationService;

        /// <summary>
        /// Removes the toolbar overflow side button.
        /// <see cref="https://stackoverflow.com/a/1051264"/>
        /// </summary>
        private void Toolbar_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ToolBar toolBar = sender as System.Windows.Controls.ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }

        public App()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "${basedir}/log.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0} - {1}", e.Exception.Message, e.Exception.StackTrace);
            logger.Fatal(errorMessage);
        }
    }
}
