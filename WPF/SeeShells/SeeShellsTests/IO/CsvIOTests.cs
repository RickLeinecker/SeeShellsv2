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
using SeeShells.ShellParser;
using SeeShells.ShellParser.ShellItems;
using SeeShellsTests.ShellParser.ShellParserMocks;
using System;
using System.Collections.Generic;
using System.IO;

namespace SeeShellsTests.IO
{
    [TestClass]
    public class CsvIOTests
    {
        /// <summary>
        /// Tests if a CSV file is otputted.
        /// </summary>
        [TestMethod()]
        public void OutputCSVFileTest()
        {
            List<IShellItem> shellItems = new List<IShellItem>();
            Dictionary<string, string> shellItemProperties = new Dictionary<string, string>();
            shellItemProperties.Add("Size", "0");
            shellItemProperties.Add("Type", "31");
            shellItemProperties.Add("TypeName", "Some Type Name");
            shellItemProperties.Add("Name", "Some Name");
            shellItemProperties.Add("ModifiedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("AccessedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("CreationDate", "1/1/0001 12:00:00 AM");
            CsvParsedShellItem ShellItem = new CsvParsedShellItem(shellItemProperties);
            shellItems.Add(ShellItem);

            if (File.Exists("raw.csv"))
            {
                File.Delete("raw.csv");
            }
            CsvIO.OutputCSVFile(shellItems, "raw.csv");
            Assert.IsTrue(File.Exists("raw.csv"));
        }

        /// <summary>
        /// Tests if a CSV file is imported, parsed and converted to a list of ShellItems.
        /// </summary>
        [TestMethod()]
        public void ImportCSVFileTest()
        {
            List<IShellItem> shellItems = CsvIO.ImportCSVFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\raw.csv");
            Assert.AreNotEqual(shellItems.Count, 0);
        }

        /// <summary>
        /// Tests if CSV file with special characters is exported and imported correctly
        /// </summary>
        [TestMethod()]
        public void ExportAndImportCSVWithSpecialCharactersTest()
        {
            // Export
            List<IShellItem> shellItems = new List<IShellItem>();
            Dictionary<string, string> shellItemProperties = new Dictionary<string, string>();
            shellItemProperties.Add("Size", "0");
            shellItemProperties.Add("Type", "31");
            shellItemProperties.Add("TypeName", "Some Type Name");
            shellItemProperties.Add("Name", "Some Name, \n \"Name\"");
            shellItemProperties.Add("ModifiedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("AccessedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("CreationDate", "1/1/0001 12:00:00 AM");
            CsvParsedShellItem ShellItem = new CsvParsedShellItem(shellItemProperties);
            shellItems.Add(ShellItem);

            if (File.Exists("raw.csv"))
            {
                File.Delete("raw.csv");
            }
            CsvIO.OutputCSVFile(shellItems, "raw.csv");
            Assert.IsTrue(File.Exists("raw.csv"));

            // Import
            List<IShellItem> importedShellItems = CsvIO.ImportCSVFile("raw.csv");
            IDictionary<string, string> allProperties = importedShellItems[0].GetAllProperties();
            Assert.IsTrue(allProperties["Size"].Equals("0"));
            Assert.IsTrue(allProperties["Type"].Equals("31"));
            Assert.IsTrue(allProperties["TypeName"].Equals("Some Type Name"));
            Assert.IsTrue(allProperties["Name"].Equals("Some Name, \n \"Name\""));
            Assert.IsTrue(allProperties["ModifiedDate"].Equals("1/1/0001 12:00:00 AM"));
            Assert.IsTrue(allProperties["AccessedDate"].Equals("1/1/0001 12:00:00 AM"));
            Assert.IsTrue(allProperties["CreationDate"].Equals("1/1/0001 12:00:00 AM"));
        }
    }
}