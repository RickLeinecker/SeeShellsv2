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
using SeeShells.ShellParser;
using SeeShellsTests.ShellParser.ShellParserMocks;
using SeeShells.ShellParser.ShellItems;
using System.Collections.Generic;
using System.IO;
using System;
using SeeShells.ShellParser.Registry;

namespace SeeShellsTests.ShellParser
{
    [TestClass()]
    public class ShellBagParserTests
    {
        /// <summary>
        /// Tests if shell itmes can be obtained from a live registry.
        /// </summary>
        [TestMethod()]
        [TestCategory("OnlineTest")]
        public void GetShellItemsOnlineTest()
        {
            List<IShellItem> shellItems = ShellBagParser.GetShellItems(new OnlineRegistryReader(new MockConfigParser()));

            Assert.AreNotEqual(shellItems.Count, 0);
        }

        /// <summary>
        /// Tests if shell itmes can be obtained from an offline hive.
        /// </summary>
        [TestMethod()]
        public void GetShellItemsOfflineTest()
        {
            String registryFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\NTUSER.DAT";
            List<IShellItem> shellItems = ShellBagParser.GetShellItems(new OfflineRegistryReader(new OfflineMockConfigParser(), registryFilePath));

            Assert.AreNotEqual(shellItems.Count, 0);

            //test for username presence in the shellItems
            foreach (IShellItem shellItem in shellItems)
            {
                Assert.AreEqual("Klayton", shellItem.GetAllProperties()["RegistryOwner"]);
            }
        }
    }
}
