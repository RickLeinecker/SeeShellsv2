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
using System;
using System.Collections.Generic;
using System.IO;
using SeeShells.ShellParser.Registry;

namespace SeeShellsTests.ShellParser
{
    [TestClass]
    public class OfflineRegistryReaderTests
    {
        /// <summary>
        /// Tests that the Offline Registry Reader can read from a NTUSER hive file.
        /// </summary>
        [TestMethod]
        public void GetRegistryKeys_NTUSERTest()
        {
            String registryFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\NTUSER.DAT";
            OfflineRegistryReader registryReader = new OfflineRegistryReader(new OfflineMockConfigParser(), registryFilePath);
            List<RegistryKeyWrapper> keys = registryReader.GetRegistryKeys();
            Assert.AreNotEqual(keys.Count, 0);
        }
        /// <summary>
        /// Tests that the Offline Registry Reader can read from a USRCLASS hive file.
        /// </summary>
        [TestMethod]
        public void GetRegistryKeys_USRCLASSTest()
        {
            String registryFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\UsrClass.dat";
            OfflineRegistryReader registryReader = new OfflineRegistryReader(new OfflineMockConfigParser(), registryFilePath);
            List<RegistryKeyWrapper> keys = registryReader.GetRegistryKeys();
            Assert.AreNotEqual(keys.Count, 0);
        }
    }
}
