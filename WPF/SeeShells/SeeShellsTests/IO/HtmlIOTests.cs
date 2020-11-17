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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.IO;
using SeeShells.UI;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SeeShellsTests.IO
{
    [TestClass]
    public class HtmlIOTests
    {
        /// <summary>
        /// Tests if a HTML file is otputted.
        /// </summary>
        [TestMethod()]
        public void OutputHtmlFileTest()
        {
            List<Node> nodeList = new List<Node>();

            Dictionary<string, string> shellItemProperties = new Dictionary<string, string>();
            shellItemProperties.Add("Size", "0");
            shellItemProperties.Add("Type", "31");
            shellItemProperties.Add("TypeName", "Some Type Name");
            shellItemProperties.Add("Name", "Some Name");
            shellItemProperties.Add("ModifiedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("AccessedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("CreationDate", "1/1/0001 12:00:00 AM");
            CsvParsedShellItem ShellItem = new CsvParsedShellItem(shellItemProperties);

            Event aEvent = new Event("item1", DateTime.Now, ShellItem, "Access");
            InfoBlock block = new InfoBlock();
            Node aNode = new Node(aEvent, block);
            nodeList.Add(aNode);

            if (File.Exists("timeline.html"))
            {
                File.Delete("timeline.html");
            }
            HtmlIO.OutputHtmlFile(nodeList, "timeline.html");
            Assert.IsTrue(File.Exists("timeline.html"));
        }
    }
}